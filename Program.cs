using System;
using System.Collections.Generic;
using System.Linq;
using DroidLord.Core;
using SharpAdbClient;
using DroidLord.Security;
using System.Windows.Forms;
using DroidLord.Task;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Text;
using DroidLord.Resource;
using Newtonsoft.Json.Linq;
using MaterialSkin;
using DroidLord.Network;

namespace DroidLord
{
    public static class Program
    {
        public static object JsonParse(string json)
        {
            try
            {
                return JObject.Parse(json);
            }
            catch
            {
                return null;               
            }
        }

        public const string DIR_LIBS = @"\bin\lib\share\";
        public const string DIR_EXE = @"\bin\lib\exe\";
        
        public static SecureHash MD5 = new SecureHash("md5");
        public static SecureHash SHA256 = new SecureHash("sha256");
        public static RSA RSA = new RSA();
        public static AES AES = new AES();
        public static EventManager Events = new EventManager();
        public static SettingManager GlobalSetting;
        public static Storage Repository = new Storage();        
        public static string etcDir = Environment.CurrentDirectory + @"\etc";
        public static string varDir = Environment.CurrentDirectory + @"\var";
        public static string usrDir = Environment.CurrentDirectory + @"\usr";
        public static string CONFIG_FILE = etcDir + @"\default.conf";
        public static Dispatcher Worker = new Dispatcher();
        public static int ScreenPortBase = 20000;
        public static int MonkeyPortBase = 23000;
        public static int LocationBase = 19000;

        private static Form mainFrm;
        // 脚本列表
        public static ResourceManager Scripts = new ResourceManager();
        // 设备列表
        public static ResourceManager Slaves = new ResourceManager();
        // 设备列表
        public static ResourceManager Contents = new ResourceManager();
        // 日志列表
        public static LogManager Logs = new LogManager();
        // tcp服务
        public static TcpServer RPCServer;
        // 后台持久任务
        public static Dispatcher Tasks = new Dispatcher();
        // 定时执行
        public static Ticker Ticker = new Ticker();
        public static bool StorageInited = false;        
        private static string DefaultStorage = varDir + @"\default.storage";
        private static string DefaultStorageLock = varDir + @"\default.storage.lock";
        public static SaveManager GlobalSave = new SaveManager(DefaultStorage);
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {          
            AdbServer.Instance.StartServer(usrDir + @"\bin\adb.exe", true);            
            LoadDefaultStorage();
            GlobalSave.Handle("Global", Repository);
            GlobalSave.Handle("Contents", Contents);
            GlobalSave.Handle("Logs", Logs);
            GlobalSetting = new SettingManager();
            GlobalSetting.LoadConfig(CONFIG_FILE);
            GlobalSave.Handle("Settings", GlobalSetting);
            AdbServer.Instance.StartServer(usrDir + @"\bin\adb.exe", false);
            Application.EnableVisualStyles();            
            Application.ApplicationExit += Application_ApplicationExit;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;     
            Application.ThreadException += Application_ThreadException;
            Application.SetCompatibleTextRenderingDefault(false);
            //RPCServer = new TcpServer((int)GlobalSetting.Get("RPCPort").Value);
            //RPCServer.DataArrived += RPCServer_DataArrived;
            ScreenPortBase = (int)GlobalSetting.Get("ScreenPortBase").Value;
            MonkeyPortBase = (int)GlobalSetting.Get("ManipulatePortBase").Value;
            // 全局错误处理器        
            Events.Register("on_exception", new GlobalEventHandler((_sender, _name, _param) =>
            {
                var ex = _param as Exception;
                checkExcetion(ex);
            }));

            Logs.RegisterEntry("remote");
            // 自动保存设置, 每分钟
            Dispatcher.StartTicking((o) =>
            {
                SaveStorages();
            }, 1000 * 60);            
            // free ports 
            Repository.Temp["screen"] = new Hashtable();
            for (var i = 0; i < 3000; i++)
            {
                Repository.Temp[ScreenPortBase + i] = true;
                Repository.Temp[MonkeyPortBase + i] = true;
                Repository.Temp[LocationBase + i] = true;
            }
            if (!UAC.Establish())
            {
               Dispatcher.BackgroundThread(() =>
                // 秘钥交换失败
                MessageBox.Show("无法与远程服务器取得联系.请稍后再试"));
                Thread.Sleep(3000);
                Process.GetCurrentProcess().Kill();
            }
            // 主题默认字体
            MaterialSkinManager.BaseFont = new System.Drawing.FontFamily("微软雅黑");
            var logFrm = new LoginForm();
            mainFrm = logFrm;
            Application.Run(logFrm);            
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject;
            File.AppendAllText(varDir + @"\err.log", ex.ToString(), Encoding.UTF8);
        }

