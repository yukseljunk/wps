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
            System.Windows.Forms.ColumnHeader date;
            this.btnStart = new System.Windows.Forms.Button();
            this.lvItems = new System.Windows.Forms.ListView();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStopScrape = new System.Windows.Forms.Button();
            this.btnGetPost = new System.Windows.Forms.Button();
            this.txtPostId = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.barStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.lblDateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSetTitle = new System.Windows.Forms.Button();
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1.SuspendLayout();
            this.grpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).BeginInit();
            this.menuStrip1.SuspendLayout();
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
            // date
            // 
            date.Text = "Date";
            date.Width = 98;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(1096, 54);
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
            date,
            postId});
            this.lvItems.FullRowSelect = true;
            this.lvItems.GridLines = true;
            this.lvItems.HideSelection = false;
            this.lvItems.Location = new System.Drawing.Point(14, 112);
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(1247, 464);
            this.lvItems.TabIndex = 5;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvItems_ColumnClick);
            this.lvItems.SelectedIndexChanged += new System.EventHandler(this.lvItems_SelectedIndexChanged);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.Location = new System.Drawing.Point(105, 597);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(85, 23);
            this.btnSelectAll.TabIndex = 6;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(970, 640);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(190, 23);
            this.btnGo.TabIndex = 8;
            this.btnGo.Text = "Create Selected on the Blog";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(1165, 640);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(97, 23);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStopScrape
            // 
            this.btnStopScrape.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopScrape.Location = new System.Drawing.Point(1187, 55);
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
            this.btnGetPost.Location = new System.Drawing.Point(781, 600);
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
            this.txtPostId.Location = new System.Drawing.Point(911, 602);
            this.txtPostId.Name = "txtPostId";
            this.txtPostId.Size = new System.Drawing.Size(118, 20);
            this.txtPostId.TabIndex = 12;
            this.txtPostId.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.barStatus,
            this.lblDateTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 669);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1295, 22);
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
            // btnSetTitle
            // 
            this.btnSetTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetTitle.Location = new System.Drawing.Point(1041, 600);
            this.btnSetTitle.Name = "btnSetTitle";
            this.btnSetTitle.Size = new System.Drawing.Size(132, 23);
            this.btnSetTitle.TabIndex = 22;
            this.btnSetTitle.Text = "Set Title";
            this.btnSetTitle.UseVisualStyleBackColor = true;
            this.btnSetTitle.Visible = false;
            this.btnSetTitle.Click += new System.EventHandler(this.btnSetTitle_Click);
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
            this.grpTop.Location = new System.Drawing.Point(12, 27);
            this.grpTop.Name = "grpTop";
            this.grpTop.Size = new System.Drawing.Size(931, 66);
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
            this.chkSites.Location = new System.Drawing.Point(323, 24);
            this.chkSites.MultiColumn = true;
            this.chkSites.Name = "chkSites";
            this.chkSites.Size = new System.Drawing.Size(300, 34);
            this.chkSites.TabIndex = 33;
            // 
            // chkAllPages
            // 
            this.chkAllPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllPages.AutoSize = true;
            this.chkAllPages.Location = new System.Drawing.Point(845, 31);
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
            this.lblPageTo.Location = new System.Drawing.Point(744, 32);
            this.lblPageTo.Name = "lblPageTo";
            this.lblPageTo.Size = new System.Drawing.Size(20, 13);
            this.lblPageTo.TabIndex = 31;
            this.lblPageTo.Text = "To";
            // 
            // numPageTo
            // 
            this.numPageTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPageTo.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numPageTo.Location = new System.Drawing.Point(772, 28);
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
            this.numPage.Location = new System.Drawing.Point(671, 27);
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
            this.label2.Location = new System.Drawing.Point(633, 30);
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
            this.txtUrl.Size = new System.Drawing.Size(296, 20);
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
            this.btnScrumble.Location = new System.Drawing.Point(14, 597);
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
            this.txtFindDuplicatePosts.Location = new System.Drawing.Point(336, 597);
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
            this.btnRemoveSelected.Location = new System.Drawing.Point(196, 597);
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
            this.btnRemoveDuplicates.Location = new System.Drawing.Point(474, 597);
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
            this.btnNavigate.Location = new System.Drawing.Point(607, 599);
            this.btnNavigate.Name = "btnNavigate";
            this.btnNavigate.Size = new System.Drawing.Size(126, 23);
            this.btnNavigate.TabIndex = 35;
            this.btnNavigate.Text = "Go to link...";
            this.btnNavigate.UseVisualStyleBackColor = true;
            this.btnNavigate.Click += new System.EventHandler(this.btnNavigate_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1295, 24);
            this.menuStrip1.TabIndex = 36;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1295, 691);
            this.Controls.Add(this.btnNavigate);
            this.Controls.Add(this.btnRemoveDuplicates);
            this.Controls.Add(this.btnRemoveSelected);
            this.Controls.Add(this.txtFindDuplicatePosts);
            this.Controls.Add(this.btnScrumble);
            this.Controls.Add(this.grpTop);
            this.Controls.Add(this.btnSetTitle);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.txtPostId);
            this.Controls.Add(this.btnGetPost);
            this.Controls.Add(this.btnStopScrape);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.btnStart);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Wordpress Scraper";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpTop.ResumeLayout(false);
            this.grpTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListView lvItems;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStopScrape;
        private System.Windows.Forms.Button btnGetPost;
        private System.Windows.Forms.TextBox txtPostId;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar barStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblDateTime;
        private System.Windows.Forms.Button btnSetTitle;
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

