using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DroidLord.Slavery;
using System.Threading;
using SharpAdbClient;
using System.Diagnostics;
using System.IO;
using DroidLord.Resource;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Data.Linq;
using System.Drawing.Drawing2D;
using Svg;
using System.Text.RegularExpressions;
using DroidLord.Core;
using DroidLord.Task;
using Newtonsoft.Json.Linq;
using DroidLord.Extension;

namespace DroidLord
{
    public partial class MainForm : MaterialForm
    {
        GraphicsPath parseSVG(string dat)
        {
            using (var s = new MemoryStream(UTF8Encoding.Default.GetBytes(dat)))
            {
                SvgDocument svgDoc = SvgDocument.Open<SvgDocument>(s, null);
                svgDoc.X = 0;
                svgDoc.Y = 0;
                var path = svgDoc.Path;
                var mat = new Matrix();
                mat.Scale(0.05f, 0.05f);
                path.Transform(mat);
                return path;
            }
        }
        void loadSettings()
        {
            lstSetting.Items.Clear();
            var conf = Program.GlobalSetting.List();
            foreach (var c in conf)
            {
                lstSetting.Items.Add(new ListViewItem(new string[] {
                    c.DisplayName,
                    c.GetDisplayType(),
                    c.Value.ToString()
                })
                { Tag = c.Name });
            }
        }
        public MainForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            materialTabSelector1.Icons = new GraphicsPath[] {
                // devices
                parseSVG("<svg width=\"120\" height=\"200\" xmlns=\"http://www.w3.org/2000/svg\"> <defs>  <style type=\"text/css\">   <![CDATA[@font-face { font-family: ifont; src: url(\"//at.alicdn.com/t/font_1442373896_4754455.eot?#iefix\") format(\"embedded-opentype\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.woff\") format(\"woff\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.ttf\") format(\"truetype\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.svg#ifont\") format(\"svg\"); }]]>  </style> </defs> <g>  <title>Layer 1</title>  <g id=\"svg_1\" class=\"transform-group\">   <g id=\"svg_2\" transform=\"translate(-28,0) scale(0.1953125) \">    <path id=\"svg_3\" fill=\"#272636\" d=\"m506.880003,419.84l35.84,-35.84c-102.4,-102.4 -261.12,-102.4 -363.52,0l35.84,35.84c81.92,-81.92 215.04,-81.92 291.84,0zm-35.84,35.84c-61.44,-61.44 -158.72,-61.44 -215.04,0l35.84,35.84c40.96,-40.96 102.4,-40.96 143.36,0l35.84,-35.84zm-71.68,71.68c-20.48,-20.48 -51.2,-20.48 -71.68,0l35.84,35.84l35.84,-35.84zm220.16,-527.36l-512,0c-56.32,0 -102.4,46.08 -102.4,102.4l0,716.8l0,102.4c0,56.32 46.08,102.4 102.4,102.4l512,0c56.32,0 102.4,-46.08 102.4,-102.4l0,-102.4l0,-716.8c0,-56.32 -46.08,-102.4 -102.4,-102.4zm-245.76,972.8c-30.72,0 -51.2,-20.48 -51.2,-51.2c0,-30.72 20.48,-51.2 51.2,-51.2s51.2,20.48 51.2,51.2c0,30.72 -20.48,51.2 -51.2,51.2zm245.76,-153.6l-512,0l0,-716.8l512,0l0,716.8z\"/>   </g>  </g> </g></svg>"),
                // scripts
                parseSVG("<?xml version=\"1.0\" standalone=\"no\"?><!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"><svg width=\"200\" height=\"200\" viewBox=\"0 0 200 200\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"><defs><style type=\"text/css\"><![CDATA[@font-face { font-family: ifont; src: url(\"//at.alicdn.com/t/font_1442373896_4754455.eot?#iefix\") format(\"embedded-opentype\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.woff\") format(\"woff\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.ttf\") format(\"truetype\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.svg#ifont\") format(\"svg\"); }]]></style></defs><g class=\"transform-group\"><g transform=\"scale(0.1953125, 0.1953125)\"><path d=\"M832 64l-12.8 0C825.6 64 825.6 64 832 64L832 64zM832 64c-6.4 0-6.4 0-12.8 0l-51.2 0-448 0c-108.8 0-192 83.2-192 192l0 448-25.6 0c-57.6 0-102.4 57.6-102.4 121.6l0 6.4c0 70.4 44.8 121.6 102.4 121.6l25.6 0 435.2 0 76.8 0c108.8 0 192-83.2 192-192l0-448 192 0 0-64C1024 166.4 940.8 70.4 832 64zM768 748.8c0 70.4-57.6 128-128 128l-38.4 0c0-12.8 0-25.6 0-38.4l0-38.4c0-44.8 6.4-57.6 38.4-76.8-12.8-12.8-57.6-19.2-76.8-19.2l-371.2 0 0-428.8c0-70.4 57.6-128 128-128l448 0 0 172.8L768 748.8zM307.2 294.4l0 44.8-44.8 0 0 38.4 44.8 0 0 262.4 51.2 0 0-262.4 51.2 0 0-38.4-51.2 0c0-32 0-51.2 6.4-64s12.8-12.8 25.6-12.8c0 0 6.4 0 6.4 0s6.4 0 12.8 0l0-44.8c-6.4 0-6.4 0-12.8 0s-6.4 0-12.8 0c-32 0-57.6 6.4-70.4 25.6C307.2 256 307.2 268.8 307.2 294.4zM486.4 640 556.8 524.8 633.6 640 697.6 640 588.8 486.4 691.2 339.2 627.2 339.2 556.8 448 492.8 339.2 422.4 339.2 524.8 486.4 416 640Z\" fill=\"#272636\"></path></g></g></svg>"),
                // contents
                parseSVG("<?xml version=\"1.0\" standalone=\"no\"?><!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"><svg width=\"200\" height=\"200\" viewBox=\"0 0 200 200\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"><defs><style type=\"text/css\"><![CDATA[@font-face { font-family: ifont; src: url(\"//at.alicdn.com/t/font_1442373896_4754455.eot?#iefix\") format(\"embedded-opentype\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.woff\") format(\"woff\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.ttf\") format(\"truetype\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.svg#ifont\") format(\"svg\"); }]]></style></defs><g class=\"transform-group\"><g transform=\"scale(0.1953125, 0.1953125)\"><path d=\"M85.943394 734.681662l54.867632 0 0 85.371366L83.590813 820.053028c-46.145993 0-83.569324-37.243229-83.569324-83.258239L0.021489 83.258239c0-45.923936 37.423331-83.258239 83.569324-83.258239l684.799578 0c46.145993 0 83.661422 37.33635 83.661422 83.258239l0 57.128115-85.784782 0L766.267031 85.600587 85.943394 85.600587 85.943394 734.681662 85.943394 734.681662zM1023.204891 254.276241l0 680.768775c0 45.923936-37.516452 83.167165-83.661422 83.167165L254.976182 1018.21218c-46.238091 0-83.708494-37.243229-83.708494-83.167165L171.267689 254.276241c0-45.923936 37.516452-83.167165 83.708494-83.167165l684.70748 0c46.007847 0.046049 83.523275 37.243229 83.523275 83.167165l0 0L1023.204891 254.276241zM937.375084 256.618589 257.189593 256.618589l0 676.222225 680.139441 0L937.329035 256.618589 937.375084 256.618589zM339.14414 456.110085l178.814572 0c3.968381 0 7.198959-3.214205 7.198959-7.164167l0-72.192206c0-0.276293 0-0.64366 0-0.917906 0-3.948939-3.229554-7.256264-7.105838-7.256264L339.145164 368.579543c-4.01443 0-7.198959 3.307326-7.198959 7.256264l0 73.110111c0 3.948939 3.183506 7.164167 7.198959 7.164167l0 0L339.14414 456.110085zM339.14414 638.655631l403.588895 0c3.968381 0 7.198959-3.214205 7.198959-7.164167l0-72.192206c0.092098-0.367367 0.092098-0.64366 0.092098-0.964978 0-3.948939-3.183506-7.164167-7.105838-7.164167L339.145164 551.170115c-4.01443 0-7.198959 3.214205-7.198959 7.164167l0 73.201186c0 3.857864 3.183506 7.119141 7.198959 7.119141l0 0L339.14414 638.655631zM339.14414 821.154106l516.091178 0c3.922333 0 7.198959-3.214205 7.198959-7.164167L862.434278 741.797733c0.046049-0.367367 0.046049-0.64366 0.046049-0.964978 0-3.948939-3.183506-7.164167-7.059789-7.164167L339.14414 733.668589c-4.01443 0-7.198959 3.214205-7.198959 7.164167l0 73.201186c0 3.90289 3.183506 7.119141 7.198959 7.119141l0 0L339.14414 821.154106z\" fill=\"#272636\"></path></g></g></svg>"),
                // settings
                parseSVG("<?xml version=\"1.0\" standalone=\"no\"?><!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"><svg width=\"200\" height=\"200\" viewBox=\"0 0 200 200\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"><defs><style type=\"text/css\"><![CDATA[@font-face { font-family: ifont; src: url(\"//at.alicdn.com/t/font_1442373896_4754455.eot?#iefix\") format(\"embedded-opentype\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.woff\") format(\"woff\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.ttf\") format(\"truetype\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.svg#ifont\") format(\"svg\"); }]]></style></defs><g class=\"transform-group\"><g transform=\"scale(0.2, 0.2)\"><path d=\"M915.0996 607.1172H1000V392.822265625h-84.900390625c-86.1211 0-106.6289-49.7441-45.8984-110.5352l60.1191-60.1191-151.5508-151.4902-60.0586 60.0586c-60.8516 60.8516-110.6563 40.2227-110.4746-45.7773 0-0.2441-0.123-0.3672-0.123-0.5488V0H392.9140625v85.205078125c-0.2441 85.8164-49.8652 106.3242-110.627 45.5332l-60.0898-60.0586L70.709 222.168l60.0586 60.1191c60.8223 60.791 40.1914 110.5352-45.7773 110.5352H0v214.294921875h84.9921875c85.9688 0 106.5977 49.7441 45.7773 110.5957l-60.0586 60.0586 151.4902 151.5508 60.0898-60.1191c60.7598-60.7305 110.3828-40.2227 110.627 45.5332V1000h214.203125v-84.412109375c0-0.1836 0.123-0.3672 0.123-0.5488-0.1836-85.998 49.6211-106.5684 110.4746-45.8379l60.0586 60.1191 151.5508-151.5508-60.1191-60.0586C808.4727 656.8613 828.9805 607.1172 915.0996 607.1172zM500 687.5c-103.5469 0-187.5-83.9844-187.5-187.5s83.9531-187.5 187.5-187.5c103.5156 0 187.5 83.9844 187.5 187.5S603.5156 687.5 500 687.5z\" fill=\"#272636\"></path></g></g></svg>"),
                // log
                parseSVG("<svg width=\"130\" height=\"200\" xmlns=\"http://www.w3.org/2000/svg\"> <defs>  <style type=\"text/css\">   <![CDATA[@font-face { font-family: ifont; src: url(\"//at.alicdn.com/t/font_1442373896_4754455.eot?#iefix\") format(\"embedded-opentype\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.woff\") format(\"woff\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.ttf\") format(\"truetype\"), url(\"//at.alicdn.com/t/font_1442373896_4754455.svg#ifont\") format(\"svg\"); }]]>  </style> </defs> <g>  <title>Layer 1</title>  <g id=\"svg_1\" class=\"transform-group\">   <g id=\"svg_2\" transform=\"translate(-1,0) scale(0.1953125) \">    <path id=\"svg_3\" fill=\"#272636\" d=\"m346.453336,85.33333l-341.33333,0c-46.93333,0 -84.90667,38.4 -84.90667,85.33334l-0.42666,682.66666c0,46.93334 37.97333,85.33334 84.90666,85.33334l512.42667,0c46.93333,0 85.33333,-38.4 85.33333,-85.33334l0,-512l-256,-256zm170.66667,768l-512,0l0,-682.66666l298.66667,0l0,213.33333l213.33333,0l0,469.33333zm-426.66667,-298.66666l341.33334,0l0,-85.33334l-341.33334,0l0,85.33334zm0,170.66666l341.33334,0l0,-85.33333l-341.33334,0l0,85.33333z\"/>   </g>  </g> </g></svg>"),
                // locations
                parseSVG("<?xml version=\"1.0\" standalone=\"no\"?><!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"><svg class=\"icon\" width=\"200px\" height=\"200.00px\" viewBox=\"0 0 1024 1024\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M490.837333 684.8c4.053333 5.973333 10.794667 9.514667 18.005333 9.514667 7.253333 0 13.994667-3.584 18.005333-9.514667l205.226667-303.189333c24.533333-38.784 37.504-83.370667 37.504-128.896 0-137.514667-116.949333-249.386667-260.736-249.386667-143.786667 0-260.736 111.872-260.736 249.386667 0 45.568 13.013333 90.24 38.016 129.664L490.837333 684.8zM508.842667 132.864c72.021333 0 130.389333 58.026667 130.389333 129.578667s-58.368 129.578667-130.389333 129.578667-130.389333-58.026667-130.389333-129.578667S436.864 132.864 508.842667 132.864z\"  /><path d=\"M39.509333 464.725333 39.509333 1018.794667 317.653333 986.197333 317.653333 559.274667 233.514667 441.984Z\"  /><path d=\"M508.842667 758.058667c-30.378667 0-58.666667-14.08-75.648-37.717333l-63.402667-88.362667 0 354.261333 278.144 32.597333 0-386.816-63.36 88.277333C567.552 743.936 539.264 758.058667 508.842667 758.058667z\"  /><path d=\"M774.144 456.021333 700.074667 559.274667 700.074667 1018.794667 978.176 986.197333 978.176 432.128Z\"  /></svg>")


            };
            MaterialSkinManager.Instance.AddFormToManage(this);
            // 搜索脚本目录
            Program.SearchScript();
            // 加载设备和脚本
            reloadScriptAndDevice();
            // 加载内容素材
            reloadContent();
            // 加载设置
            loadSettings();

            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://api.map.baidu.com/lbsapi/getpoint/index.html");
        }

