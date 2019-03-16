
using DroidLord.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DroidLord.Task;
using System.Text.RegularExpressions;
using SharpAdbClient;

namespace DroidLord.Slavery
{
    public class Overseer
    {
        Slave slave;
        public event Action<Slave, string> FileDetected;
        public event Action<Slave, string> DirFileDetected;

        public Overseer(Slave slave)
        {
            this.slave = slave;
        }

        public void WatchDirectory(string dir)
        {
            var adb = new AdbClient(AdbServer.Instance.EndPoint);
            try
            {
                adb.ExecuteRemoteCommand("mkdir /data/local/tmp/img", slave.Device, null);
            }
            catch { }
            Dispatcher.BackgroundThread(() =>
            {
                while (true)
                {
                    Thread.Sleep(500);
                    var o = AdbClient.Instance.ExecuteRemoteCommandSync(slave.Device, $"test -e {dir}/* && echo '1';");
                    if (o.Contains("1"))
                    {
                        o = adb.ExecuteRemoteCommandSync(slave.Device, $"ls -l {dir}/*");
                        var lines = Regex.Split(o, "\r\n");
                        foreach(var l in lines)
                        {
                            if (string.IsNullOrWhiteSpace(l)) continue;

                            var idx = l.LastIndexOf(" ") + 1;
                            var fileName = l.Substring(idx, l.Length - idx);
                            DirFileDetected?.Invoke(slave, dir + "/" + fileName);
                            adb.ExecuteRemoteCommandSync(slave.Device, $"su -c 'rm -f {dir}/{fileName}'");
                        }
                    }
                }
            });
        }

        public void Watch(string path)
        {
            Dispatcher.BackgroundThread(() =>
            {
                while (true)
                {
                    Thread.Sleep(500);
                    var cmdOut = AdbClient.Instance.ExecuteRemoteCommandSync(slave.Device, $"test -e {path} && echo '1';");
                    if (cmdOut.Contains("1"))
                    {
                        // 存在
                        var content = AdbClient.Instance.ExecuteRemoteCommandSync(slave.Device, $"su -c 'cat {path}'");
                        AdbClient.Instance.ExecuteRemoteCommandSync(slave.Device, $"su -c 'rm -f {path}'");
                        if (!string.IsNullOrEmpty(content))
                        {
                            FileDetected?.Invoke(slave, content);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            });
        }
    }
}
