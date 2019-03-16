using DroidLord.Task;
using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormAnimation;

namespace DroidLord
{
    public partial class LoginForm : MaterialForm
    {
        private bool Loadding = false;

        public LoginForm()
        {
            InitializeComponent();
            btnReg.AutoSize = false;
            btnLogin.AutoSize = false;
            btnReg.Invalidate();
            btnLogin.Invalidate();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);


            txbUsr.Text = (Program.Repository.Global["username"] ?? "").ToString();
            txbPwd.Text = (Program.Repository.Global["password"] ?? "").ToString();            
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            txbPwd.Enabled = false;
            txbUsr.Enabled = false;
            btnLogin.Enabled = false;
            btnReg.Enabled = false;
            LoadingProgress();
            Dispatcher.BackgroundThread(() =>
            {
                var ret = UAC.Login(txbUsr.Text, txbPwd.Text);
                Loadding = false;
                Program.Invoke(() =>
                {
                    txbPwd.Enabled = true;
                    txbUsr.Enabled = true;
                    btnLogin.Enabled = true;
                    btnReg.Enabled = true;
                });

                if (ret != null)
                {
                    var obj = ret as JObject;
                    // 失败了
                    if (ret is string)
                    {
                        MessageBox.Show(ret.ToString());
                        return;
                    }
                    else if (obj["privilege"].ToString() == "0")
                    {
                        MessageBox.Show("您的账号尚未授权, 请联系商家进行授权");
                        return;
                    }
                    else
                    {
                        Program.Repository.Global["username"] = txbUsr.Text;
                        Program.Repository.Global["password"] = txbPwd.Text;

                        // 成功, 并且有权限
                        UAC.Privilege = obj["privilege"].ToString();
                        UAC.Username = txbUsr.Text;
                        UAC.Password = txbPwd.Text;

                        Program.Invoke(() =>
                        {
                            this.Hide();
                            new MainForm().ShowDialog();
                            this.Close();
                        });
                    }
                }              
            });
        }
        void LoadingProgress()
        {
            Loadding = true;
            materialProgressBar1.Visible = true;
            materialProgressBar1.Invalidate();           

            // 非线性动效
            var animate = new Animator(new Path(0, 100, 1500, AnimationFunctions.CubicEaseInOut)) { Repeat = true };
            animate.Play(materialProgressBar1, Animator.KnownProperties.Value);
            Dispatcher.BackgroundThread(() =>
            {
                while (Loadding)
                {
                    Thread.Sleep(100);
                }
                Program.Invoke(() =>
                {
                    materialProgressBar1.Visible = false;
                    animate.Stop();
                });
            });
        }
        private void btnReg_Click(object sender, EventArgs e)
        {
            txbPwd.Enabled = false;
            txbUsr.Enabled = false;
            btnLogin.Enabled = false;
            btnReg.Enabled = false;
            LoadingProgress();
            Dispatcher.BackgroundThread(() =>
            {
                dynamic ret = UAC.Register(txbUsr.Text, txbPwd.Text);
                Loadding = false;
                Program.Invoke(() =>
                {
                    txbPwd.Enabled = true;
                    txbUsr.Enabled = true;
                    btnLogin.Enabled = true;
                    btnReg.Enabled = true;
                });
                if (ret != null)
                {
                    if (ret is string)
                    {
                        MessageBox.Show(ret);
                        return;
                    }
                    Program.Repository.Global["username"] = txbUsr.Text;
                    Program.Repository.Global["password"] = txbPwd.Text;
                    MessageBox.Show("注册成功! 请联系商家进行授权!");
                }               
            });
        }

        private void txbPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        private void txbUsr_KeyDown(object sender, KeyEventArgs e)
        {
            txbPwd_KeyDown(sender, e);
        }
    }
}