        private void Contents_ResourceItemChanged(object sender, string Key)
        {
            reloadContent();
        }

        void reloadContent()
        {
            lstContent.Items.Clear();
            foreach (Content item in Program.Contents.Source)
            {
                lstContent.Items.Add(
                          new ListViewItem()
                          {
                              Text = item.Title,
                              Tag = item.Key
                          });
            }
        }

        private void Slaves_ResourceItemChanged(object sender, string Key)
        {
            reloadScriptAndDevice();
            var dev = Program.Slaves.Get(Key) as SlavedDevice;
            if (dev != null)
            {
                var slave = dev.Object;
                slave.Overseer.FileDetected += Overseer_FileDetected;
                // TODO: WATCH DIR
                slave.Overseer.DirFileDetected += Overseer_DirFileDetected;
            }
        }

        private void Overseer_DirFileDetected(Slave arg1, string arg2)
        {
            var sync = new SyncService(arg1.Device);
            var info = new FileInfo(arg2);
            var adb = new AdbClient(AdbServer.Instance.EndPoint);
            adb.ExecuteRemoteCommandSync(arg1.Device, $"su -c 'chmod 777 {arg2}'");
            sync.Pull(arg2, File.Create(Program.varDir + @"\snapshot\" + info.Name), CancellationToken.None);
        }

        private void Overseer_FileDetected(Slave arg1, string arg2)
        {
            var lines = Regex.Split(arg2, "\r\n");
            foreach (var l in lines)
            {
                if (string.IsNullOrWhiteSpace(l)) continue;

                var groups = l.Split('|');
                var level = Core.LogLevel.Message;
                if (groups[2].ToLower() == "error")
                {
                    level = Core.LogLevel.Error;
                }
                else if (groups[2].ToLower() == "warning")
                {
                    level = Core.LogLevel.Warning;
                }
                // Example:
                // 脚本调用发生错误|无法连接远程服务器,远程服务器返回(500)|ERROR|   
                var sb = new StringBuilder();
                for (int i = 2; i < groups.Length; i++)
                {
                    sb.Append($"| {groups[i]}");
                }         
                Program.Logs.WriteLog($"[设备:{arg1.Device.Serial}] {groups[0]} {sb}", groups[1], level, "remote");
            }
        }

        // 脚本有变
        private void Scripts_ResourceItemChanged(object sender, string Key)
        {
            reloadScriptAndDevice();
        }

        // 刷新列表
        void reloadScriptAndDevice()
        {
            lock (lstScripts)
            {
                lstScripts.Items.Clear();
                lstDevices.Items.Clear();
                // 加载脚本
                foreach (Script res in Program.Scripts.Source)
                {
                    var name = res.DisplayName;
                    lstScripts.Items.Add(new ListViewItem() { Text = name, Tag = res.Key });
                }
                // 加载设备
                foreach (SlavedDevice dev in Program.Slaves.Source)
                {
                    var serial = dev.Key;
                    lstDevices.Items.Add(serial);
                }
            }
        }

        // 设备上线
        void devOnline(DeviceData dev)
        {
            //  TODO : 抢红包，定位app

            // 判断设备数量
            var connected = Program.Repository.Temp["totalDev"] as int?;
            if (connected == null)
            {
                Program.Repository.Temp["totalDev"] = 1;
            }
            else
            {
                var priv = UAC.Privilege;
                var maxConnect = int.Parse(priv) * 30;
                if (connected > maxConnect)
                {
                    return;
                }
            }

            // 添加屏幕监控
            this.Invoke((ThreadStart)(() =>
            {
                if (Program.Slaves.Get(dev.Serial) == null)
                {
                    Program.Logs.WriteLog($"设备上线:{dev.Serial}");

                    var view = new SlaveViewer();
                    view.ResponseClick = false;
                    view.Margin = new Padding(10);
                    view.ShowMenu = true;
                    view.Parent = layoutPanel;
                    layoutPanel.Controls.Add(view);
                    view.Display(dev);
                    // 设备上线全局事件
                    Program.Events.Raise(view, "dev_online", dev);
                }
            }));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += " " + Application.ProductVersion;
            if ((bool)Program.GlobalSetting.Get("DefaultFullScreen").Value)
            {
                MaximizeWindow(true);
            }
            // UAC定时自动检测
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 2 * 60 * 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            // 设备上线全局事件
            Program.Events.Register("dev_online", new GlobalEventHandler((_sender, evName, param) =>
            {
                reloadScriptAndDevice();
            }));
            Program.Logs.LogWritten += Logs_LogWritten;
            Program.Scripts.ResourceItemChanged += Scripts_ResourceItemChanged;
            Program.Slaves.ResourceItemChanged += Slaves_ResourceItemChanged;
            Program.Contents.ResourceItemChanged += Contents_ResourceItemChanged;
            // 插入设备状态变更
            Program.Events.Register("dev_statuschange", new GlobalEventHandler((_sender, evtName, param) =>
            {
                var dev = param as DeviceData;
                // 连接后online
                if (dev.State == DeviceState.Online)
                {
                    devOnline(dev);
                }
            }));
            Program.Events.Register("adb_connect", new GlobalEventHandler((_sender, evtName, param) =>
            {
                var dev = param as DeviceData;
                // 连接上立马online, 一般是已插设备
                if (dev.State == DeviceState.Online)
                {
                    devOnline(dev);
                }
            }));
            Program.Events.Register("adb_disconnect", new GlobalEventHandler((_sender, name, param) =>
            {
                var dev = param as DeviceData;
                Program.Slaves.Remove(dev.Serial);
                // 查找屏幕监控并移除
                foreach (var v in layoutPanel.Controls)
                {
                    if (v is SlaveViewer)
                    {
                        var view = v as SlaveViewer;
                        if (view.GetSlave().Device.Serial == dev.Serial)
                        {
                            var sl = view.GetSlave();
                            sl.ScreenService.Stop();
                            this.Invoke((ThreadStart)(() =>
                                    layoutPanel.Controls.Remove(view)
                            ));
                        }
                    }
                }
                Program.Logs.WriteLog($"设备下线:{dev.Serial}");
            }));
            Dispatcher.BackgroundThread(() => Program.StartAdbMonitor());
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("UAC CHECKED");
            if (!UAC.KeepAlive())
            {
                Dispatcher.BackgroundThread(() =>
                {
                    Process.GetCurrentProcess().Kill();
                });
            }
        }

        void addLog(LogItem log)
        {
            var logTxt = $"[{log.CreateTime}][{log.Level}] {log.Message} {log.Desc}\r\n";
            txbLog.AppendText(logTxt);
            txbLog.Select(txbLog.Text.Length - (logTxt.Length - 1), logTxt.Length);
            if (log.Level == Core.LogLevel.Error || log.Level == Core.LogLevel.Exception || log.Level == Core.LogLevel.Fatal)
            {
                txbLog.SelectionColor = Color.FromArgb(169, 68, 66);
                txbLog.SelectionBackColor = Color.FromArgb(242, 222, 222);
            }
            else if (log.Level == Core.LogLevel.Warning)
            {
                txbLog.SelectionColor = Color.FromArgb(138, 109, 59);
                txbLog.SelectionBackColor = Color.FromArgb(252, 248, 227);
            }
            txbLog.ScrollToCaret();
            txbLog.SelectedText = "";
        }
        private void Logs_LogWritten(LogItem log)
        {
            // 只有与选中日志管理时动态更新日志， 节省资源
            if (tabControl1.SelectedIndex == 4)
            {
                if (log.Category == "default")
                {
                    if (radioAll.Checked)
                    {
                        addLog(log);
                    }
                    if (radioError.Checked && (log.Level == Core.LogLevel.Error || log.Level == Core.LogLevel.Exception || log.Level == Core.LogLevel.Fatal))
                    {
                        addLog(log);
                    }
                    if (radioWarning.Checked && log.Level == Core.LogLevel.Warning)
                    {
                        addLog(log);
                    }
                }
                else if (log.Category == "remote")
                {
                    if (radioRemote.Checked)
                    {
                        addLog(log);
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Program.Scripts.Clear();
            Program.SearchScript();
            reloadScriptAndDevice();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (lstScripts.SelectedItems.Count == 0) return;

            var curScript = lstScripts.SelectedItems[0];
            var resScript = Program.Scripts.Get(curScript.Tag as string) as Script;
            // 需要配置文件执行
            if (resScript.Configuration != null)
            {
                var require = JArray.Parse(resScript.Require);
                foreach (var req in require)
                {
                    var type = req["type"].ToString();
                    switch (type)
                    {
                        case "select":
                            // 需要选择框
                            var arr = (JArray)req["value"];
                            // 选择列表
                            var candidate = (from r in arr select r.ToString()).ToArray<string>();
                            var sel = new SelectForm();
                            var ti = req["title"].ToString();
                            sel.SetSelection(ti, ti, candidate);
                            if (sel.ShowDialog() == DialogResult.OK)
                            {
                                var idx = sel.GetSelection();
                                var varKey = req["return"].ToString();
                                resScript.SetValue(varKey, idx.ToString());
                                break;
                            }
                            return;
                        case "content":
                            var titleList = (from t in Program.Contents.ListAll().Cast<Content>() select t.Title).ToArray();
                            var idList = (from t in Program.Contents.ListAll().Cast<Content>() select t.Key).ToArray();

                            var coSel = new SelectForm();
                            coSel.SetSelection("素材", "素材", titleList);
                            if (coSel.ShowDialog() == DialogResult.OK)
                            {
                                var varKey = req["return"].ToString();
                                var idx = coSel.GetSelection();
                                var co = Program.Contents.Get(idList[idx]) as Content;
                                resScript.SetValue("selContent", co);
                                resScript.SetValue(varKey, co.Body.Replace("\r\n", "#ENTER#"));
                                break;
                            }
                            return;
                        case "input":
                            var ib = new InputForm();
                            var _default = req["value"].ToString();
                            var title = req["title"].ToString();
                            ib.SetDefault(_default, title, title);
                            if (ib.ShowDialog() == DialogResult.OK)
                            {
                                var keyword = ib.GetInput();
                                var variable = req["return"].ToString();
                                resScript.SetValue(variable, keyword);
                                break;
                            }
                            return;
                        default:
                            return;
                    }
                }
            }
            Dispatcher.BackgroundThread(() =>
            {
                btnSync_Click(null, null);
                Thread.Sleep(1000);
                foreach (ListViewItem dev in lstDevices.Items)
                {
                    if (dev.Selected)
                    {
                        var serial = dev.Text;
                        var slDev = Program.Slaves.Get(serial) as SlavedDevice;
                        var slave = slDev.Object;
                        // 如果有配置文件则生成配置文件
                        if (resScript.Configuration != null)
                        {
                            // 同步素材图片
                            syncContent(resScript.GetValue("selContent") as Content, slDev);


                            var configPath = Program.GlobalSetting.Get("TouchSpriteConfig").Value as string;
                            Dispatcher.BackgroundThread(() =>
                            {
                                var adb = new AdbClient(AdbServer.Instance.EndPoint);
                                adb.ExecuteRemoteCommand($"rm -rf {configPath}/*", slave.Device, null);
                            });
                            configPath += "/";
                            configPath += resScript.ConfigName;
                            // 生成配置文件
                            var config = resScript.ForgeConfig();
                            Dispatcher.BackgroundThread(() =>
                            {
                                lock (slave)
                                {
                                    var sync = new SyncService(slave.Device);
                                    File.WriteAllText(Program.varDir + $@"\tmp_{slave.Device.Serial}.config", config, Encoding.UTF8);
                                    sync.Push(File.OpenRead(Program.varDir + $@"\tmp_{slave.Device.Serial}.config"), configPath, 777, DateTime.Now, CancellationToken.None);
                                }
                            });
                        }
                        new Thread(() =>
                        {
                            Thread.Sleep(3000);
                            slave.ExecuteScript();
                        })
                        { IsBackground = true }.Start();
                    }
                }
            });
        }
        private void btnSync_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem script in lstScripts.Items)
            {
                if (script.Selected)
                {
                    foreach (ListViewItem dev in lstDevices.Items)
                    {
                        if (dev.Selected)
                        {
                            var serial = dev.Text;
                            var slDev = Program.Slaves.Get(serial) as SlavedDevice;
                            var slave = slDev.Object;
                            new Thread(() =>
                            {
                                slave.UploadScript((string)Program.Scripts.Get(script.Tag as string).GetValue("path"));
                            })
                            { IsBackground = true }.Start();
                        }
                    }
                }
            }
        }

        private void layoutPanel_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (var c in layoutPanel.Controls)
            {
                if (c is SlaveViewer)
                {
                    var view = c as SlaveViewer;
                    view.LayoutScrolled();
                }
            }
        }

        private void chkScript_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstScripts.Items)
            {
                item.Selected = chkScript.Checked;
                item.Focused = chkScript.Checked;
            }
        }

