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
            this.label1 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numPage = new System.Windows.Forms.NumericUpDown();
            this.btnStart = new System.Windows.Forms.Button();
            this.lvItems = new System.Windows.Forms.ListView();
            this.No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Url = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MetaDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.content = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.price = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Image = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.postId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.chkClearResults = new System.Windows.Forms.CheckBox();
            this.numPageTo = new System.Windows.Forms.NumericUpDown();
            this.lblPageTo = new System.Windows.Forms.Label();
            this.chkAllPages = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.barStatus = new System.Windows.Forms.ToolStripProgressBar();
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
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).BeginInit();
            this.grpBlogProp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageTo)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.grpMysql.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Url To Scrape";
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(100, 10);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(368, 20);
            this.txtUrl.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(474, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Page";
            // 
            // numPage
            // 
            this.numPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPage.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numPage.Location = new System.Drawing.Point(512, 13);
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
            this.numPage.TabIndex = 3;
            this.numPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPage.ValueChanged += new System.EventHandler(this.numPage_ValueChanged);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(932, 12);
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
            this.No,
            this.Id,
            this.Url,
            this.Title,
            this.MetaDescription,
            this.content,
            this.price,
            this.Image,
            this.tags,
            this.postId});
            this.lvItems.FullRowSelect = true;
            this.lvItems.GridLines = true;
            this.lvItems.HideSelection = false;
            this.lvItems.Location = new System.Drawing.Point(15, 52);
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(1084, 340);
            this.lvItems.TabIndex = 5;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            // 
            // No
            // 
            this.No.Text = "#";
            // 
            // Id
            // 
            this.Id.Text = "Id";
            this.Id.Width = 75;
            // 
            // Url
            // 
            this.Url.Text = "Url";
            this.Url.Width = 104;
            // 
            // Title
            // 
            this.Title.Text = "Title";
            this.Title.Width = 199;
            // 
            // MetaDescription
            // 
            this.MetaDescription.Text = "Meta Description";
            this.MetaDescription.Width = 242;
            // 
            // content
            // 
            this.content.Text = "Content";
            // 
            // price
            // 
            this.price.Text = "price";
            // 
            // Image
            // 
            this.Image.Text = "Image";
            this.Image.Width = 117;
            // 
            // tags
            // 
            this.tags.Text = "Tags";
            // 
            // postId
            // 
            this.postId.Text = "Post Id";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.Location = new System.Drawing.Point(15, 398);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(85, 23);
            this.btnSelectAll.TabIndex = 6;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // grpBlogProp
            // 
            this.grpBlogProp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBlogProp.Controls.Add(this.txtPassword);
            this.grpBlogProp.Controls.Add(this.label5);
            this.grpBlogProp.Controls.Add(this.txtUserName);
            this.grpBlogProp.Controls.Add(this.label4);
            this.grpBlogProp.Controls.Add(this.txtBlogUrl);
            this.grpBlogProp.Controls.Add(this.label3);
            this.grpBlogProp.Location = new System.Drawing.Point(15, 438);
            this.grpBlogProp.Name = "grpBlogProp";
            this.grpBlogProp.Size = new System.Drawing.Size(1097, 71);
            this.grpBlogProp.TabIndex = 7;
            this.grpBlogProp.TabStop = false;
            this.grpBlogProp.Text = "Target Blog";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(633, 26);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(182, 20);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.Text = "Kazmanot111+";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(562, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Password";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(362, 26);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(182, 20);
            this.txtUserName.TabIndex = 4;
            this.txtUserName.Text = "yuksel";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(296, 30);
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
            this.btnGo.Location = new System.Drawing.Point(703, 616);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(214, 23);
            this.btnGo.TabIndex = 8;
            this.btnGo.Text = "Create Selected Items on the Blog";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(936, 617);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(176, 23);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStopScrape
            // 
            this.btnStopScrape.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopScrape.Location = new System.Drawing.Point(1023, 13);
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
            this.btnGetPost.Location = new System.Drawing.Point(168, 613);
            this.btnGetPost.Name = "btnGetPost";
            this.btnGetPost.Size = new System.Drawing.Size(141, 23);
            this.btnGetPost.TabIndex = 11;
            this.btnGetPost.Text = "Get Selected Post";
            this.btnGetPost.UseVisualStyleBackColor = true;
            this.btnGetPost.Click += new System.EventHandler(this.btnGetPost_Click);
            // 
            // txtPostId
            // 
            this.txtPostId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPostId.Location = new System.Drawing.Point(52, 613);
            this.txtPostId.Name = "txtPostId";
            this.txtPostId.Size = new System.Drawing.Size(101, 20);
            this.txtPostId.TabIndex = 12;
            // 
            // chkFeatureImage
            // 
            this.chkFeatureImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFeatureImage.AutoSize = true;
            this.chkFeatureImage.Location = new System.Drawing.Point(491, 616);
            this.chkFeatureImage.Name = "chkFeatureImage";
            this.chkFeatureImage.Size = new System.Drawing.Size(192, 17);
            this.chkFeatureImage.TabIndex = 13;
            this.chkFeatureImage.Text = "Make First Image as Feature Image";
            this.chkFeatureImage.UseVisualStyleBackColor = true;
            // 
            // chkClearResults
            // 
            this.chkClearResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkClearResults.AutoSize = true;
            this.chkClearResults.Checked = true;
            this.chkClearResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClearResults.Location = new System.Drawing.Point(798, 16);
            this.chkClearResults.Name = "chkClearResults";
            this.chkClearResults.Size = new System.Drawing.Size(128, 17);
            this.chkClearResults.TabIndex = 14;
            this.chkClearResults.Text = "Clear Existent Results";
            this.chkClearResults.UseVisualStyleBackColor = true;
            // 
            // numPageTo
            // 
            this.numPageTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPageTo.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numPageTo.Location = new System.Drawing.Point(613, 14);
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
            this.numPageTo.TabIndex = 15;
            this.numPageTo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPageTo.ValueChanged += new System.EventHandler(this.numPageTo_ValueChanged);
            // 
            // lblPageTo
            // 
            this.lblPageTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPageTo.AutoSize = true;
            this.lblPageTo.Location = new System.Drawing.Point(585, 18);
            this.lblPageTo.Name = "lblPageTo";
            this.lblPageTo.Size = new System.Drawing.Size(20, 13);
            this.lblPageTo.TabIndex = 16;
            this.lblPageTo.Text = "To";
            // 
            // chkAllPages
            // 
            this.chkAllPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllPages.AutoSize = true;
            this.chkAllPages.Location = new System.Drawing.Point(686, 17);
            this.chkAllPages.Name = "chkAllPages";
            this.chkAllPages.Size = new System.Drawing.Size(70, 17);
            this.chkAllPages.TabIndex = 17;
            this.chkAllPages.Text = "All Pages";
            this.chkAllPages.UseVisualStyleBackColor = true;
            this.chkAllPages.CheckedChanged += new System.EventHandler(this.chkAllPages_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.barStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 647);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1132, 22);
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
            // chkCache
            // 
            this.chkCache.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCache.AutoSize = true;
            this.chkCache.Checked = true;
            this.chkCache.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCache.Location = new System.Drawing.Point(398, 617);
            this.chkCache.Name = "chkCache";
            this.chkCache.Size = new System.Drawing.Size(87, 17);
            this.chkCache.TabIndex = 19;
            this.chkCache.Text = "Use Caching";
            this.chkCache.UseVisualStyleBackColor = true;
            this.chkCache.Visible = false;
            // 
            // grpMysql
            // 
            this.grpMysql.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMysql.Controls.Add(this.btnTestMySqlConnection);
            this.grpMysql.Controls.Add(this.txtMySqlDatabase);
            this.grpMysql.Controls.Add(this.lblDatabase);
            this.grpMysql.Controls.Add(this.txtMySqlPass);
            this.grpMysql.Controls.Add(this.label6);
            this.grpMysql.Controls.Add(this.txtMysqlUser);
            this.grpMysql.Controls.Add(this.label7);
            this.grpMysql.Controls.Add(this.txtMySqlIp);
            this.grpMysql.Controls.Add(this.label8);
            this.grpMysql.Location = new System.Drawing.Point(15, 528);
            this.grpMysql.Name = "grpMysql";
            this.grpMysql.Size = new System.Drawing.Size(1097, 71);
            this.grpMysql.TabIndex = 20;
            this.grpMysql.TabStop = false;
            this.grpMysql.Text = "Target Blog MySql Settings";
            // 
            // btnTestMySqlConnection
            // 
            this.btnTestMySqlConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestMySqlConnection.Location = new System.Drawing.Point(859, 21);
            this.btnTestMySqlConnection.Name = "btnTestMySqlConnection";
            this.btnTestMySqlConnection.Size = new System.Drawing.Size(141, 23);
            this.btnTestMySqlConnection.TabIndex = 12;
            this.btnTestMySqlConnection.Text = "Test mysql connection";
            this.btnTestMySqlConnection.UseVisualStyleBackColor = true;
            this.btnTestMySqlConnection.Click += new System.EventHandler(this.btnTestMySqlConnection_Click);
            // 
            // txtMySqlDatabase
            // 
            this.txtMySqlDatabase.Location = new System.Drawing.Point(712, 23);
            this.txtMySqlDatabase.Name = "txtMySqlDatabase";
            this.txtMySqlDatabase.Size = new System.Drawing.Size(118, 20);
            this.txtMySqlDatabase.TabIndex = 8;
            this.txtMySqlDatabase.Text = "nalgor5_wpgonbl";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(653, 26);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(53, 13);
            this.lblDatabase.TabIndex = 7;
            this.lblDatabase.Text = "Database";
            // 
            // txtMySqlPass
            // 
            this.txtMySqlPass.Location = new System.Drawing.Point(532, 23);
            this.txtMySqlPass.Name = "txtMySqlPass";
            this.txtMySqlPass.PasswordChar = '*';
            this.txtMySqlPass.Size = new System.Drawing.Size(102, 20);
            this.txtMySqlPass.TabIndex = 6;
            this.txtMySqlPass.Text = "S]P-a588C3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(473, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Password";
            // 
            // txtMysqlUser
            // 
            this.txtMysqlUser.Location = new System.Drawing.Point(340, 23);
            this.txtMysqlUser.Name = "txtMysqlUser";
            this.txtMysqlUser.Size = new System.Drawing.Size(113, 20);
            this.txtMysqlUser.TabIndex = 4;
            this.txtMysqlUser.Text = "nalgor5_wpgonbl";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(277, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "UserName";
            // 
            // txtMySqlIp
            // 
            this.txtMySqlIp.Location = new System.Drawing.Point(75, 23);
            this.txtMySqlIp.Name = "txtMySqlIp";
            this.txtMySqlIp.Size = new System.Drawing.Size(182, 20);
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
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 669);
            this.Controls.Add(this.grpMysql);
            this.Controls.Add(this.chkCache);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.chkAllPages);
            this.Controls.Add(this.lblPageTo);
            this.Controls.Add(this.numPageTo);
            this.Controls.Add(this.chkClearResults);
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
            this.Controls.Add(this.numPage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.label1);
            this.Name = "frmMain";
            this.Text = "Wordpress Scraper";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).EndInit();
            this.grpBlogProp.ResumeLayout(false);
            this.grpBlogProp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageTo)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpMysql.ResumeLayout(false);
            this.grpMysql.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numPage;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListView lvItems;
        private System.Windows.Forms.ColumnHeader Id;
        private System.Windows.Forms.ColumnHeader Url;
        private System.Windows.Forms.ColumnHeader Title;
        private System.Windows.Forms.ColumnHeader MetaDescription;
        private System.Windows.Forms.ColumnHeader Image;
        private System.Windows.Forms.ColumnHeader No;
        private System.Windows.Forms.ColumnHeader content;
        private System.Windows.Forms.ColumnHeader price;
        private System.Windows.Forms.ColumnHeader tags;
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
        private System.Windows.Forms.ColumnHeader postId;
        private System.Windows.Forms.Button btnStopScrape;
        private System.Windows.Forms.Button btnGetPost;
        private System.Windows.Forms.TextBox txtPostId;
        private System.Windows.Forms.CheckBox chkFeatureImage;
        private System.Windows.Forms.CheckBox chkClearResults;
        private System.Windows.Forms.NumericUpDown numPageTo;
        private System.Windows.Forms.Label lblPageTo;
        private System.Windows.Forms.CheckBox chkAllPages;
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
    }
}

