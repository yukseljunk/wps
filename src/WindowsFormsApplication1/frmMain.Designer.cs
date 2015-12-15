namespace WindowsFormsApplication1
{
    partial class frmMain
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
            System.Windows.Forms.ColumnHeader No;
            System.Windows.Forms.ColumnHeader Id;
            System.Windows.Forms.ColumnHeader Url;
            System.Windows.Forms.ColumnHeader Title;
            System.Windows.Forms.ColumnHeader MetaDescription;
            System.Windows.Forms.ColumnHeader content;
            System.Windows.Forms.ColumnHeader price;
            System.Windows.Forms.ColumnHeader Image;
            System.Windows.Forms.ColumnHeader tags;
            System.Windows.Forms.ColumnHeader postId;
            System.Windows.Forms.ColumnHeader site;
            System.Windows.Forms.ColumnHeader wordcount;
            this.btnStart = new System.Windows.Forms.Button();
            this.lvItems = new System.Windows.Forms.ListView();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.grpBlogProp = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBlogUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStopScrape = new System.Windows.Forms.Button();
            this.btnGetPost = new System.Windows.Forms.Button();
            this.txtPostId = new System.Windows.Forms.TextBox();
            this.chkFeatureImage = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.barStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.lblDateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.chkCache = new System.Windows.Forms.CheckBox();
            this.grpMysql = new System.Windows.Forms.GroupBox();
            this.btnTestMySqlConnection = new System.Windows.Forms.Button();
            this.txtMySqlDatabase = new System.Windows.Forms.TextBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.txtMySqlPass = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMysqlUser = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMySqlIp = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.grpAuthors = new System.Windows.Forms.GroupBox();
            this.txtAuthors = new System.Windows.Forms.TextBox();
            this.btnSetTitle = new System.Windows.Forms.Button();
            this.chkNoAPI = new System.Windows.Forms.CheckBox();
            this.grpFtp = new System.Windows.Forms.GroupBox();
            this.btnTestFtpConnection = new System.Windows.Forms.Button();
            this.txtFtpPassword = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFtpUserName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtFtpUrl = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkResizeImages = new System.Windows.Forms.CheckBox();
            this.lblResizePix = new System.Windows.Forms.Label();
            this.numMaxImageDimension = new System.Windows.Forms.NumericUpDown();
            this.grpTop = new System.Windows.Forms.GroupBox();
            this.chkSites = new System.Windows.Forms.CheckedListBox();
            this.chkAllPages = new System.Windows.Forms.CheckBox();
            this.lblPageTo = new System.Windows.Forms.Label();
            this.numPageTo = new System.Windows.Forms.NumericUpDown();
            this.numPage = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnScrumble = new System.Windows.Forms.Button();
            this.txtFindDuplicatePosts = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnRemoveDuplicates = new System.Windows.Forms.Button();
            this.btnNavigate = new System.Windows.Forms.Button();
            this.numThumbnailSize = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnGoAsync = new System.Windows.Forms.Button();
            No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            Url = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            MetaDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            content = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            price = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            Image = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            tags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            postId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            site = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            wordcount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpBlogProp.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.grpMysql.SuspendLayout();
            this.grpAuthors.SuspendLayout();
            this.grpFtp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxImageDimension)).BeginInit();
            this.grpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThumbnailSize)).BeginInit();
            this.SuspendLayout();
            // 
            // No
            // 
            No.Text = "#";
            // 
            // Id
            // 
            Id.Text = "Id";
            Id.Width = 75;
            // 
            // Url
            // 
            Url.Text = "Url";
            Url.Width = 104;
            // 
            // Title
            // 
            Title.Text = "Title";
            Title.Width = 199;
            // 
            // MetaDescription
            // 
            MetaDescription.Text = "Meta Description";
            MetaDescription.Width = 242;
            // 
            // content
            // 
            content.Text = "Content";
            // 
            // price
            // 
            price.Text = "price";
            // 
            // Image
            // 
            Image.Text = "Image";
            Image.Width = 117;
            // 
            // tags
            // 
            tags.Text = "Tags";
            // 
            // postId
            // 
            postId.Text = "Post Id";
            // 
            // site
            // 
            site.Text = "Site";
            // 
            // wordcount
            // 
            wordcount.Text = "Word Count";
            wordcount.Width = 79;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(1196, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(83, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Go!";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lvItems
            // 
            this.lvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            No,
            Id,
            Url,
            Title,
            MetaDescription,
            content,
            price,
            Image,
            tags,
            site,
            wordcount,
            postId});
            this.lvItems.FullRowSelect = true;
            this.lvItems.GridLines = true;
            this.lvItems.HideSelection = false;
            this.lvItems.Location = new System.Drawing.Point(14, 72);
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(1348, 420);
            this.lvItems.TabIndex = 5;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvItems_ColumnClick);
            this.lvItems.SelectedIndexChanged += new System.EventHandler(this.lvItems_SelectedIndexChanged);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.Location = new System.Drawing.Point(103, 498);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(85, 23);
            this.btnSelectAll.TabIndex = 6;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // grpBlogProp
            // 
            this.grpBlogProp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpBlogProp.Controls.Add(this.txtPassword);
            this.grpBlogProp.Controls.Add(this.label5);
            this.grpBlogProp.Controls.Add(this.txtUserName);
            this.grpBlogProp.Controls.Add(this.label4);
            this.grpBlogProp.Controls.Add(this.txtBlogUrl);
            this.grpBlogProp.Controls.Add(this.label3);
            this.grpBlogProp.Location = new System.Drawing.Point(15, 537);
            this.grpBlogProp.Name = "grpBlogProp";
            this.grpBlogProp.Size = new System.Drawing.Size(268, 124);
            this.grpBlogProp.TabIndex = 7;
            this.grpBlogProp.TabStop = false;
            this.grpBlogProp.Text = "Target Blog";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(75, 85);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(182, 20);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.Text = "Kazmanot111+";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Password";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(75, 55);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(182, 20);
            this.txtUserName.TabIndex = 4;
            this.txtUserName.Text = "yuksel";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "UserName";
            // 
            // txtBlogUrl
            // 
            this.txtBlogUrl.Location = new System.Drawing.Point(75, 23);
            this.txtBlogUrl.Name = "txtBlogUrl";
            this.txtBlogUrl.Size = new System.Drawing.Size(182, 20);
            this.txtBlogUrl.TabIndex = 2;
            this.txtBlogUrl.Text = "http://blog.guessornot.com/";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Url";
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(1000, 680);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(190, 23);
            this.btnGo.TabIndex = 8;
            this.btnGo.Text = "Create Selected Items on the Blog";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(1211, 679);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(152, 23);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStopScrape
            // 
            this.btnStopScrape.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopScrape.Location = new System.Drawing.Point(1287, 13);
            this.btnStopScrape.Name = "btnStopScrape";
            this.btnStopScrape.Size = new System.Drawing.Size(75, 23);
            this.btnStopScrape.TabIndex = 10;
            this.btnStopScrape.Text = "Stop";
            this.btnStopScrape.UseVisualStyleBackColor = true;
            this.btnStopScrape.Click += new System.EventHandler(this.btnStopScrape_Click);
            // 
            // btnGetPost
            // 
            this.btnGetPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetPost.Location = new System.Drawing.Point(880, 501);
            this.btnGetPost.Name = "btnGetPost";
            this.btnGetPost.Size = new System.Drawing.Size(124, 23);
            this.btnGetPost.TabIndex = 11;
            this.btnGetPost.Text = "Get Selected Post";
            this.btnGetPost.UseVisualStyleBackColor = true;
            this.btnGetPost.Visible = false;
            this.btnGetPost.Click += new System.EventHandler(this.btnGetPost_Click);
            // 
            // txtPostId
            // 
            this.txtPostId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPostId.Location = new System.Drawing.Point(1010, 503);
            this.txtPostId.Name = "txtPostId";
            this.txtPostId.Size = new System.Drawing.Size(118, 20);
            this.txtPostId.TabIndex = 12;
            this.txtPostId.Visible = false;
            // 
            // chkFeatureImage
            // 
            this.chkFeatureImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFeatureImage.AutoSize = true;
            this.chkFeatureImage.Location = new System.Drawing.Point(702, 679);
            this.chkFeatureImage.Name = "chkFeatureImage";
            this.chkFeatureImage.Size = new System.Drawing.Size(192, 17);
            this.chkFeatureImage.TabIndex = 13;
            this.chkFeatureImage.Text = "Make First Image as Feature Image";
            this.chkFeatureImage.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.barStatus,
            this.lblDateTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 709);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1396, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // barStatus
            // 
            this.barStatus.Margin = new System.Windows.Forms.Padding(0, 3, 1, 3);
            this.barStatus.Name = "barStatus";
            this.barStatus.Size = new System.Drawing.Size(300, 16);
            this.barStatus.Step = 1;
            this.barStatus.Visible = false;
            // 
            // lblDateTime
            // 
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(118, 17);
            this.lblDateTime.Text = "toolStripStatusLabel1";
            // 
            // chkCache
            // 
            this.chkCache.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCache.AutoSize = true;
            this.chkCache.Checked = true;
            this.chkCache.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCache.Location = new System.Drawing.Point(1276, 504);
            this.chkCache.Name = "chkCache";
            this.chkCache.Size = new System.Drawing.Size(87, 17);
            this.chkCache.TabIndex = 19;
            this.chkCache.Text = "Use Caching";
            this.chkCache.UseVisualStyleBackColor = true;
            this.chkCache.Visible = false;
            // 
            // grpMysql
            // 
            this.grpMysql.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpMysql.Controls.Add(this.btnTestMySqlConnection);
            this.grpMysql.Controls.Add(this.txtMySqlDatabase);
            this.grpMysql.Controls.Add(this.lblDatabase);
            this.grpMysql.Controls.Add(this.txtMySqlPass);
            this.grpMysql.Controls.Add(this.label6);
            this.grpMysql.Controls.Add(this.txtMysqlUser);
            this.grpMysql.Controls.Add(this.label7);
            this.grpMysql.Controls.Add(this.txtMySqlIp);
            this.grpMysql.Controls.Add(this.label8);
            this.grpMysql.Location = new System.Drawing.Point(313, 537);
            this.grpMysql.Name = "grpMysql";
            this.grpMysql.Size = new System.Drawing.Size(450, 124);
            this.grpMysql.TabIndex = 20;
            this.grpMysql.TabStop = false;
            this.grpMysql.Text = "Target Blog MySql Settings";
            // 
            // btnTestMySqlConnection
            // 
            this.btnTestMySqlConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestMySqlConnection.Location = new System.Drawing.Point(292, 84);
            this.btnTestMySqlConnection.Name = "btnTestMySqlConnection";
            this.btnTestMySqlConnection.Size = new System.Drawing.Size(141, 23);
            this.btnTestMySqlConnection.TabIndex = 12;
            this.btnTestMySqlConnection.Text = "Test mysql connection";
            this.btnTestMySqlConnection.UseVisualStyleBackColor = true;
            this.btnTestMySqlConnection.Click += new System.EventHandler(this.btnTestMySqlConnection_Click);
            // 
            // txtMySqlDatabase
            // 
            this.txtMySqlDatabase.Location = new System.Drawing.Point(303, 20);
            this.txtMySqlDatabase.Name = "txtMySqlDatabase";
            this.txtMySqlDatabase.Size = new System.Drawing.Size(130, 20);
            this.txtMySqlDatabase.TabIndex = 8;
            this.txtMySqlDatabase.Text = "nalgor5_wpgonbl";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(232, 23);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(53, 13);
            this.lblDatabase.TabIndex = 7;
            this.lblDatabase.Text = "Database";
            // 
            // txtMySqlPass
            // 
            this.txtMySqlPass.Location = new System.Drawing.Point(303, 55);
            this.txtMySqlPass.Name = "txtMySqlPass";
            this.txtMySqlPass.PasswordChar = '*';
            this.txtMySqlPass.Size = new System.Drawing.Size(130, 20);
            this.txtMySqlPass.TabIndex = 6;
            this.txtMySqlPass.Text = "S]P-a588C3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(232, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Password";
            // 
            // txtMysqlUser
            // 
            this.txtMysqlUser.Location = new System.Drawing.Point(75, 54);
            this.txtMysqlUser.Name = "txtMysqlUser";
            this.txtMysqlUser.Size = new System.Drawing.Size(130, 20);
            this.txtMysqlUser.TabIndex = 4;
            this.txtMysqlUser.Text = "nalgor5_wpgonbl";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "UserName";
            // 
            // txtMySqlIp
            // 
            this.txtMySqlIp.Location = new System.Drawing.Point(75, 23);
            this.txtMySqlIp.Name = "txtMySqlIp";
            this.txtMySqlIp.Size = new System.Drawing.Size(130, 20);
            this.txtMySqlIp.TabIndex = 2;
            this.txtMySqlIp.Text = "198.46.81.196";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Server";
            // 
            // grpAuthors
            // 
            this.grpAuthors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAuthors.Controls.Add(this.txtAuthors);
            this.grpAuthors.Location = new System.Drawing.Point(1080, 537);
            this.grpAuthors.Name = "grpAuthors";
            this.grpAuthors.Size = new System.Drawing.Size(283, 124);
            this.grpAuthors.TabIndex = 21;
            this.grpAuthors.TabStop = false;
            this.grpAuthors.Text = "Authors";
            // 
            // txtAuthors
            // 
            this.txtAuthors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAuthors.BackColor = System.Drawing.Color.White;
            this.txtAuthors.Location = new System.Drawing.Point(20, 18);
            this.txtAuthors.Multiline = true;
            this.txtAuthors.Name = "txtAuthors";
            this.txtAuthors.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAuthors.Size = new System.Drawing.Size(257, 90);
            this.txtAuthors.TabIndex = 0;
            this.txtAuthors.Text = "John Doe\r\nJennifer X";
            // 
            // btnSetTitle
            // 
            this.btnSetTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetTitle.Location = new System.Drawing.Point(1140, 501);
            this.btnSetTitle.Name = "btnSetTitle";
            this.btnSetTitle.Size = new System.Drawing.Size(132, 23);
            this.btnSetTitle.TabIndex = 22;
            this.btnSetTitle.Text = "Set Title";
            this.btnSetTitle.UseVisualStyleBackColor = true;
            this.btnSetTitle.Visible = false;
            this.btnSetTitle.Click += new System.EventHandler(this.btnSetTitle_Click);
            // 
            // chkNoAPI
            // 
            this.chkNoAPI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkNoAPI.AutoSize = true;
            this.chkNoAPI.Checked = true;
            this.chkNoAPI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNoAPI.Location = new System.Drawing.Point(484, 679);
            this.chkNoAPI.Name = "chkNoAPI";
            this.chkNoAPI.Size = new System.Drawing.Size(181, 17);
            this.chkNoAPI.TabIndex = 23;
            this.chkNoAPI.Text = "Don\'t use Api but Mysql and FTP";
            this.chkNoAPI.UseVisualStyleBackColor = true;
            this.chkNoAPI.CheckedChanged += new System.EventHandler(this.chkNoAPI_CheckedChanged);
            // 
            // grpFtp
            // 
            this.grpFtp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpFtp.Controls.Add(this.btnTestFtpConnection);
            this.grpFtp.Controls.Add(this.txtFtpPassword);
            this.grpFtp.Controls.Add(this.label9);
            this.grpFtp.Controls.Add(this.txtFtpUserName);
            this.grpFtp.Controls.Add(this.label10);
            this.grpFtp.Controls.Add(this.txtFtpUrl);
            this.grpFtp.Controls.Add(this.label11);
            this.grpFtp.Location = new System.Drawing.Point(779, 540);
            this.grpFtp.Name = "grpFtp";
            this.grpFtp.Size = new System.Drawing.Size(285, 121);
            this.grpFtp.TabIndex = 24;
            this.grpFtp.TabStop = false;
            this.grpFtp.Text = "FTP to wp_content/uploads folder";
            // 
            // btnTestFtpConnection
            // 
            this.btnTestFtpConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestFtpConnection.Location = new System.Drawing.Point(157, 85);
            this.btnTestFtpConnection.Name = "btnTestFtpConnection";
            this.btnTestFtpConnection.Size = new System.Drawing.Size(100, 23);
            this.btnTestFtpConnection.TabIndex = 13;
            this.btnTestFtpConnection.Text = "Test connection";
            this.btnTestFtpConnection.UseVisualStyleBackColor = true;
            this.btnTestFtpConnection.Click += new System.EventHandler(this.btnTestFtpConnection_Click);
            // 
            // txtFtpPassword
            // 
            this.txtFtpPassword.Location = new System.Drawing.Point(75, 85);
            this.txtFtpPassword.Name = "txtFtpPassword";
            this.txtFtpPassword.PasswordChar = '*';
            this.txtFtpPassword.Size = new System.Drawing.Size(76, 20);
            this.txtFtpPassword.TabIndex = 6;
            this.txtFtpPassword.Text = "U4E9TrT;5!)F";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Password";
            // 
            // txtFtpUserName
            // 
            this.txtFtpUserName.Location = new System.Drawing.Point(75, 55);
            this.txtFtpUserName.Name = "txtFtpUserName";
            this.txtFtpUserName.Size = new System.Drawing.Size(182, 20);
            this.txtFtpUserName.TabIndex = 4;
            this.txtFtpUserName.Text = "bloggon@nalgorithm.com";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "UserName";
            // 
            // txtFtpUrl
            // 
            this.txtFtpUrl.Location = new System.Drawing.Point(75, 23);
            this.txtFtpUrl.Name = "txtFtpUrl";
            this.txtFtpUrl.Size = new System.Drawing.Size(182, 20);
            this.txtFtpUrl.TabIndex = 2;
            this.txtFtpUrl.Text = "ftp.nalgorithm.com";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Url";
            // 
            // chkResizeImages
            // 
            this.chkResizeImages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkResizeImages.AutoSize = true;
            this.chkResizeImages.Checked = true;
            this.chkResizeImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkResizeImages.Location = new System.Drawing.Point(203, 679);
            this.chkResizeImages.Name = "chkResizeImages";
            this.chkResizeImages.Size = new System.Drawing.Size(153, 17);
            this.chkResizeImages.TabIndex = 26;
            this.chkResizeImages.Text = "Resize images bigger than ";
            this.chkResizeImages.UseVisualStyleBackColor = true;
            // 
            // lblResizePix
            // 
            this.lblResizePix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResizePix.AutoSize = true;
            this.lblResizePix.Location = new System.Drawing.Point(425, 680);
            this.lblResizePix.Name = "lblResizePix";
            this.lblResizePix.Size = new System.Drawing.Size(18, 13);
            this.lblResizePix.TabIndex = 28;
            this.lblResizePix.Text = "px";
            // 
            // numMaxImageDimension
            // 
            this.numMaxImageDimension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numMaxImageDimension.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numMaxImageDimension.Location = new System.Drawing.Point(364, 678);
            this.numMaxImageDimension.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numMaxImageDimension.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxImageDimension.Name = "numMaxImageDimension";
            this.numMaxImageDimension.Size = new System.Drawing.Size(55, 20);
            this.numMaxImageDimension.TabIndex = 29;
            this.numMaxImageDimension.Value = new decimal(new int[] {
            750,
            0,
            0,
            0});
            // 
            // grpTop
            // 
            this.grpTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTop.Controls.Add(this.chkSites);
            this.grpTop.Controls.Add(this.chkAllPages);
            this.grpTop.Controls.Add(this.lblPageTo);
            this.grpTop.Controls.Add(this.numPageTo);
            this.grpTop.Controls.Add(this.numPage);
            this.grpTop.Controls.Add(this.label2);
            this.grpTop.Controls.Add(this.txtUrl);
            this.grpTop.Controls.Add(this.label1);
            this.grpTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpTop.Location = new System.Drawing.Point(12, 0);
            this.grpTop.Name = "grpTop";
            this.grpTop.Size = new System.Drawing.Size(1032, 66);
            this.grpTop.TabIndex = 30;
            this.grpTop.TabStop = false;
            // 
            // chkSites
            // 
            this.chkSites.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSites.CheckOnClick = true;
            this.chkSites.ColumnWidth = 90;
            this.chkSites.FormattingEnabled = true;
            this.chkSites.Items.AddRange(new object[] {
            "Etsy"});
            this.chkSites.Location = new System.Drawing.Point(424, 24);
            this.chkSites.MultiColumn = true;
            this.chkSites.Name = "chkSites";
            this.chkSites.Size = new System.Drawing.Size(300, 34);
            this.chkSites.TabIndex = 33;
            // 
            // chkAllPages
            // 
            this.chkAllPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllPages.AutoSize = true;
            this.chkAllPages.Location = new System.Drawing.Point(946, 31);
            this.chkAllPages.Name = "chkAllPages";
            this.chkAllPages.Size = new System.Drawing.Size(70, 17);
            this.chkAllPages.TabIndex = 32;
            this.chkAllPages.Text = "All Pages";
            this.chkAllPages.UseVisualStyleBackColor = true;
            this.chkAllPages.CheckedChanged += new System.EventHandler(this.chkAllPages_CheckedChanged);
            // 
            // lblPageTo
            // 
            this.lblPageTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPageTo.AutoSize = true;
            this.lblPageTo.Location = new System.Drawing.Point(845, 32);
            this.lblPageTo.Name = "lblPageTo";
            this.lblPageTo.Size = new System.Drawing.Size(20, 13);
            this.lblPageTo.TabIndex = 31;
            this.lblPageTo.Text = "To";
            // 
            // numPageTo
            // 
            this.numPageTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPageTo.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numPageTo.Location = new System.Drawing.Point(873, 28);
            this.numPageTo.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numPageTo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPageTo.Name = "numPageTo";
            this.numPageTo.Size = new System.Drawing.Size(67, 20);
            this.numPageTo.TabIndex = 30;
            this.numPageTo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPageTo.ValueChanged += new System.EventHandler(this.numPageTo_ValueChanged);
            // 
            // numPage
            // 
            this.numPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPage.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numPage.Location = new System.Drawing.Point(772, 27);
            this.numPage.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numPage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPage.Name = "numPage";
            this.numPage.Size = new System.Drawing.Size(67, 20);
            this.numPage.TabIndex = 29;
            this.numPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPage.ValueChanged += new System.EventHandler(this.numPage_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(734, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Page";
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(17, 31);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(397, 20);
            this.txtUrl.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Keyword";
            // 
            // btnScrumble
            // 
            this.btnScrumble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnScrumble.Location = new System.Drawing.Point(12, 498);
            this.btnScrumble.Name = "btnScrumble";
            this.btnScrumble.Size = new System.Drawing.Size(85, 23);
            this.btnScrumble.TabIndex = 31;
            this.btnScrumble.Text = "Scramble";
            this.btnScrumble.UseVisualStyleBackColor = true;
            this.btnScrumble.Click += new System.EventHandler(this.btnScrumble_Click);
            // 
            // txtFindDuplicatePosts
            // 
            this.txtFindDuplicatePosts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtFindDuplicatePosts.Location = new System.Drawing.Point(334, 498);
            this.txtFindDuplicatePosts.Name = "txtFindDuplicatePosts";
            this.txtFindDuplicatePosts.Size = new System.Drawing.Size(132, 23);
            this.txtFindDuplicatePosts.TabIndex = 32;
            this.txtFindDuplicatePosts.Text = "Find Duplicates";
            this.txtFindDuplicatePosts.UseVisualStyleBackColor = true;
            this.txtFindDuplicatePosts.Click += new System.EventHandler(this.txtFindDuplicatePosts_Click);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveSelected.Location = new System.Drawing.Point(194, 498);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(126, 23);
            this.btnRemoveSelected.TabIndex = 33;
            this.btnRemoveSelected.Text = "Remove Selected";
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // btnRemoveDuplicates
            // 
            this.btnRemoveDuplicates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveDuplicates.Location = new System.Drawing.Point(472, 498);
            this.btnRemoveDuplicates.Name = "btnRemoveDuplicates";
            this.btnRemoveDuplicates.Size = new System.Drawing.Size(126, 23);
            this.btnRemoveDuplicates.TabIndex = 34;
            this.btnRemoveDuplicates.Text = "Remove Duplicates";
            this.btnRemoveDuplicates.UseVisualStyleBackColor = true;
            this.btnRemoveDuplicates.Click += new System.EventHandler(this.btnRemoveDuplicates_Click);
            // 
            // btnNavigate
            // 
            this.btnNavigate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNavigate.Location = new System.Drawing.Point(605, 500);
            this.btnNavigate.Name = "btnNavigate";
            this.btnNavigate.Size = new System.Drawing.Size(126, 23);
            this.btnNavigate.TabIndex = 35;
            this.btnNavigate.Text = "Go to link...";
            this.btnNavigate.UseVisualStyleBackColor = true;
            this.btnNavigate.Click += new System.EventHandler(this.btnNavigate_Click);
            // 
            // numThumbnailSize
            // 
            this.numThumbnailSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numThumbnailSize.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numThumbnailSize.Location = new System.Drawing.Point(102, 678);
            this.numThumbnailSize.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numThumbnailSize.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numThumbnailSize.Name = "numThumbnailSize";
            this.numThumbnailSize.Size = new System.Drawing.Size(55, 20);
            this.numThumbnailSize.TabIndex = 36;
            this.numThumbnailSize.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(160, 683);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(18, 13);
            this.label12.TabIndex = 37;
            this.label12.Text = "px";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 680);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 38;
            this.label13.Text = "Thumbnail Size";
            // 
            // btnGoAsync
            // 
            this.btnGoAsync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGoAsync.Location = new System.Drawing.Point(1100, 12);
            this.btnGoAsync.Name = "btnGoAsync";
            this.btnGoAsync.Size = new System.Drawing.Size(83, 23);
            this.btnGoAsync.TabIndex = 39;
            this.btnGoAsync.Text = "Go Aysnc!";
            this.btnGoAsync.UseVisualStyleBackColor = true;
            this.btnGoAsync.Click += new System.EventHandler(this.btnGoAsync_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1396, 731);
            this.Controls.Add(this.btnGoAsync);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.numThumbnailSize);
            this.Controls.Add(this.btnNavigate);
            this.Controls.Add(this.btnRemoveDuplicates);
            this.Controls.Add(this.btnRemoveSelected);
            this.Controls.Add(this.txtFindDuplicatePosts);
            this.Controls.Add(this.btnScrumble);
            this.Controls.Add(this.grpTop);
            this.Controls.Add(this.numMaxImageDimension);
            this.Controls.Add(this.lblResizePix);
            this.Controls.Add(this.chkResizeImages);
            this.Controls.Add(this.grpFtp);
            this.Controls.Add(this.chkNoAPI);
            this.Controls.Add(this.btnSetTitle);
            this.Controls.Add(this.grpAuthors);
            this.Controls.Add(this.grpMysql);
            this.Controls.Add(this.chkCache);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.chkFeatureImage);
            this.Controls.Add(this.txtPostId);
            this.Controls.Add(this.btnGetPost);
            this.Controls.Add(this.btnStopScrape);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.grpBlogProp);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.btnStart);
            this.Name = "frmMain";
            this.Text = "Wordpress Scraper";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.grpBlogProp.ResumeLayout(false);
            this.grpBlogProp.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpMysql.ResumeLayout(false);
            this.grpMysql.PerformLayout();
            this.grpAuthors.ResumeLayout(false);
            this.grpAuthors.PerformLayout();
            this.grpFtp.ResumeLayout(false);
            this.grpFtp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxImageDimension)).EndInit();
            this.grpTop.ResumeLayout(false);
            this.grpTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThumbnailSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListView lvItems;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.GroupBox grpBlogProp;
        private System.Windows.Forms.TextBox txtBlogUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStopScrape;
        private System.Windows.Forms.Button btnGetPost;
        private System.Windows.Forms.TextBox txtPostId;
        private System.Windows.Forms.CheckBox chkFeatureImage;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar barStatus;
        private System.Windows.Forms.CheckBox chkCache;
        private System.Windows.Forms.GroupBox grpMysql;
        private System.Windows.Forms.TextBox txtMySqlPass;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMysqlUser;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMySqlIp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMySqlDatabase;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.Button btnTestMySqlConnection;
        private System.Windows.Forms.GroupBox grpAuthors;
        private System.Windows.Forms.TextBox txtAuthors;
        private System.Windows.Forms.ToolStripStatusLabel lblDateTime;
        private System.Windows.Forms.Button btnSetTitle;
        private System.Windows.Forms.CheckBox chkNoAPI;
        private System.Windows.Forms.GroupBox grpFtp;
        private System.Windows.Forms.TextBox txtFtpPassword;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFtpUserName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtFtpUrl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnTestFtpConnection;
        private System.Windows.Forms.CheckBox chkResizeImages;
        private System.Windows.Forms.Label lblResizePix;
        private System.Windows.Forms.NumericUpDown numMaxImageDimension;
        private System.Windows.Forms.GroupBox grpTop;
        private System.Windows.Forms.CheckedListBox chkSites;
        private System.Windows.Forms.CheckBox chkAllPages;
        private System.Windows.Forms.Label lblPageTo;
        private System.Windows.Forms.NumericUpDown numPageTo;
        private System.Windows.Forms.NumericUpDown numPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnScrumble;
        private System.Windows.Forms.Button txtFindDuplicatePosts;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.Button btnRemoveDuplicates;
        private System.Windows.Forms.Button btnNavigate;
        private System.Windows.Forms.NumericUpDown numThumbnailSize;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnGoAsync;
    }
}

