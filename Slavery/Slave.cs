using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using SharpAdbClient;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using DroidLord.Extension;
using Newtonsoft.Json.Linq;
using DroidLord.Resource;
using DroidLord.Task;
using System.Net.Sockets;

namespace DroidLord.Slavery
{
    public class test : IShellOutputReceiver
    {
        public bool ParsesErrors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AddOutput(string line)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }
    }

    public class Slave : IShellOutputReceiver
    {
        public DeviceData Device;
        public Screen ScreenService;
        public Manipulate Manipulation;
        public Overseer Overseer;
        public int ScreenPort;
        public int ManipulatePort;
        public string ABI, SDK, REL;
        private bool flushed = false;        
        public int VirtualWidth = 360, VirtualHeight = 720;
        public int LocationPort = -1;
        Socket locClient;

        public bool ParsesErrors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Slave(DeviceData dev)
        {
            Device = dev;
            Enslave(VirtualWidth, VirtualHeight);
        }
        public Slave(DeviceData dev, int viWid, int viHei)
        {
            Device = dev;
            Enslave(viWid, viHei);
        }

        void Enslave(int virtualWid, int virtualHei)
        {
            Overseer = new Overseer(this);
            var adb = new AdbClient(AdbServer.Instance.EndPoint);
            // 初始化minicap            
            adb.ExecuteRemoteCommand("getprop", Device, this);
            while (!flushed)
            {
                Thread.Sleep(100);
            }
            if (int.Parse(SDK) > 23)
            {
                throw new NotSupportedException($"SDK版本不支持:{SDK}");
            }
            try
            {
                adb.ExecuteRemoteCommand("mkdir /data/local", Device, null);
            }
            catch
            {

            }
            try
            {
                adb.ExecuteRemoteCommand("mkdir /data/local/tmp", Device, null);
            }
            catch
            {

            }
            try
            {
                adb.ExecuteRemoteCommand("chmpod -R 777 /data/local/tmp/", Device, null);
            }
            catch
            {

            }
            try
            {
                adb.ExecuteRemoteCommand($"mkdir {Program.GlobalSetting.Get("ContentPath").Value}", Device, null);
            }
            catch
            {

            }
            // 上传动态链接库            
            new SyncService(Device).Push(
                File.OpenRead($@"{Program.usrDir + Program.DIR_LIBS}\android-{SDK}\{ABI}\minicap.so"),
                "/data/local/tmp/minicap.so", 777, DateTime.Now, CancellationToken.None);
            adb.ExecuteRemoteCommand("chmod 777 /data/local/tmp/minicap.so", Device, this);
            var exeFile = "minicap";
            if (int.Parse(SDK) < 16)
            {
                exeFile = "minicap-nopie";
            }
            // 上传可执行文件
            new SyncService(Device).Push(File.OpenRead(Program.usrDir + Program.DIR_EXE + ABI + @"\" + exeFile),
                "/data/local/tmp/minicap", 777, DateTime.Now, CancellationToken.None);
            adb.ExecuteRemoteCommand("chmod 777 /data/local/tmp/minicap", Device, this);
            // 分配端口
            ScreenPort = Program.getFreePort(Program.ScreenPortBase);
            ManipulatePort = Program.getFreePort(Program.MonkeyPortBase);
            var touchFile = "minitouch";
            if (int.Parse(SDK) < 16)
            {
                touchFile = "minitouch-nopie";
            }
            new SyncService(Device).Push(
                File.OpenRead($@"{Program.usrDir}\bin\lib\touch\{ABI}\{touchFile}"), "/data/local/tmp/minitouch", 777, DateTime.Now, CancellationToken.None);
            adb.ExecuteRemoteCommand("chmod 777 /data/local/tmp/minitouch", Device, this);

            adb.CreateForward(Device,
                new ForwardSpec() { Port = ManipulatePort, Protocol = ForwardProtocol.Tcp },
                new ForwardSpec() { Protocol = ForwardProtocol.LocalAbstract, SocketName = "minitouch" }, true);

            ScreenService = new Screen(this, virtualWid, virtualHei);
            Manipulation = new Manipulate(this);

            Overseer.WatchDirectory("/data/local/tmp/img/");
            Overseer.Watch(Program.GlobalSetting.Get("DroidLog").Value.ToString());
        }

        string getPropVal(string name, string input)
        {
            var mat = Regex.Match(input, @"\[" + name + @"\]\s*?:\s*?\[(.*?)\]");
            if (mat.Success)
            {
                return mat.Groups[1].Value;
            }
            return "";
        }

        public void AddOutput(string line)
        {               
            if (line.Contains("[ro.product.cpu.abi]"))
            {
                // 获取cpu架构 abi
                ABI = getPropVal("ro.product.cpu.abi", line);
            }
            if (line.Contains("[ro.build.version.sdk]"))
            {
                // 获取 sdk版本
                SDK = getPropVal("ro.build.version.sdk", line);
            }
            if (line.Contains("[ro.build.version.release]"))
            {
                // 获取 release info
                REL = getPropVal("ro.build.version.release", line);
            }
        }

        public void UploadScript(string path)
        {
            var adb = new AdbClient(AdbServer.Instance.EndPoint);
            adb.ExecuteRemoteCommand("rm -rf /sdcard/TouchSprite/lua/*", Device, this);
            var sync = new SyncService(Device);
            var info = new FileInfo(path);
            sync.Push(
                File.OpenRead(info.FullName), "/sdcard/TouchSprite/lua/" + System.Web.HttpUtility.UrlEncode(info.Name), 777, DateTime.Now, CancellationToken.None
            );
        }

        public void ExecuteScript()
        {
            Manipulation.Keypress(DroidKey.HOME);
            Thread.Sleep(500);
            var adb = new AdbClient(AdbServer.Instance.EndPoint);
            adb.ExecuteRemoteCommand("su -c 'am start com.touchsprite.android/.activity.Activity_Main'", Device, this);            
            Thread.Sleep(1000 * int.Parse(
                Program.GlobalSetting.Get("ExecuteScriptWait").Value.ToString()
            ));
            // 选中第一个脚本
            var firstScript = getRelative(0.4708333, 0.23671875);
            // 点开下拉菜单
            var actMenu = getRelative(0.925333333, 0.223125);
            // 运行脚本
            var startBtn = getRelative(0.768055555, 0.33515625);

            Manipulation.Touch(firstScript, () =>
            {
                Thread.Sleep(1000);
                Manipulation.Touch(actMenu, ()=> {
                    Thread.Sleep(1000);
                    Manipulation.Touch(startBtn);
                });                                
            });   
        }
        //public string ForgeConfig(int typeSelect, Content content)
        //{
        //    var root = new JObject();
        //    root["width"] = ScreenService.Width;
        //    root["height"] = ScreenService.Height;
        //    root["style"] = "default";
        //    root["timer"] = 2;
        //    root["selpage"] = 0;
        //    // 发送类型
        //    var viewType = new JObject();
        //    viewType["isAdd"] = "False";
        //    viewType["list"] = "0.无,1.文字朋友圈,2.图文朋友圈,3.文字群发,4.通讯录好友,5.附近的人";
        //    viewType["nowrap"] = 0;
        //    viewType["scale"] = 0;
        //    viewType["returnStr"] = typeSelect;
        //    viewType["select"] = typeSelect;
        //    viewType["size"] = 0;
        //    viewType["type"] = "ComboBox";
        //    viewType["width"] = 0;
        //    // 发送素材
        //    var viewContent = new JObject();
        //    viewContent["isAdd"] = "False";
        //    viewContent["prompt"] = content.Title;            
        //    viewContent["nowrap"] = 0;
        //    viewContent["scale"] = 0;
        //    viewContent["returnStr"]= viewContent["select"] = viewContent["text"] = content.Body;            
        //    viewContent["size"] = 0;
        //    viewContent["type"] = "Edit";
        //    viewContent["width"] = 0;
        //    // ??? 这是啥啊
        //    var viewCount = new JObject();
        //    viewCount["isAdd"] = "False";
        //    viewCount["list"] = "1,2,3,4,5,6,7,8,9";
        //    viewCount["nowrap"] = 0;
        //    viewCount["scale"] = 0;
        //    viewCount["returnStr"] = 0;
        //    viewCount["select"] = 0;
        //    viewCount["size"] = 0;
        //    viewCount["type"] = "ComboBox";
        //    viewCount["width"] = 0;

        //    root["views"] = new JArray(new object[] { viewType, viewContent, viewCount });
        //    return root.ToString();
        //}


        public void UpdateLocation(string loc)
        {
            if (LocationPort ==-1)
            {
                StartLocationService();
                return;
            }
            Dispatcher.BackgroundThread(() =>
            {
                try
                {
                    // 短连接发送经纬度信息
                    locClient.Send(Encoding.UTF8.GetBytes(loc + "\r\n"));
                }
                catch
                {

                }
            });
        }

        public void StartLocationService()
        {
            Dispatcher.BackgroundThread(() =>
            {
                var adb = new AdbClient(AdbServer.Instance.EndPoint);
                if (LocationPort == -1)
                {
                    adb.ExecuteRemoteCommand("am start com.github.fakegps/.ui.MainActivity", Device, null);
                    adb.CreateForward( Device, LocationPort = Program.getFreePort(Program.LocationBase), 9998);
                    Thread.Sleep(1000);
                    locClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    locClient.Connect("127.0.0.1", LocationPort);
                }
            });
        }

        Point getRelative(double x, double y)
        {
            return new Point((int)(x * ScreenService.Width), (int)(y * ScreenService.Height));
        }

        public void Flush()
        {
            flushed = true;
        }
    }
}
