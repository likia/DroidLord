using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DroidLord.Extension
{
    class CommandRecv : IShellOutputReceiver
    {
        public Action<string> Finished;

        StringBuilder result;

        public CommandRecv()
        {
            result = new StringBuilder();
        }

        public bool ParsesErrors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AddOutput(string line)
        {
            result.AppendLine(line);
        }

        public void Flush()
        {
            Finished?.Invoke(result.ToString());
        }
    }

    public static class AdbClientExtension
    {
        public static string ExecuteRemoteCommandSync(this IAdbClient client, DeviceData dev, string command)
        {
            var finished = false;
            var retv = "";
            var recv = new CommandRecv();
            recv.Finished = (result) =>
            {
                finished = true;
                retv = result;
            };
            client.ExecuteRemoteCommand(command, dev, recv);
            while (!finished)
            {
                Thread.Sleep(32);
            }
            return retv;
        }
    }
}
