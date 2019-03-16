using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpAdbClient;
using System.Threading;

namespace DroidLord
{
    public partial class ConsoleForm : Form , IShellOutputReceiver
    {
        private Slavery.Slave slave;
        private AdbClient adb;

        public bool ParsesErrors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ConsoleForm()
        {
            InitializeComponent();
        }

        public Form SetSlave(Slavery.Slave slave)
        {
            this.slave = slave;
            adb = new AdbClient(AdbServer.Instance.EndPoint);
            return this;
        }

        public void AddOutput(string line)
        {
            this.Invoke((ThreadStart)(() =>
            richTextBox1.AppendText(line + "\r\n")));
        }

        public void Flush()
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var txb = sender as TextBox;

                try
                {
                    new Thread(() =>
                       new AdbClient(AdbServer.Instance.EndPoint).ExecuteRemoteCommand(txb.Text, slave.Device, this)
                    ){ IsBackground = true }.Start();
                }
                catch (Exception ex)
                {
                    AddOutput($"E:{ex.Message}");
                }
                finally
                {
                    txb.Clear();
                }
            }
        }
    }
}
