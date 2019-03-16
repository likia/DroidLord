namespace DroidLord
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.layoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkDev = new MaterialSkin.Controls.MaterialCheckBox();
            this.chkScript = new MaterialSkin.Controls.MaterialCheckBox();
            this.btnSync = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnExecute = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnRefresh = new MaterialSkin.Controls.MaterialRaisedButton();
            this.lstDevices = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstScripts = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnSyncContent = new MaterialSkin.Controls.MaterialRaisedButton();
            this.contentAlbum = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOpenImage = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnCoDel = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnAddContent = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.txbCoTitle = new System.Windows.Forms.TextBox();
            this.btnSaveContent = new MaterialSkin.Controls.MaterialRaisedButton();
            this.txbContent = new System.Windows.Forms.TextBox();
            this.lstContent = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lstSetting = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnClearLog = new MaterialSkin.Controls.MaterialFlatButton();
            this.txbLog = new System.Windows.Forms.RichTextBox();
            this.radioRemote = new MaterialSkin.Controls.MaterialRadioButton();
            this.radioError = new MaterialSkin.Controls.MaterialRadioButton();
            this.radioWarning = new MaterialSkin.Controls.MaterialRadioButton();
            this.radioAll = new MaterialSkin.Controls.MaterialRadioButton();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.albumMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.全部清除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txbLocation = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.btLoc = new MaterialSkin.Controls.MaterialFlatButton();
            this.btnLocation = new MaterialSkin.Controls.MaterialRaisedButton();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.albumMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutPanel
            // 
            this.layoutPanel.AutoScroll = true;
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanel.Location = new System.Drawing.Point(3, 3);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.Size = new System.Drawing.Size(1262, 539);
            this.layoutPanel.TabIndex = 0;
            this.layoutPanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.layoutPanel_Scroll);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Depth = 0;
            this.tabControl1.Location = new System.Drawing.Point(4, 146);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.tabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1276, 571);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.layoutPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1268, 545);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设备预览";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.btnLocation);
            this.tabPage2.Controls.Add(this.chkDev);
            this.tabPage2.Controls.Add(this.chkScript);
            this.tabPage2.Controls.Add(this.btnSync);
            this.tabPage2.Controls.Add(this.btnExecute);
            this.tabPage2.Controls.Add(this.btnRefresh);
            this.tabPage2.Controls.Add(this.lstDevices);
            this.tabPage2.Controls.Add(this.lstScripts);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1268, 545);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "脚本管理";
            // 
            // chkDev
            // 
            this.chkDev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDev.AutoSize = true;
            this.chkDev.Depth = 0;
            this.chkDev.Font = new System.Drawing.Font("Roboto", 10F);
            this.chkDev.Location = new System.Drawing.Point(926, 5);
            this.chkDev.Margin = new System.Windows.Forms.Padding(0);
            this.chkDev.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkDev.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkDev.Name = "chkDev";
            this.chkDev.Ripple = true;
            this.chkDev.Size = new System.Drawing.Size(60, 30);
            this.chkDev.TabIndex = 6;
            this.chkDev.Text = "全选";
            this.chkDev.UseVisualStyleBackColor = true;
            this.chkDev.CheckedChanged += new System.EventHandler(this.chkDev_CheckedChanged);
            // 
            // chkScript
            // 
            this.chkScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkScript.AutoSize = true;
            this.chkScript.Depth = 0;
            this.chkScript.Font = new System.Drawing.Font("Roboto", 10F);
            this.chkScript.Location = new System.Drawing.Point(141, 5);
            this.chkScript.Margin = new System.Windows.Forms.Padding(0);
            this.chkScript.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkScript.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkScript.Name = "chkScript";
            this.chkScript.Ripple = true;
            this.chkScript.Size = new System.Drawing.Size(60, 30);
            this.chkScript.TabIndex = 5;
            this.chkScript.Text = "全选";
            this.chkScript.UseVisualStyleBackColor = true;
            this.chkScript.Visible = false;
            this.chkScript.CheckedChanged += new System.EventHandler(this.chkScript_CheckedChanged);
            // 
            // btnSync
            // 
            this.btnSync.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSync.Depth = 0;
            this.btnSync.Icon = null;
            this.btnSync.Location = new System.Drawing.Point(713, 4);
            this.btnSync.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSync.Name = "btnSync";
            this.btnSync.Primary = true;
            this.btnSync.Size = new System.Drawing.Size(90, 39);
            this.btnSync.TabIndex = 4;
            this.btnSync.Text = "同步脚本";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExecute.Depth = 0;
            this.btnExecute.Icon = null;
            this.btnExecute.Location = new System.Drawing.Point(617, 5);
            this.btnExecute.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Primary = true;
            this.btnExecute.Size = new System.Drawing.Size(90, 39);
            this.btnExecute.TabIndex = 3;
            this.btnExecute.Text = "执行脚本";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRefresh.Depth = 0;
            this.btnRefresh.Icon = null;
            this.btnRefresh.Location = new System.Drawing.Point(521, 4);
            this.btnRefresh.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Primary = true;
            this.btnRefresh.Size = new System.Drawing.Size(90, 39);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新列表";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lstDevices
            // 
            this.lstDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDevices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstDevices.CheckBoxes = true;
            this.lstDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lstDevices.Depth = 0;
            this.lstDevices.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.lstDevices.FullRowSelect = true;
            this.lstDevices.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstDevices.Location = new System.Drawing.Point(816, 0);
            this.lstDevices.MouseLocation = new System.Drawing.Point(-1, -1);
            this.lstDevices.MouseState = MaterialSkin.MouseState.OUT;
            this.lstDevices.Name = "lstDevices";
            this.lstDevices.OwnerDraw = true;
            this.lstDevices.Size = new System.Drawing.Size(449, 540);
            this.lstDevices.TabIndex = 1;
            this.lstDevices.UseCompatibleStateImageBehavior = false;
            this.lstDevices.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "设备名";
            this.columnHeader2.Width = 381;
            // 
            // lstScripts
            // 
            this.lstScripts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstScripts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstScripts.CheckBoxes = true;
            this.lstScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstScripts.Depth = 0;
            this.lstScripts.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.lstScripts.FullRowSelect = true;
            this.lstScripts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstScripts.Location = new System.Drawing.Point(0, 0);
            this.lstScripts.MouseLocation = new System.Drawing.Point(-1, -1);
            this.lstScripts.MouseState = MaterialSkin.MouseState.OUT;
            this.lstScripts.MultiSelect = false;
            this.lstScripts.Name = "lstScripts";
            this.lstScripts.OwnerDraw = true;
            this.lstScripts.Size = new System.Drawing.Size(512, 545);
            this.lstScripts.TabIndex = 0;
            this.lstScripts.UseCompatibleStateImageBehavior = false;
            this.lstScripts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "脚本文件";
            this.columnHeader1.Width = 431;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnSyncContent);
            this.tabPage3.Controls.Add(this.contentAlbum);
            this.tabPage3.Controls.Add(this.btnOpenImage);
            this.tabPage3.Controls.Add(this.btnCoDel);
            this.tabPage3.Controls.Add(this.btnAddContent);
            this.tabPage3.Controls.Add(this.materialLabel1);
            this.tabPage3.Controls.Add(this.txbCoTitle);
            this.tabPage3.Controls.Add(this.btnSaveContent);
            this.tabPage3.Controls.Add(this.txbContent);
            this.tabPage3.Controls.Add(this.lstContent);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1268, 545);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "素材管理";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnSyncContent
            // 
            this.btnSyncContent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSyncContent.Depth = 0;
            this.btnSyncContent.Icon = null;
            this.btnSyncContent.Location = new System.Drawing.Point(867, 4);
            this.btnSyncContent.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSyncContent.Name = "btnSyncContent";
            this.btnSyncContent.Primary = true;
            this.btnSyncContent.Size = new System.Drawing.Size(94, 45);
            this.btnSyncContent.TabIndex = 10;
            this.btnSyncContent.Text = "同步素材";
            this.btnSyncContent.UseVisualStyleBackColor = true;
            this.btnSyncContent.Click += new System.EventHandler(this.btnSyncContent_Click);
            // 
            // contentAlbum
            // 
            this.contentAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contentAlbum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contentAlbum.Location = new System.Drawing.Point(984, 3);
            this.contentAlbum.Name = "contentAlbum";
            this.contentAlbum.Size = new System.Drawing.Size(281, 542);
            this.contentAlbum.TabIndex = 9;
            // 
            // btnOpenImage
            // 
            this.btnOpenImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOpenImage.Depth = 0;
            this.btnOpenImage.Icon = null;
            this.btnOpenImage.Location = new System.Drawing.Point(767, 4);
            this.btnOpenImage.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnOpenImage.Name = "btnOpenImage";
            this.btnOpenImage.Primary = true;
            this.btnOpenImage.Size = new System.Drawing.Size(94, 45);
            this.btnOpenImage.TabIndex = 8;
            this.btnOpenImage.Text = "选取图片";
            this.btnOpenImage.UseVisualStyleBackColor = true;
            this.btnOpenImage.Click += new System.EventHandler(this.btnOpenImage_Click);
            // 
            // btnCoDel
            // 
            this.btnCoDel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCoDel.Depth = 0;
            this.btnCoDel.Icon = null;
            this.btnCoDel.Location = new System.Drawing.Point(667, 4);
            this.btnCoDel.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnCoDel.Name = "btnCoDel";
            this.btnCoDel.Primary = true;
            this.btnCoDel.Size = new System.Drawing.Size(94, 45);
            this.btnCoDel.TabIndex = 7;
            this.btnCoDel.Text = "删除素材";
            this.btnCoDel.UseVisualStyleBackColor = true;
            this.btnCoDel.Click += new System.EventHandler(this.btnCoDel_Click);
            // 
            // btnAddContent
            // 
            this.btnAddContent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddContent.Depth = 0;
            this.btnAddContent.Icon = null;
            this.btnAddContent.Location = new System.Drawing.Point(567, 4);
            this.btnAddContent.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAddContent.Name = "btnAddContent";
            this.btnAddContent.Primary = true;
            this.btnAddContent.Size = new System.Drawing.Size(94, 45);
            this.btnAddContent.TabIndex = 6;
            this.btnAddContent.Text = "添加素材";
            this.btnAddContent.UseVisualStyleBackColor = true;
            this.btnAddContent.Click += new System.EventHandler(this.btnAddContent_Click);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(463, 76);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(73, 19);
            this.materialLabel1.TabIndex = 5;
            this.materialLabel1.Text = "资源名称";
            // 
            // txbCoTitle
            // 
            this.txbCoTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbCoTitle.Location = new System.Drawing.Point(542, 71);
            this.txbCoTitle.Name = "txbCoTitle";
            this.txbCoTitle.Size = new System.Drawing.Size(436, 29);
            this.txbCoTitle.TabIndex = 4;
            // 
            // btnSaveContent
            // 
            this.btnSaveContent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSaveContent.Depth = 0;
            this.btnSaveContent.Icon = null;
            this.btnSaveContent.Location = new System.Drawing.Point(467, 4);
            this.btnSaveContent.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSaveContent.Name = "btnSaveContent";
            this.btnSaveContent.Primary = true;
            this.btnSaveContent.Size = new System.Drawing.Size(94, 45);
            this.btnSaveContent.TabIndex = 3;
            this.btnSaveContent.Text = "保存修改";
            this.btnSaveContent.UseVisualStyleBackColor = true;
            this.btnSaveContent.Click += new System.EventHandler(this.btnSaveContent_Click);
            // 
            // txbContent
            // 
            this.txbContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txbContent.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbContent.Location = new System.Drawing.Point(467, 106);
            this.txbContent.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.txbContent.Multiline = true;
            this.txbContent.Name = "txbContent";
            this.txbContent.Size = new System.Drawing.Size(511, 437);
            this.txbContent.TabIndex = 2;
            // 
            // lstContent
            // 
            this.lstContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstContent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.lstContent.Depth = 0;
            this.lstContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.lstContent.FullRowSelect = true;
            this.lstContent.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstContent.Location = new System.Drawing.Point(0, 0);
            this.lstContent.MouseLocation = new System.Drawing.Point(-1, -1);
            this.lstContent.MouseState = MaterialSkin.MouseState.OUT;
            this.lstContent.MultiSelect = false;
            this.lstContent.Name = "lstContent";
            this.lstContent.OwnerDraw = true;
            this.lstContent.Size = new System.Drawing.Size(461, 545);
            this.lstContent.TabIndex = 1;
            this.lstContent.UseCompatibleStateImageBehavior = false;
            this.lstContent.View = System.Windows.Forms.View.Details;
            this.lstContent.SelectedIndexChanged += new System.EventHandler(this.lstContent_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "素材内容";
            this.columnHeader3.Width = 382;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.White;
            this.tabPage4.Controls.Add(this.lstSetting);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1268, 545);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "系统设置";
            // 
            // lstSetting
            // 
            this.lstSetting.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstSetting.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lstSetting.Depth = 0;
            this.lstSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.lstSetting.FullRowSelect = true;
            this.lstSetting.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstSetting.Location = new System.Drawing.Point(0, 0);
            this.lstSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.lstSetting.MouseState = MaterialSkin.MouseState.OUT;
            this.lstSetting.Name = "lstSetting";
            this.lstSetting.OwnerDraw = true;
            this.lstSetting.Size = new System.Drawing.Size(1268, 545);
            this.lstSetting.TabIndex = 0;
            this.lstSetting.UseCompatibleStateImageBehavior = false;
            this.lstSetting.View = System.Windows.Forms.View.Details;
            this.lstSetting.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstSetting_MouseDoubleClick);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "设置名称";
            this.columnHeader4.Width = 724;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "设置类型";
            this.columnHeader5.Width = 159;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "设置值";
            this.columnHeader6.Width = 383;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btnClearLog);
            this.tabPage5.Controls.Add(this.txbLog);
            this.tabPage5.Controls.Add(this.radioRemote);
            this.tabPage5.Controls.Add(this.radioError);
            this.tabPage5.Controls.Add(this.radioWarning);
            this.tabPage5.Controls.Add(this.radioAll);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1268, 545);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "日志管理";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btnClearLog
            // 
            this.btnClearLog.AutoSize = true;
            this.btnClearLog.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClearLog.Depth = 0;
            this.btnClearLog.Icon = null;
            this.btnClearLog.Location = new System.Drawing.Point(427, 11);
            this.btnClearLog.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnClearLog.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Primary = false;
            this.btnClearLog.Size = new System.Drawing.Size(81, 36);
            this.btnClearLog.TabIndex = 5;
            this.btnClearLog.Text = "清除日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.materialFlatButton1_Click);
            // 
            // txbLog
            // 
            this.txbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbLog.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbLog.Location = new System.Drawing.Point(1, 57);
            this.txbLog.Margin = new System.Windows.Forms.Padding(0, 55, 0, 0);
            this.txbLog.Name = "txbLog";
            this.txbLog.ReadOnly = true;
            this.txbLog.Size = new System.Drawing.Size(1262, 488);
            this.txbLog.TabIndex = 4;
            this.txbLog.Text = "";
            // 
            // radioRemote
            // 
            this.radioRemote.AutoSize = true;
            this.radioRemote.Depth = 0;
            this.radioRemote.Font = new System.Drawing.Font("Roboto", 10F);
            this.radioRemote.Location = new System.Drawing.Point(324, 13);
            this.radioRemote.Margin = new System.Windows.Forms.Padding(0);
            this.radioRemote.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radioRemote.MouseState = MaterialSkin.MouseState.HOVER;
            this.radioRemote.Name = "radioRemote";
            this.radioRemote.Ripple = true;
            this.radioRemote.Size = new System.Drawing.Size(89, 30);
            this.radioRemote.TabIndex = 3;
            this.radioRemote.Text = "远程日志";
            this.radioRemote.UseVisualStyleBackColor = true;
            this.radioRemote.CheckedChanged += new System.EventHandler(this.radioRemote_CheckedChanged);
            // 
            // radioError
            // 
            this.radioError.AutoSize = true;
            this.radioError.Depth = 0;
            this.radioError.Font = new System.Drawing.Font("Roboto", 10F);
            this.radioError.Location = new System.Drawing.Point(219, 13);
            this.radioError.Margin = new System.Windows.Forms.Padding(0);
            this.radioError.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radioError.MouseState = MaterialSkin.MouseState.HOVER;
            this.radioError.Name = "radioError";
            this.radioError.Ripple = true;
            this.radioError.Size = new System.Drawing.Size(89, 30);
            this.radioError.TabIndex = 2;
            this.radioError.Text = "错误日志";
            this.radioError.UseVisualStyleBackColor = true;
            this.radioError.CheckedChanged += new System.EventHandler(this.radioError_CheckedChanged);
            // 
            // radioWarning
            // 
            this.radioWarning.AutoSize = true;
            this.radioWarning.Depth = 0;
            this.radioWarning.Font = new System.Drawing.Font("Roboto", 10F);
            this.radioWarning.Location = new System.Drawing.Point(114, 13);
            this.radioWarning.Margin = new System.Windows.Forms.Padding(0);
            this.radioWarning.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radioWarning.MouseState = MaterialSkin.MouseState.HOVER;
            this.radioWarning.Name = "radioWarning";
            this.radioWarning.Ripple = true;
            this.radioWarning.Size = new System.Drawing.Size(89, 30);
            this.radioWarning.TabIndex = 1;
            this.radioWarning.Text = "警告日志";
            this.radioWarning.UseVisualStyleBackColor = true;
            this.radioWarning.CheckedChanged += new System.EventHandler(this.radioWarning_CheckedChanged);
            // 
            // radioAll
            // 
            this.radioAll.AutoSize = true;
            this.radioAll.Checked = true;
            this.radioAll.Depth = 0;
            this.radioAll.Font = new System.Drawing.Font("Roboto", 10F);
            this.radioAll.Location = new System.Drawing.Point(12, 13);
            this.radioAll.Margin = new System.Windows.Forms.Padding(0);
            this.radioAll.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radioAll.MouseState = MaterialSkin.MouseState.HOVER;
            this.radioAll.Name = "radioAll";
            this.radioAll.Ripple = true;
            this.radioAll.Size = new System.Drawing.Size(89, 30);
            this.radioAll.TabIndex = 0;
            this.radioAll.TabStop = true;
            this.radioAll.Text = "全部日志";
            this.radioAll.UseVisualStyleBackColor = true;
            this.radioAll.CheckedChanged += new System.EventHandler(this.radioAll_CheckedChanged);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.panel1);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(1268, 545);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "虚拟定位";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btLoc);
            this.panel1.Controls.Add(this.materialLabel2);
            this.panel1.Controls.Add(this.txbLocation);
            this.panel1.Controls.Add(this.webBrowser1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1268, 545);
            this.panel1.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(0, 38);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(1265, 502);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted_1);
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabSelector1.BaseTabControl = this.tabControl1;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Icons = null;
            this.materialTabSelector1.Location = new System.Drawing.Point(0, 40);
            this.materialTabSelector1.Margin = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(1280, 105);
            this.materialTabSelector1.TabIndex = 18;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // albumMenu
            // 
            this.albumMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.全部清除ToolStripMenuItem});
            this.albumMenu.Name = "albumMenu";
            this.albumMenu.Size = new System.Drawing.Size(125, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem1.Text = "删除";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // 全部清除ToolStripMenuItem
            // 
            this.全部清除ToolStripMenuItem.Name = "全部清除ToolStripMenuItem";
            this.全部清除ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.全部清除ToolStripMenuItem.Text = "全部清除";
            this.全部清除ToolStripMenuItem.Click += new System.EventHandler(this.全部清除ToolStripMenuItem_Click);
            // 
            // txbLocation
            // 
            this.txbLocation.Depth = 0;
            this.txbLocation.Hint = "";
            this.txbLocation.Location = new System.Drawing.Point(92, 6);
            this.txbLocation.MaxLength = 32767;
            this.txbLocation.MouseState = MaterialSkin.MouseState.HOVER;
            this.txbLocation.Name = "txbLocation";
            this.txbLocation.PasswordChar = '\0';
            this.txbLocation.SelectedText = "";
            this.txbLocation.SelectionLength = 0;
            this.txbLocation.SelectionStart = 0;
            this.txbLocation.Size = new System.Drawing.Size(300, 23);
            this.txbLocation.TabIndex = 1;
            this.txbLocation.TabStop = false;
            this.txbLocation.Text = "116.403406,39.914935";
            this.txbLocation.UseSystemPasswordChar = false;
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(4, 7);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(89, 19);
            this.materialLabel2.TabIndex = 2;
            this.materialLabel2.Text = "定位坐标：";
            // 
            // btLoc
            // 
            this.btLoc.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btLoc.Depth = 0;
            this.btLoc.Icon = null;
            this.btLoc.Location = new System.Drawing.Point(408, 6);
            this.btLoc.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btLoc.MouseState = MaterialSkin.MouseState.HOVER;
            this.btLoc.Name = "btLoc";
            this.btLoc.Primary = false;
            this.btLoc.Size = new System.Drawing.Size(86, 23);
            this.btLoc.TabIndex = 3;
            this.btLoc.Text = "定位";
            this.btLoc.UseVisualStyleBackColor = true;
            this.btLoc.Visible = false;
            this.btLoc.Click += new System.EventHandler(this.btLoc_Click);
            // 
            // btnLocation
            // 
            this.btnLocation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLocation.Depth = 0;
            this.btnLocation.Icon = null;
            this.btnLocation.Location = new System.Drawing.Point(521, 49);
            this.btnLocation.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Primary = true;
            this.btnLocation.Size = new System.Drawing.Size(90, 39);
            this.btnLocation.TabIndex = 7;
            this.btnLocation.Text = "模拟定位";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.materialTabSelector1);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "MainForm";
            this.ShowActionBar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "幻影群控";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.albumMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel layoutPanel;
        private MaterialSkin.Controls.MaterialTabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private MaterialSkin.Controls.MaterialListView lstDevices;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private MaterialSkin.Controls.MaterialListView lstScripts;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private MaterialSkin.Controls.MaterialRaisedButton btnRefresh;
        private MaterialSkin.Controls.MaterialRaisedButton btnExecute;
        private MaterialSkin.Controls.MaterialRaisedButton btnSync;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private MaterialSkin.Controls.MaterialCheckBox chkDev;
        private MaterialSkin.Controls.MaterialCheckBox chkScript;
        private System.Windows.Forms.TabPage tabPage3;
        private MaterialSkin.Controls.MaterialListView lstContent;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private MaterialSkin.Controls.MaterialRaisedButton btnSaveContent;
        private System.Windows.Forms.TextBox txbContent;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private System.Windows.Forms.TextBox txbCoTitle;
        private MaterialSkin.Controls.MaterialRaisedButton btnAddContent;
        private MaterialSkin.Controls.MaterialRaisedButton btnCoDel;
        private System.Windows.Forms.TabPage tabPage4;
        private MaterialSkin.Controls.MaterialListView lstSetting;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private MaterialSkin.Controls.MaterialRaisedButton btnOpenImage;
        private System.Windows.Forms.TabPage tabPage5;
        private MaterialSkin.Controls.MaterialRadioButton radioRemote;
        private MaterialSkin.Controls.MaterialRadioButton radioError;
        private MaterialSkin.Controls.MaterialRadioButton radioWarning;
        private MaterialSkin.Controls.MaterialRadioButton radioAll;
        private System.Windows.Forms.RichTextBox txbLog;
        private System.Windows.Forms.FlowLayoutPanel contentAlbum;
        private MaterialSkin.Controls.MaterialFlatButton btnClearLog;
        private MaterialSkin.Controls.MaterialRaisedButton btnSyncContent;
        private System.Windows.Forms.ContextMenuStrip albumMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 全部清除ToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private MaterialSkin.Controls.MaterialFlatButton btLoc;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialSingleLineTextField txbLocation;
        private MaterialSkin.Controls.MaterialRaisedButton btnLocation;
    }
}

