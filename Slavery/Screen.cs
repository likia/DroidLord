using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpAdbClient;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace DroidLord.Slavery
{
    
    public delegate bool FrameBufferCallback(DeviceData device, object sender, Image frame);

    public class Screen : IShellOutputReceiver
    {
        private Slave _dev;
        private Socket client;
        private int port;
        public int Width, Height;
        private bool started = false;
        private bool flushed;
        // 垂直同步
        public int frameLimiter = 15;

        public bool ParsesErrors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event FrameBufferCallback FrameArrived;

        public Screen(Slave slave, int virtualWid, int virtualHei)
        {
            // initialize
            this.port = slave.ScreenPort;
            _dev = slave;
            // 获取屏幕大小
            var adb = new AdbClient(AdbServer.Instance.EndPoint);
            adb.ExecuteRemoteCommand("dumpsys window", slave.Device, this);
            // 等待执行完毕
            while (!flushed) { Thread.Sleep(100); }
            // 运行
            new Thread(() =>
            {
                var realSiz = $"{Width}x{Height}";
                var virtualSiz = $"{virtualWid}x{virtualHei}";
                // 启动minicap
                new AdbClient(AdbServer.Instance.EndPoint)
                    .ExecuteRemoteCommand($"LD_LIBRARY_PATH=/data/local/tmp/ /data/local/tmp/minicap -S -P {realSiz}@{virtualSiz}/0", slave.Device, this);
            })
            { IsBackground = true }.Start();
            // 等待minicap服务启动
            Thread.Sleep(1000);
            // 转发
            AdbClient.Instance.CreateForward(slave.Device,
                new ForwardSpec() { Port = port, Protocol = ForwardProtocol.Tcp },
                new ForwardSpec() { Protocol = ForwardProtocol.LocalAbstract, SocketName = "minicap" },
                true);

            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            new Thread(() => {
#if DEBUG
                var secAccu = 0.0;
                var frameAccu = 0;
#endif
                while (true)
                {
                    if (!started)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    try
                    {
                        var before = DateTime.Now;
                        // 帧前4字节为长度
                        var lenBuf = new byte[4];
                        client.Receive(lenBuf, 4, SocketFlags.None);
                        var length = BitConverter.ToInt32(lenBuf, 0);
                        if (length <= 0)
                        {
                            // wrong frame
                            continue;
                        }
                        if (length / 1024 > 1000)
                        {
                            continue;
                        }
                        var frameBuf = new byte[length];
                        int len = 0;
                        var ms = new MemoryStream();
                        while (ms.Length < length)
                        {
                            len = client.Receive(frameBuf, length - (int)ms.Length, SocketFlags.None);
                            ms.Write(frameBuf, 0, len);
                        }
                        var imgBuffer = ms.ToArray();
                        // jpeg header
                        if ((imgBuffer[0] != 0xff || imgBuffer[1] != 0xD8))
                        {
                            continue;
                        }
                        var img = Image.FromStream(ms, true, true);
                        FrameArrived?.Invoke(slave.Device, this, img);
                        GC.Collect();

                        var subTime = DateTime.Now.Subtract(before);
                        var frameTime = 1000 / frameLimiter;
                        //// 还没消耗够1frame所需的时间
                        //if (subTime.TotalMilliseconds < frameTime)
                        //{
                        //    Thread.Sleep(frameTime - (int)subTime.TotalMilliseconds);
                        //}
#if DEBUG
                        secAccu += subTime.TotalMilliseconds < frameTime ? frameTime : subTime.TotalMilliseconds;
                        if (secAccu >= 1000)
                        {
                            //Debug.WriteLine($"***************** frame rate: {frameAccu}");
                            frameAccu = 0;
                            secAccu = 0;
                        }
                        //Debug.WriteLine($"frame size: {imgBuffer.Length / 1024} kB.");
                        //Debug.WriteLine($"proc time: {subTime.TotalMilliseconds} ms.");
                        ++frameAccu;
#endif
                    }
                    catch(Exception ex)
                    {
                        Program.Logs.WriteLog("读取帧字节出错!", ex.Message, Core.LogLevel.Exception);
                    }
                }
            })
            { IsBackground = true }.Start();
        }

        public void Start()
        {
            lock (client)
            {
                if (!started)
                {
                    started = true;
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    client.Connect(IPAddress.Loopback, this.port);
                    // 读头部24字节信息
                    //byte[] headerBuf = new byte[24];
                    //client.Receive(headerBuf, 24, SocketFlags.None);
                }
            }
        }
        
        public void Stop()
        {
            lock (client)
            {
                if (started)
                {
                    started = false;
                    client.Close();                   
                }
            }
        }

        public void AddOutput(string line)
        {
            var resMatch = Regex.Match(line, @"init=(\d+)x(\d+)");
            if (!resMatch.Success)
            {
                var wMatch = Regex.Match(line, @"DisplayWidth=(\d+)");
                var hMatch = Regex.Match(line, @"DisplayHeight=(\d+)");
                if (wMatch.Success && hMatch.Success)
                {
                    // matched
                    Width = int.Parse(wMatch.Groups[1].Value);
                    Height = int.Parse(hMatch.Groups[1].Value);                    
                }
            }
            else
            {
                // matched
                Width = int.Parse(resMatch.Groups[1].Value);
                Height = int.Parse(resMatch.Groups[2].Value);
            }
            Debug.WriteLine(line);                      
        }

        public void Flush()
        {
            flushed = true;
        }
    }
}
