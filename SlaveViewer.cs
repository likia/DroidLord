using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpAdbClient;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace DroidLord
{
    public partial class SlaveViewer : UserControl
    {
        private Slavery.Slave slave;
        private Graphics graph;
        private Image curFrame = null;
        // 上次轮训窗口状态时间
        private DateTime lastQuery;

        // 显示右键菜单
        public bool ShowMenu
        {
            get; set;
        }             
            = false;

        // 响应鼠标事件
        public bool ResponseClick
        {
            get;
            set;
        }
        public Slavery.Slave GetSlave()
        {
            return slave;
        }
        public SlaveViewer()
        {
            InitializeComponent();

            // 鼠标事件
            ResponseClick = true;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 默认大小
            this.Width = 180;
            this.Height = 360;

            // 轮训窗口状态， 被遮挡则断开连接
            var timer = new System.Threading.Timer((obj) =>
            {
                // 轮询间隔保持1秒
                if (DateTime.Now.Subtract(lastQuery).TotalMilliseconds > 1000)
                {
                    LayoutScrolled();
                }
            }, null, 0, 1000);
        }

        public void LayoutScrolled()
        {
            if (slave != null)
            {
                lastQuery = DateTime.Now;

                var scRect = RectangleToScreen(ClientRectangle);
                if (IsOnScreen(scRect))                    
                {
                    // 滚动没有遮住
                    slave.ScreenService.Start();
                }
                else
                {
                    slave.ScreenService.Stop();
                }
            }
        }

        // Return True if at least the percent specified of the rectangle is shown on the total screen area of all monitors, otherwise return False.
        public bool IsOnScreen(System.Drawing.Rectangle Rec, double MinPercentOnScreen = 0.001)
        {
            double PixelsVisible = 0;

            foreach (Screen scrn in Screen.AllScreens)
            {
                System.Drawing.Rectangle r = System.Drawing.Rectangle.Intersect(Rec, scrn.WorkingArea);
                // intersect rectangle with screen
                if (r.Width != 0 & r.Height != 0)
                {
                    PixelsVisible += (r.Width * r.Height);
                    // tally visible pixels
                }
            }
            return PixelsVisible >= (Rec.Width * Rec.Height) * MinPercentOnScreen;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (curFrame != null)
            {
                lock (curFrame)
                {
                    lock (e.Graphics)
                    {
                        try
                        {
                            e.Graphics.DrawImage(curFrame,
                               new Rectangle(0, 0, Width, Height),
                               new Rectangle(0, 0, curFrame.Width, curFrame.Height), GraphicsUnit.Pixel);
                        }
                        catch 
                        {                        
                        }                        
                    }
                }
            }
        }
        public void Display(Slavery.Slave sl)
        {
            this.slave = sl;
            slave.ScreenService.FrameArrived += ScreenService_FrameArrived;
            graph = this.CreateGraphics();
        }
        public void Display(DeviceData dev)
        {
            graph = this.CreateGraphics();

            new Thread(() =>
            {
                slave = new Slavery.Slave(dev, 360, 720);
                slave.ScreenService.FrameArrived += ScreenService_FrameArrived;
                slave.ScreenService?.Start();

                // 加入设备管理器
                Program.Slaves.Add(new SlavedDevice() { Key = slave.Device.Serial, Object = slave });
            })
            { IsBackground = true }.Start();
        }
        // 鼠标坐标映射到触屏坐标
        Point transTouch(int x, int y)
        {
            var wid = (double)Width;
            var hei = (double)Height;
            var dstX = x / wid;
            var dstY = y / hei;
            return new 
                Point(
                (int)(dstX * slave.ScreenService.Width), 
                (int)(dstY * slave.ScreenService.Height));
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (ResponseClick && e.Button == MouseButtons.Left)
            {
                down = true;
                slave.Manipulation.TouchDown(transTouch(e.X, e.Y));
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (ResponseClick)
            {
                down = false;
                slave.Manipulation.TouchUp(transTouch(e.X, e.Y));
            }
        }
        bool down = false;
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right
                && ResponseClick)
            {
                if (down)
                {
                    slave.Manipulation.TouchMove(transTouch(e.X, e.Y));
                }
                var sc = Parent as ScreenForm;
                sc.ScreenForm_MouseMove(sender, e);
            }
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            slave.Manipulation.Keypress(e.KeyChar.ToString());
        }

        private bool ScreenService_FrameArrived(DeviceData device, object sender, Image frame)
        {
            if (this.Visible)
            {
                curFrame = frame;
                graph.DrawImage(frame,
                        new Rectangle(0, 0, Width, Height),
                        new Rectangle(0, 0, frame.Width, frame.Height), GraphicsUnit.Pixel);
                GC.Collect();
                return true;
            }
            return false;
        }

        private void 命令行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var adbPath = Program.usrDir + @"\bin\adb.exe";
            var serial = slave.Device.Serial;
            var proc = new Process()
            {               
                StartInfo = new ProcessStartInfo()
                {
                    FileName = adbPath,
                    Arguments = $"-s \"{serial}\" shell",
                    WindowStyle = ProcessWindowStyle.Normal                    
                }
            };           
            proc.Start();
        }

        private void SlaveViewer_MouseDown(object sender, MouseEventArgs e)
        {
            // 加载中...等待加载完毕
            while (slave == null)
            {
                Thread.Sleep(10);
            }
            if (e.Button == MouseButtons.Right)
            {
                if (!ShowMenu)
                {
                    // 只显示按键选项
                    for(int i = 0; i < devMenu.Items.Count; i++)
                    {
                        if (devMenu.Items[i].Name.Contains("keys"))
                        {
                            break;
                        }
                        devMenu.Items[i].Visible = false;
                    }
                }
                devMenu.Items[0].Text = slave.Device.Serial;
                devMenu.Show(this, new Point(e.X, e.Y));                               
            }
        }

        private void 控制设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 加载中...等待加载完毕
            while (slave == null)
            {
                Thread.Sleep(10);
            }

            var frm = new ScreenForm();
            var sla = slave;
            frm.Width = 360;
            frm.Height = 720;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Display(sla);
            frm.ShowDialog();
        }

        private void 删除设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Events.Raise(null, "adb_disconnect", slave.Device);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            slave.Manipulation.Keypress(Slavery.DroidKey.HOME);

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            slave.Manipulation.Keypress(Slavery.DroidKey.POWER);
            
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            slave.Manipulation.Keypress(Slavery.DroidKey.MENU);
        }

        private void 返回ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slave.Manipulation.Keypress(Slavery.DroidKey.BACK);

        }
    }
}