        private void chkDev_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstDevices.Items)
            {
                item.Focused = chkDev.Checked;
                item.Selected = chkDev.Checked;
            }
        }

        private void lstContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstContent.SelectedItems.Count == 0)
            {
                txbContent.Clear();
                txbCoTitle.Clear();
                contentAlbum.Controls.Clear();
                return;
            }
            var item = lstContent.SelectedItems[0];
            var coId = item.Tag as string;
            var co = Program.Contents.Get(coId) as Content;
            txbContent.Text = co.Body;
            txbCoTitle.Text = co.Title;
            editContentKey = co.Key;
            contentAlbum.Controls.Clear();
            foreach (var f in co.Album)
            {
                contentAlbum.Controls.Add(new PictureBox()
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Width = 128,
                    Height = 128,
                    ImageLocation = f,
                    ContextMenuStrip = albumMenu,
                    SizeMode = PictureBoxSizeMode.Zoom
                });
            }
        }

        string editContentKey = "";

        private void btnSaveContent_Click(object sender, EventArgs e)
        {
            var album = new List<string>();
            foreach (var ctl in contentAlbum.Controls)
            {
                var pic = ctl as PictureBox;
                if (pic != null)
                {
                    album.Add(pic.ImageLocation);
                }
            }
            var co = new Content()
            {
                Title = txbCoTitle.Text,
                Body = txbContent.Text,
                Key = editContentKey,
                Album = album
            };
            if (string.IsNullOrEmpty(co.Title) || string.IsNullOrEmpty(co.Body))
            {
                MessageBox.Show("名称与内容不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(editContentKey) || Program.Contents.Get(editContentKey) == null)
            {
                MessageBox.Show("编辑对象不存在！");
                return;
            }
            Program.Contents.Set(co.Key, co);
        }

        private void btnAddContent_Click(object sender, EventArgs e)
        {
            var album = new List<string>();
            foreach (var ctl in contentAlbum.Controls)
            {
                var pic = ctl as PictureBox;
                if (pic != null)
                {
                    album.Add(pic.ImageLocation);
                }
            }
            var co = new Content()
            {
                Title = txbCoTitle.Text,
                Body = txbContent.Text,
                Key = Guid.NewGuid().ToString().Replace("-", ""),
                Album = album
            };
            if (string.IsNullOrEmpty(co.Title) || string.IsNullOrEmpty(co.Body))
            {
                MessageBox.Show("名称与内容不能为空！");
                return;
            }
            Program.Contents.Add(co);
        }

        private void btnCoDel_Click(object sender, EventArgs e)
        {
            Program.Contents.Remove(editContentKey);
        }

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            var open = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "(*.jpg,*.png,*.jpeg,*.bmp,*.gif)|*.jpg;*.png;*.jpeg;*.bmp;*.gif| 所有文件 (*.*)|*.*"
            };
            if (open.ShowDialog() == DialogResult.OK)
            {
                var files = open.FileNames;
                foreach (var file in files)
                {
                    var pic = new PictureBox()
                    {
                        BorderStyle = BorderStyle.FixedSingle,
                        Width = 128,
                        Height = 128,
                        ImageLocation = file,
                        ContextMenuStrip = albumMenu,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    contentAlbum.Controls.Add(pic);
                }
            }
        }

        private void lstSetting_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstSetting.SelectedItems.Count == 0) return;
            var selectItem = lstSetting.SelectedItems[0];
            var frm = new SettingEdit();
            frm.SetRow(Program.GlobalSetting.Get(selectItem.Tag.ToString()));
            if (frm.ShowDialog() == DialogResult.OK)
            {
                loadSettings();
            }
        }

        void displayLog(List<LogItem> list)
        {
            txbLog.Clear();
            for (var i = 0; i < list.Count; i++)
            {
                addLog(list[i]);
            }
        }

        private void radioAll_CheckedChanged(object sender, EventArgs e)
        {
            showLogs();
        }

        void showLogs()
        {
            if (radioAll.Checked)
            {
                // display all category log
                var logs = Program.Logs.GetLogs("default");
                displayLog(logs);
            }
            if (radioError.Checked)
            {
                var logs = Program.Logs.GetLogs("default")
                .Where(l =>
                    l.Level == Core.LogLevel.Error ||
                    l.Level == Core.LogLevel.Exception ||
                    l.Level == Core.LogLevel.Fatal).ToList();
                displayLog(logs);
            }
            if (radioWarning.Checked)
            {
                // display all category log
                var logs = Program.Logs.GetLogs("default").Where(l => l.Level == Core.LogLevel.Warning).ToList();
                displayLog(logs);
            }
            if (radioRemote.Checked)
            {
                var logs = Program.Logs.GetLogs("remote");
                displayLog(logs);
            }
        }

        private void radioWarning_CheckedChanged(object sender, EventArgs e)
        {
            showLogs();
        }

        private void radioError_CheckedChanged(object sender, EventArgs e)
        {
            showLogs();
        }

        private void radioRemote_CheckedChanged(object sender, EventArgs e)
        {
            showLogs();
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            if (radioRemote.Checked)
            {
                Program.Logs.ClearLog("remote");
            }
            else
            {
                Program.Logs.ClearLog();
            }
            showLogs();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 4)
            {
                showLogs();
            }
        }

        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            foreach (ListViewItem item in lstContent.Items)
            {
                item.Selected = chk.Checked;
                item.Focused = chk.Checked;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            var menu = menuItem.Owner as ContextMenuStrip;
            var picBox = menu.SourceControl;
            contentAlbum.Controls.Remove(picBox);
        }

        void syncContent(Content co, SlavedDevice dev)
        {
            var album = co.Album;
            var ti = co.Title;
            var cont = co.Body;

            Dispatcher.BackgroundThread(() =>
            {
                var adb = new AdbClient(AdbServer.Instance.EndPoint);
                //try
                //{
                //    adb.ExecuteRemoteCommand($"mkdir {Program.GlobalSetting.Get("ContentPath").Value}", dev.Object.Device, null);
                //}
                //catch
                //{

                //}
                //try
                //{
                //    adb.ExecuteRemoteCommand($"rm -rf {Program.GlobalSetting.Get("ContentPath").Value}/*", dev.Object.Device, null);
                //}
                //catch
                //{

                //}
                try
                {
                    adb.ExecuteRemoteCommand($"rm -rf {Program.GlobalSetting.Get("DroidAlbum").Value}/*", dev.Object.Device, null);
                }
                catch
                {

                }
                var sync = new SyncService(dev.Object.Device);
                //File.WriteAllText(Program.varDir + "/tmp.dat", cont, Encoding.UTF8);
                //var stream = File.OpenRead(Program.varDir + "/tmp.dat");
                // sync.Push(stream, $"{Program.GlobalSetting.Get("ContentPath").Value}/{ti}-{co.Key}.txt", 777, DateTime.Now, CancellationToken.None);
                foreach (var pic in album)
                {
                    if (File.Exists(pic))
                    {
                        var info = new FileInfo(pic);
                        sync = new SyncService(dev.Object.Device);
                        sync.Push(File.OpenRead(pic), $"{Program.GlobalSetting.Get("DroidAlbum").Value}/{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10)}{info.Extension}", 777, DateTime.Now, CancellationToken.None);
                    }
                }
                // 广播相册图片改变消息
                adb.ExecuteRemoteCommand($"am broadcast -a android.intent.action.MEDIA_SCANNER_SCAN_FILE -d file://{Program.GlobalSetting.Get("DroidAlbum").Value}", dev.Object.Device, null);
                //File.Delete(Program.varDir + "/tmp.dat");
            });
        }

        private void btnSyncContent_Click(object sender, EventArgs e)
        {
            if (lstContent.Items.Count == 0) return;

            foreach (ListViewItem sel in lstContent.Items)
            {
                var co = Program.Contents.Get(sel.Tag.ToString()) as Content;
                foreach (SlavedDevice dev in Program.Slaves.Source)
                {
                    syncContent(co, dev);
                }
            }
        }

        private void 全部清除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            var menu = menuItem.Owner as ContextMenuStrip;
            contentAlbum.Controls.Clear();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
        bool loaded = false;
        private void webBrowser1_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!loaded)
            {
                loaded = true;

                var browser = webBrowser1;
                browser.Document.GetElementById("wrapper").Click += MainForm_Click;
                var tags = browser.Document.GetElementsByTagName("div");

                foreach (HtmlElement el in tags)
                {
                    // 暴力判断
                    if (el.OuterHtml.ToUpper().TrimStart('\r', '\n', ' ').StartsWith("<DIV CLASS=\"LOGOCON CLEAR\""))
                    {
                        el.Style = "display: none";
                    }
                }
            }
        }
        private void MainForm_Click(object sender, HtmlElementEventArgs e)
        {
            // 坐标点
            var loc = webBrowser1.Document.GetElementById("pointInput").GetAttribute("value");
            var pGroups = loc.Split(',');
            if (pGroups.Length == 2)
            {
                txbLocation.Text = pGroups[1] + "," + pGroups[0];
            }
        }

        private void btLoc_Click(object sender, EventArgs e)
        {

        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            var loc = txbLocation.Text;
            foreach (ListViewItem dev in lstDevices.Items)
            {
                if (dev.Selected)
                {
                    var serial = dev.Text;
                    var slave = Program.Slaves.Get(serial) as SlavedDevice;
                    slave.Object.UpdateLocation(txbLocation.Text);         
                }
            }
        }
    }
}