using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpAdbClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using DroidLord.Task;

namespace DroidLord.Slavery
{
    public enum DroidKey
    {
        HOME = 3,
        BACK = 4,
        MENU = 1,
        V_UP = 24,
        V_DOWN = 25,
        POWER = 26
    }

    public class Manipulate :IShellOutputReceiver
    {
        private Slave slave;
        private Socket client;

        public bool ParsesErrors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Manipulate(Slave slave)
        {
            this.slave = slave;
            var adb = AdbClient.Instance;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            new Thread(() =>
            {
                new AdbClient(AdbServer.Instance.EndPoint)
                .ExecuteRemoteCommand($"/data/local/tmp/minitouch", slave.Device, this);
            })
            { IsBackground = true }.Start();
            client.Connect(IPAddress.Loopback, slave.ManipulatePort);
            sendCommand("hello", () => {
                Thread.Sleep(1000);
                sendCommand("world",()=> Touch(Point.Empty));
            });            
        }

        void sendCommand(string cmd, ThreadStart finished = null)
        {
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        if (!client.Connected)
                        {
                            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            client.Connect(IPAddress.Loopback, slave.ManipulatePort);
                        }
                        var buffer = Encoding.UTF8.GetBytes(cmd);
                        client.Send(buffer);
                        break;
                    }
                    catch (Exception ex)
                    {                        
                        Thread.Sleep(200);
                        Debug.WriteLine(ex.ToString());
                        //Program.Logs.WriteLog("无法连接远程操纵端口", ex.Message, Core.LogLevel.Exception);
                    }
                    finally
                    {
                      
                    }                   
                }
                // 处理完成回调
                if (finished != null)
                {
                    finished.Invoke();
                }
            })
            { IsBackground = true }.Start();
        }

        public void TouchDown(Point poi)
        {
            sendCommand($"d 0 {poi.X} {poi.Y} 255\nc\n");
            Debug.WriteLine($"POI:{poi.X}/{poi.Y} | {poi.X / (double)slave.ScreenService.Width}/{poi.Y / (double)slave.ScreenService.Height}");
        }


        public void Touch(Point poi, ThreadStart finished = null) 
        {
            sendCommand($"d 0 {poi.X} {poi.Y} 255\nc\n",
                () =>
                {
                    Thread.Sleep(32);
                    sendCommand($"u 0 {poi.X} {poi.Y} 255\nc\n", () =>
                    {
                        if (finished != null)
                        {
                            finished.Invoke();
                        }
                    });
                });
        }
        public void TouchMove(Point poi)
        {
            sendCommand($"m 0 {poi.X} {poi.Y} 255\nc\n");
        }
        public void TouchUp(Point poi)
        {
            sendCommand($"u 0 {poi.X} {poi.Y} 255\nc\n");
            Debug.WriteLine($"POI:{poi.X}/{poi.Y} | {poi.X / (double)slave.ScreenService.Width}/{poi.Y / (double)slave.ScreenService.Height}");
        }

        public void Keypress(DroidKey key)
        {
            Dispatcher.BackgroundThread(() =>
                new AdbClient(AdbServer.Instance.EndPoint)
                        .ExecuteRemoteCommand($"input keyevent {(int)key}", slave.Device, this));
        }                
        public void Keypress(string keyChar)
        {
            Dispatcher.BackgroundThread(() =>
              new AdbClient(AdbServer.Instance.EndPoint)
                        .ExecuteRemoteCommand($"input text {keyChar}", slave.Device, this));
        }

        public void AddOutput(string line)
        {
            Debug.WriteLine(line);
        }

        public void Flush()
        {
        }
    }
}
