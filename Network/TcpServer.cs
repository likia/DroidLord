using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.IO;
using DroidLord.Task;

namespace DroidLord.Network
{
    public delegate void ServerConnectionCallBack(Socket client, object ext);
        
    public class TcpServer
    {
        public event ServerConnectionCallBack ConnectionEstablished;
        public event ServerConnectionCallBack ConnectionLost;
        public event ServerConnectionCallBack DataArrived;

        Hashtable connection;
        Socket server;
        Thread loop;

        public TcpServer(int port)
        {
            connection = new Hashtable();
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Any, port));
            server.Listen(255);
            loop = Dispatcher.BackgroundThread(() =>
            {
                proc();
            });
        }

        void proc()
        {
            while(true)
            {
                var clientConnection = server.Accept();
                ConnectionEstablished?.Invoke(clientConnection, null);
                var context = new TcpContext(clientConnection) { server = this };
                context.DataArrived += Context_DataArrived;
                var ip = (IPEndPoint)clientConnection.RemoteEndPoint;                
                connection[ip.Address] = clientConnection;
            }
        }
        public void NotifyConnectionLost(Socket client)
        {
            ConnectionLost?.Invoke(client, null);
        }
        private void Context_DataArrived(Socket client, object ext)
        {
            DataArrived?.Invoke(client, ext);
        }
    }
    /// <summary>
    /// 连接上下文
    /// </summary>
    public class TcpContext
    {
        public event ServerConnectionCallBack DataArrived;
        public Socket client;
        public TcpServer server;
        Thread loop;
        

        public TcpContext(Socket client)
        {
            this.client = client;
            loop = Dispatcher.BackgroundThread(() => {
                while (true)
                {
                    try
                    {
                        var buf = new byte[1024];
                        int len = 0;
                        var buffer = new MemoryStream();
                        while (true)
                        {
                            len = client.Receive(buf);
                            buffer.Write(buf, 0, len);
                            if (buf[len - 1] == 0x00)
                            {
                                // 字符串结束
                                break;
                            }
                        }
                        var bytes = buffer.ToArray();
                        var str = Encoding.UTF8.GetString(bytes);
                        DataArrived?.Invoke(client, str);
                    }
                    catch
                    {
                        server.NotifyConnectionLost(client);
                    }
                }
            });
        }
        ~TcpContext()
        {
            client.Close();
            loop.Abort();
        }
    }
}
