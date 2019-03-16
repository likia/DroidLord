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
using MaterialSkin.Controls;
using MaterialSkin;

namespace DroidLord
{
    public partial class ScreenForm : MaterialForm
    {
        SlaveViewer view;
        public ScreenForm()
        {
            InitializeComponent();
            MaterialSkinManager.Instance.AddFormToManage(this);
            view = new SlaveViewer();
            view.Left = 0;
            view.Top = 40;
            view.Width = Width+11;
            view.Height = Height - 1;
            view.ResponseClick = true;
            Controls.Add(view);
        }
        public void Display(Slavery.Slave sl)
        {
            view.Display(sl);
            view.Invalidate();
            this.Invalidate();
        }
        private void ScreenForm_Load(object sender, EventArgs e)
        {
        }

        private void hOMEToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            view.GetSlave().Manipulation.Keypress(Slavery.DroidKey.HOME);
        }

        private void 后退ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            view.GetSlave().Manipulation.Keypress(Slavery.DroidKey.BACK);
        }

        private void 菜单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            view.GetSlave().Manipulation.Keypress(Slavery.DroidKey.MENU);
        }

        private void 电源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 音量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            view.GetSlave().Manipulation.Keypress(Slavery.DroidKey.V_UP);
        }

        private void 音量ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            view.GetSlave().Manipulation.Keypress(Slavery.DroidKey.V_DOWN);
        }

        public void ScreenForm_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