        private static void RPCServer_DataArrived(System.Net.Sockets.Socket client, object ext)
        {
            Debug.WriteLine(ext as string);
        }

        static void Init()
        {
           
        }
        /// <summary>
        /// 主线程代理执行
        /// </summary>
        /// <param name="func"></param>
        public static void Invoke(ThreadStart func)
        {
            mainFrm.Invoke(func);
        }
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            UAC.Offline();
            SaveStorages();
        }

        public static void LoadDefaultStorage()
        {
            if (System.IO.File.Exists(DefaultStorage))
            {
                GlobalSave.LoadStorage(DefaultStorage);               
            }
            StorageInited = true;
        }
        public static void SaveStorages()
        {
            if (StorageInited)
            {                
                GlobalSave.SaveStorage(DefaultStorage);
                Log("saved to " + DefaultStorage + ".");
            }
        }

        static void checkExcetion(Exception e)
        {
            if (e.GetType().Name.ToLower().Contains("adb"))
            {
                Logs.WriteLog("ADB服务器崩溃...正在重启中...", e.ToString(), Core.LogLevel.Fatal);
                Events.Raise(null, "adb_dead", e);               
                AdbServer.Instance.RestartServer();
                new Thread(() => StartAdbMonitor()) { IsBackground = true }.Start();
            }
            if (e.InnerException != null)
            {
                checkExcetion(e.InnerException);
            }
            Logs.WriteLog("逻辑错误", e.ToString(), Core.LogLevel.Exception);
        }
        public static void SearchScript()
        {
            var noConfigfiles = Directory.GetFiles(varDir + @"\script\", "*.lua", SearchOption.TopDirectoryOnly);
            foreach (var f in noConfigfiles)
            {
                var info = new FileInfo(f);
                var key = Guid.NewGuid().ToString();
                Scripts.Add(key, new Script()
                {
                    FileName = info.Name,
                    DisplayName = info.Name.Replace(info.Extension, ""),
                    FullPath = info.FullName,
                    Content = File.ReadAllText(info.FullName, Encoding.Default)
                });
            }
            var configFiles = Directory.GetFiles(varDir + @"\script\", "*.lua", SearchOption.AllDirectories);
            foreach (var f in configFiles)
            {
                // 已经读过了
                if (noConfigfiles.Contains(f)) continue;                
                var info = new FileInfo(f);
                var conf = info.Directory + @"\config.json";
                if (!File.Exists(conf))
                {
                    // 没有配置文件                    
                    var key = Guid.NewGuid().ToString();
                    Scripts.Add(key, new Script()
                    {
                        FileName = info.Name,
                        DisplayName = info.Name.Replace(info.Extension, ""),
                        FullPath = info.FullName,
                        Content = File.ReadAllText(info.FullName, Encoding.Default)
                    });
                    continue;
                }
                var script = new Script()
                {
                    FileName = info.Name,
                    FullPath = info.FullName,
                    Content = File.ReadAllText(info.FullName, Encoding.Default)
                };
                // 解析配置
                script.ParseConfig(conf);
                Program.Scripts.Add(Guid.NewGuid().ToString(), script);
            }
        }
        public static void Log(string msg)
        {
            Debug.WriteLine($"[{DateTime.Now}] {msg}");
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Logs.WriteLog("FATAL ERROR", e.ToString(), Core.LogLevel.Fatal);
        }

        public static void StartAdbMonitor()
        {
            var monitor = new DeviceMonitor(new AdbSocket(AdbServer.Instance.EndPoint));
            monitor.DeviceConnected += Monitor_DeviceConnected;
            monitor.DeviceDisconnected += Monitor_DeviceDisconnected;
            monitor.DeviceChanged += Monitor_DeviceChanged;
            monitor.Start();
        }

        private static void Monitor_DeviceChanged(object sender, DeviceDataEventArgs e)
        {
            Events.Raise(sender, "dev_statuschange", e.Device);
        }

        public static int getFreePort(int Base)
        {           
            for (var i = Base; i < Base + 3000; i++)
            {                
                if ((bool)Repository.Temp[i])
                {
                    Repository.Temp[i] = false;
                    return i;
                }
            }
            return -1;
        }
        private static void Monitor_DeviceDisconnected(object sender, DeviceDataEventArgs e)
        {
            Events.Raise(sender, "adb_disconnect", e.Device);
        }

        private static void Monitor_DeviceConnected(object sender, DeviceDataEventArgs e)
        {
            Events.Raise(sender, "adb_connect", e.Device);
        }
    }
}
