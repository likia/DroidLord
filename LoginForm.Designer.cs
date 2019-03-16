namespace DroidLord
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new MaterialSkin.Controls.MaterialLabel();
            this.label2 = new MaterialSkin.Controls.MaterialLabel();
            this.txbUsr = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.txbPwd = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.btnLogin = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnReg = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialProgressBar1 = new MaterialSkin.Controls.MaterialProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Depth = 0;
            this.label1.Font = new System.Drawing.Font("Roboto", 11F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(54, 119);
            this.label1.MouseState = MaterialSkin.MouseState.HOVER;
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Depth = 0;
            this.label2.Font = new System.Drawing.Font("Roboto", 11F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(62, 183);
            this.label2.MouseState = MaterialSkin.MouseState.HOVER;
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "密码";
            // 
            // txbUsr
            // 
            this.txbUsr.Depth = 0;
            this.txbUsr.Hint = "";
            this.txbUsr.Location = new System.Drawing.Point(117, 119);
            this.txbUsr.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbUsr.MaxLength = 32767;
            this.txbUsr.MouseState = MaterialSkin.MouseState.HOVER;
            this.txbUsr.Name = "txbUsr";
            this.txbUsr.PasswordChar = '\0';
            this.txbUsr.SelectedText = "";
            this.txbUsr.SelectionLength = 0;
            this.txbUsr.SelectionStart = 0;
            this.txbUsr.Size = new System.Drawing.Size(271, 23);
            this.txbUsr.TabIndex = 2;
            this.txbUsr.TabStop = false;
            this.txbUsr.UseSystemPasswordChar = false;
            this.txbUsr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbUsr_KeyDown);
            // 
            // txbPwd
            // 
            this.txbPwd.Depth = 0;
            this.txbPwd.Hint = "";
            this.txbPwd.Location = new System.Drawing.Point(117, 179);
            this.txbPwd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbPwd.MaxLength = 32767;
            this.txbPwd.MouseState = MaterialSkin.MouseState.HOVER;
            this.txbPwd.Name = "txbPwd";
            this.txbPwd.PasswordChar = '●';
            this.txbPwd.SelectedText = "";
            this.txbPwd.SelectionLength = 0;
            this.txbPwd.SelectionStart = 0;
            this.txbPwd.Size = new System.Drawing.Size(271, 23);
            this.txbPwd.TabIndex = 3;
            this.txbPwd.TabStop = false;
            this.txbPwd.UseSystemPasswordChar = false;
            this.txbPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPwd_KeyDown);
            // 
            // btnLogin
            // 
            this.btnLogin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLogin.Depth = 0;
            this.btnLogin.Icon = null;
            this.btnLogin.Location = new System.Drawing.Point(66, 255);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLogin.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Primary = true;
            this.btnLogin.Size = new System.Drawing.Size(95, 36);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnReg
            // 
            this.btnReg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnReg.Depth = 0;
            this.btnReg.Icon = null;
            this.btnReg.Location = new System.Drawing.Point(293, 255);
            this.btnReg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnReg.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnReg.Name = "btnReg";
            this.btnReg.Primary = true;
            this.btnReg.Size = new System.Drawing.Size(95, 36);
            this.btnReg.TabIndex = 5;
            this.btnReg.Text = "注册";
            this.btnReg.UseVisualStyleBackColor = true;
            this.btnReg.Click += new System.EventHandler(this.btnReg_Click);
            // 
            // materialProgressBar1
            // 
            this.materialProgressBar1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.materialProgressBar1.BackColor = System.Drawing.Color.White;
            this.materialProgressBar1.Depth = 0;
            this.materialProgressBar1.Location = new System.Drawing.Point(0, 63);
            this.materialProgressBar1.Margin = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.materialProgressBar1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialProgressBar1.Name = "materialProgressBar1";
            this.materialProgressBar1.Size = new System.Drawing.Size(483, 5);
            this.materialProgressBar1.TabIndex = 6;
            this.materialProgressBar1.Visible = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 327);
            this.Controls.Add(this.materialProgressBar1);
            this.Controls.Add(this.btnReg);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txbPwd);
            this.Controls.Add(this.txbUsr);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户登录";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel label1;
        private MaterialSkin.Controls.MaterialLabel label2;
        private MaterialSkin.Controls.MaterialSingleLineTextField txbUsr;
        private MaterialSkin.Controls.MaterialSingleLineTextField txbPwd;
        private MaterialSkin.Controls.MaterialRaisedButton btnLogin;
        private MaterialSkin.Controls.MaterialRaisedButton btnReg;
        private MaterialSkin.Controls.MaterialProgressBar materialProgressBar1;
    }
}