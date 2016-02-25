namespace WordpressScraper
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openSettingFile = new System.Windows.Forms.OpenFileDialog();
            this.saveSettings = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pnlBlog = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtBlogUrl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnlMysql = new System.Windows.Forms.Panel();
            this.btnTestMySqlConnection = new System.Windows.Forms.Button();
            this.txtMySqlIp = new System.Windows.Forms.TextBox();
            this.txtMySqlDatabase = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMySqlPass = new System.Windows.Forms.TextBox();
            this.txtMysqlUser = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pnlFtp = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.btnTestFtpConnection = new System.Windows.Forms.Button();
            this.txtFtpPassword = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFtpUserName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtFtpUrl = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.pnlYoutube = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtYoutubeClientSecret = new System.Windows.Forms.TextBox();
            this.txtYoutubeClientId = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.pnlProxy = new System.Windows.Forms.Panel();
            this.numProxyPort = new System.Windows.Forms.NumericUpDown();
            this.txtProxyIp = new System.Windows.Forms.TextBox();
            this.chkUseProxy = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnDuplicate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lvSites = new System.Windows.Forms.ListView();
            this.Sallama = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.pnlBlog.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.pnlMysql.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.pnlFtp.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.pnlYoutube.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.pnlProxy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProxyPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(753, 669);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 58;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(847, 669);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 57;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // openSettingFile
            // 
            this.openSettingFile.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(934, 24);
            this.menuStrip1.TabIndex = 64;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSettingsToolStripMenuItem,
            this.loadSettingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // saveSettingsToolStripMenuItem
            // 
            this.saveSettingsToolStripMenuItem.Name = "saveSettingsToolStripMenuItem";
            this.saveSettingsToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.saveSettingsToolStripMenuItem.Text = "Export Settings...";
            this.saveSettingsToolStripMenuItem.Click += new System.EventHandler(this.saveSettingsToolStripMenuItem_Click);
            // 
            // loadSettingsToolStripMenuItem
            // 
            this.loadSettingsToolStripMenuItem.Name = "loadSettingsToolStripMenuItem";
            this.loadSettingsToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.loadSettingsToolStripMenuItem.Text = "Import Settings...";
            this.loadSettingsToolStripMenuItem.Click += new System.EventHandler(this.loadSettingsToolStripMenuItem_Click);
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabPage1);
            this.tabMain.Controls.Add(this.tabPage2);
            this.tabMain.Controls.Add(this.tabPage3);
            this.tabMain.Controls.Add(this.tabPage4);
            this.tabMain.Controls.Add(this.tabPage5);
            this.tabMain.Location = new System.Drawing.Point(277, 37);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(645, 607);
            this.tabMain.TabIndex = 65;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pnlBlog);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(637, 581);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Blog";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pnlBlog
            // 
            this.pnlBlog.Controls.Add(this.txtPassword);
            this.pnlBlog.Controls.Add(this.txtBlogUrl);
            this.pnlBlog.Controls.Add(this.label5);
            this.pnlBlog.Controls.Add(this.label3);
            this.pnlBlog.Controls.Add(this.txtUserName);
            this.pnlBlog.Controls.Add(this.label4);
            this.pnlBlog.Location = new System.Drawing.Point(6, 13);
            this.pnlBlog.Name = "pnlBlog";
            this.pnlBlog.Size = new System.Drawing.Size(286, 136);
            this.pnlBlog.TabIndex = 65;
            this.pnlBlog.Tag = "";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(87, 87);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(182, 20);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.Text = "Kazmanot111+";
            // 
            // txtBlogUrl
            // 
            this.txtBlogUrl.Location = new System.Drawing.Point(87, 25);
            this.txtBlogUrl.Name = "txtBlogUrl";
            this.txtBlogUrl.Size = new System.Drawing.Size(182, 20);
            this.txtBlogUrl.TabIndex = 2;
            this.txtBlogUrl.Text = "http://blog.guessornot.com/";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Url";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(87, 57);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(182, 20);
            this.txtUserName.TabIndex = 4;
            this.txtUserName.Text = "yuksel";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "UserName";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pnlMysql);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(637, 581);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "MySQL";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pnlMysql
            // 
            this.pnlMysql.Controls.Add(this.btnTestMySqlConnection);
            this.pnlMysql.Controls.Add(this.txtMySqlIp);
            this.pnlMysql.Controls.Add(this.txtMySqlDatabase);
            this.pnlMysql.Controls.Add(this.label8);
            this.pnlMysql.Controls.Add(this.lblDatabase);
            this.pnlMysql.Controls.Add(this.label7);
            this.pnlMysql.Controls.Add(this.txtMySqlPass);
            this.pnlMysql.Controls.Add(this.txtMysqlUser);
            this.pnlMysql.Controls.Add(this.label6);
            this.pnlMysql.Location = new System.Drawing.Point(18, 13);
            this.pnlMysql.Name = "pnlMysql";
            this.pnlMysql.Size = new System.Drawing.Size(297, 220);
            this.pnlMysql.TabIndex = 66;
            this.pnlMysql.Tag = "";
            // 
            // btnTestMySqlConnection
            // 
            this.btnTestMySqlConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestMySqlConnection.Location = new System.Drawing.Point(128, 159);
            this.btnTestMySqlConnection.Name = "btnTestMySqlConnection";
            this.btnTestMySqlConnection.Size = new System.Drawing.Size(141, 23);
            this.btnTestMySqlConnection.TabIndex = 12;
            this.btnTestMySqlConnection.Text = "Test mysql connection";
            this.btnTestMySqlConnection.UseVisualStyleBackColor = true;
            this.btnTestMySqlConnection.Click += new System.EventHandler(this.btnTestMySqlConnection_Click);
            // 
            // txtMySqlIp
            // 
            this.txtMySqlIp.Location = new System.Drawing.Point(82, 19);
            this.txtMySqlIp.Name = "txtMySqlIp";
            this.txtMySqlIp.Size = new System.Drawing.Size(182, 20);
            this.txtMySqlIp.TabIndex = 2;
            this.txtMySqlIp.Text = "198.46.81.196";
            // 
            // txtMySqlDatabase
            // 
            this.txtMySqlDatabase.Location = new System.Drawing.Point(82, 85);
            this.txtMySqlDatabase.Name = "txtMySqlDatabase";
            this.txtMySqlDatabase.Size = new System.Drawing.Size(182, 20);
            this.txtMySqlDatabase.TabIndex = 8;
            this.txtMySqlDatabase.Text = "nalgor5_wpgonbl";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Server";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(19, 88);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(53, 13);
            this.lblDatabase.TabIndex = 7;
            this.lblDatabase.Text = "Database";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "UserName";
            // 
            // txtMySqlPass
            // 
            this.txtMySqlPass.Location = new System.Drawing.Point(82, 122);
            this.txtMySqlPass.Name = "txtMySqlPass";
            this.txtMySqlPass.PasswordChar = '*';
            this.txtMySqlPass.Size = new System.Drawing.Size(182, 20);
            this.txtMySqlPass.TabIndex = 6;
            this.txtMySqlPass.Text = "S]P-a588C3";
            // 
            // txtMysqlUser
            // 
            this.txtMysqlUser.Location = new System.Drawing.Point(82, 50);
            this.txtMysqlUser.Name = "txtMysqlUser";
            this.txtMysqlUser.Size = new System.Drawing.Size(182, 20);
            this.txtMysqlUser.TabIndex = 4;
            this.txtMysqlUser.Text = "nalgor5_wpgonbl";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Password";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pnlFtp);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(637, 581);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "FTP";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pnlFtp
            // 
            this.pnlFtp.Controls.Add(this.label12);
            this.pnlFtp.Controls.Add(this.btnTestFtpConnection);
            this.pnlFtp.Controls.Add(this.txtFtpPassword);
            this.pnlFtp.Controls.Add(this.label9);
            this.pnlFtp.Controls.Add(this.txtFtpUserName);
            this.pnlFtp.Controls.Add(this.label10);
            this.pnlFtp.Controls.Add(this.txtFtpUrl);
            this.pnlFtp.Controls.Add(this.label11);
            this.pnlFtp.Location = new System.Drawing.Point(12, 13);
            this.pnlFtp.Name = "pnlFtp";
            this.pnlFtp.Size = new System.Drawing.Size(291, 172);
            this.pnlFtp.TabIndex = 64;
            this.pnlFtp.Tag = "";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(29, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(232, 13);
            this.label12.TabIndex = 21;
            this.label12.Text = "FTP account should be pointed to root directory";
            // 
            // btnTestFtpConnection
            // 
            this.btnTestFtpConnection.Location = new System.Drawing.Point(130, 135);
            this.btnTestFtpConnection.Name = "btnTestFtpConnection";
            this.btnTestFtpConnection.Size = new System.Drawing.Size(141, 23);
            this.btnTestFtpConnection.TabIndex = 20;
            this.btnTestFtpConnection.Text = "Test FTP connection";
            this.btnTestFtpConnection.UseVisualStyleBackColor = true;
            this.btnTestFtpConnection.Click += new System.EventHandler(this.btnTestFtpConnection_Click);
            // 
            // txtFtpPassword
            // 
            this.txtFtpPassword.Location = new System.Drawing.Point(89, 95);
            this.txtFtpPassword.Name = "txtFtpPassword";
            this.txtFtpPassword.PasswordChar = '*';
            this.txtFtpPassword.Size = new System.Drawing.Size(182, 20);
            this.txtFtpPassword.TabIndex = 19;
            this.txtFtpPassword.Text = "x6b8a[o!bxFe";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(29, 101);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Password";
            // 
            // txtFtpUserName
            // 
            this.txtFtpUserName.Location = new System.Drawing.Point(89, 65);
            this.txtFtpUserName.Name = "txtFtpUserName";
            this.txtFtpUserName.Size = new System.Drawing.Size(182, 20);
            this.txtFtpUserName.TabIndex = 17;
            this.txtFtpUserName.Text = "bloggon2@nalgorithm.com";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(29, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "UserName";
            // 
            // txtFtpUrl
            // 
            this.txtFtpUrl.Location = new System.Drawing.Point(89, 33);
            this.txtFtpUrl.Name = "txtFtpUrl";
            this.txtFtpUrl.Size = new System.Drawing.Size(182, 20);
            this.txtFtpUrl.TabIndex = 15;
            this.txtFtpUrl.Text = "ftp.nalgorithm.com";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(26, 36);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Url";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.pnlYoutube);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(637, 581);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Youtube";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // pnlYoutube
            // 
            this.pnlYoutube.Controls.Add(this.label16);
            this.pnlYoutube.Controls.Add(this.label15);
            this.pnlYoutube.Controls.Add(this.txtYoutubeClientSecret);
            this.pnlYoutube.Controls.Add(this.txtYoutubeClientId);
            this.pnlYoutube.Controls.Add(this.label13);
            this.pnlYoutube.Controls.Add(this.label14);
            this.pnlYoutube.Location = new System.Drawing.Point(7, 13);
            this.pnlYoutube.Name = "pnlYoutube";
            this.pnlYoutube.Size = new System.Drawing.Size(627, 276);
            this.pnlYoutube.TabIndex = 67;
            this.pnlYoutube.Tag = "";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(19, 101);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(153, 13);
            this.label16.TabIndex = 8;
            this.label16.Text = "How to get these codes? ";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(19, 125);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(458, 113);
            this.label15.TabIndex = 7;
            this.label15.Text = resources.GetString("label15.Text");
            // 
            // txtYoutubeClientSecret
            // 
            this.txtYoutubeClientSecret.Location = new System.Drawing.Point(92, 62);
            this.txtYoutubeClientSecret.Name = "txtYoutubeClientSecret";
            this.txtYoutubeClientSecret.Size = new System.Drawing.Size(182, 20);
            this.txtYoutubeClientSecret.TabIndex = 6;
            // 
            // txtYoutubeClientId
            // 
            this.txtYoutubeClientId.Location = new System.Drawing.Point(92, 25);
            this.txtYoutubeClientId.Name = "txtYoutubeClientId";
            this.txtYoutubeClientId.Size = new System.Drawing.Size(522, 20);
            this.txtYoutubeClientId.TabIndex = 2;
            this.txtYoutubeClientId.Text = "977332511117-fem10do6477tnclko771q553u31kt0s3";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(19, 65);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "Client Secret";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(19, 25);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 13);
            this.label14.TabIndex = 1;
            this.label14.Text = "Client ID";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.pnlProxy);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(637, 581);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Proxy";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // pnlProxy
            // 
            this.pnlProxy.Controls.Add(this.numProxyPort);
            this.pnlProxy.Controls.Add(this.txtProxyIp);
            this.pnlProxy.Controls.Add(this.chkUseProxy);
            this.pnlProxy.Controls.Add(this.label2);
            this.pnlProxy.Controls.Add(this.label1);
            this.pnlProxy.Location = new System.Drawing.Point(13, 17);
            this.pnlProxy.Name = "pnlProxy";
            this.pnlProxy.Size = new System.Drawing.Size(291, 125);
            this.pnlProxy.TabIndex = 63;
            this.pnlProxy.Tag = "";
            // 
            // numProxyPort
            // 
            this.numProxyPort.Location = new System.Drawing.Point(93, 85);
            this.numProxyPort.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numProxyPort.Name = "numProxyPort";
            this.numProxyPort.Size = new System.Drawing.Size(64, 20);
            this.numProxyPort.TabIndex = 10;
            this.numProxyPort.Value = new decimal(new int[] {
            21324,
            0,
            0,
            0});
            // 
            // txtProxyIp
            // 
            this.txtProxyIp.Location = new System.Drawing.Point(93, 53);
            this.txtProxyIp.Name = "txtProxyIp";
            this.txtProxyIp.Size = new System.Drawing.Size(117, 20);
            this.txtProxyIp.TabIndex = 6;
            this.txtProxyIp.Text = "192.3.252.232";
            // 
            // chkUseProxy
            // 
            this.chkUseProxy.AutoSize = true;
            this.chkUseProxy.Location = new System.Drawing.Point(33, 27);
            this.chkUseProxy.Name = "chkUseProxy";
            this.chkUseProxy.Size = new System.Drawing.Size(74, 17);
            this.chkUseProxy.TabIndex = 9;
            this.chkUseProxy.Text = "Use Proxy";
            this.chkUseProxy.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "IP";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Port";
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNew.Location = new System.Drawing.Point(27, 592);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(103, 23);
            this.btnNew.TabIndex = 67;
            this.btnNew.Text = "New Site";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnRename
            // 
            this.btnRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRename.Location = new System.Drawing.Point(27, 621);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(103, 23);
            this.btnRename.TabIndex = 68;
            this.btnRename.Text = "Rename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnDuplicate
            // 
            this.btnDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDuplicate.Location = new System.Drawing.Point(145, 592);
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.Size = new System.Drawing.Size(94, 23);
            this.btnDuplicate.TabIndex = 69;
            this.btnDuplicate.Text = "Duplicate";
            this.btnDuplicate.UseVisualStyleBackColor = true;
            this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(145, 621);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(94, 23);
            this.btnDelete.TabIndex = 70;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lvSites
            // 
            this.lvSites.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvSites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Sallama});
            this.lvSites.FullRowSelect = true;
            this.lvSites.HideSelection = false;
            this.lvSites.LabelEdit = true;
            this.lvSites.Location = new System.Drawing.Point(27, 37);
            this.lvSites.MultiSelect = false;
            this.lvSites.Name = "lvSites";
            this.lvSites.Size = new System.Drawing.Size(231, 537);
            this.lvSites.TabIndex = 71;
            this.lvSites.UseCompatibleStateImageBehavior = false;
            this.lvSites.View = System.Windows.Forms.View.Details;
            this.lvSites.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvSites_AfterLabelEdit);
            this.lvSites.SelectedIndexChanged += new System.EventHandler(this.lvSites_SelectedIndexChanged);
            // 
            // Sallama
            // 
            this.Sallama.Text = "Sites";
            this.Sallama.Width = 215;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(934, 704);
            this.Controls.Add(this.lvSites);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnDuplicate);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 320);
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.pnlBlog.ResumeLayout(false);
            this.pnlBlog.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.pnlMysql.ResumeLayout(false);
            this.pnlMysql.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.pnlFtp.ResumeLayout(false);
            this.pnlFtp.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.pnlYoutube.ResumeLayout(false);
            this.pnlYoutube.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.pnlProxy.ResumeLayout(false);
            this.pnlProxy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProxyPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog openSettingFile;
        private System.Windows.Forms.SaveFileDialog saveSettings;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSettingsToolStripMenuItem;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel pnlBlog;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtBlogUrl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel pnlMysql;
        private System.Windows.Forms.Button btnTestMySqlConnection;
        private System.Windows.Forms.TextBox txtMySqlIp;
        private System.Windows.Forms.TextBox txtMySqlDatabase;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMySqlPass;
        private System.Windows.Forms.TextBox txtMysqlUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel pnlFtp;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnTestFtpConnection;
        private System.Windows.Forms.TextBox txtFtpPassword;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFtpUserName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtFtpUrl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Panel pnlYoutube;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtYoutubeClientSecret;
        private System.Windows.Forms.TextBox txtYoutubeClientId;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Panel pnlProxy;
        private System.Windows.Forms.NumericUpDown numProxyPort;
        private System.Windows.Forms.TextBox txtProxyIp;
        private System.Windows.Forms.CheckBox chkUseProxy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnDuplicate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ListView lvSites;
        private System.Windows.Forms.ColumnHeader Sallama;
    }
}