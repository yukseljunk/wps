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
            this.components = new System.ComponentModel.Container();
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
            System.Windows.Forms.ColumnHeader relevance;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnStart = new System.Windows.Forms.Button();
            this.lvItems = new System.Windows.Forms.ListView();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStopScrape = new System.Windows.Forms.Button();
            this.btnGetPost = new System.Windows.Forms.Button();
            this.txtPostId = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.barStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.lblDateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelection = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createAuthorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.publishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.addUpdateExtraFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanupBlogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTotalResults = new System.Windows.Forms.Label();
            this.totalCountTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlItemOps = new System.Windows.Forms.Panel();
            this.btnPublish = new System.Windows.Forms.Button();
            this.btnMultiplyPrice = new System.Windows.Forms.Button();
            this.txtPriceCoeff = new System.Windows.Forms.TextBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnRelevanceScramble = new System.Windows.Forms.Button();
            this.btnNavigate = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnRemoveDuplicates = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.txtFindDuplicatePosts = new System.Windows.Forms.Button();
            this.btnScrumble = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.openSettingFile = new System.Windows.Forms.OpenFileDialog();
            this.saveSettings = new System.Windows.Forms.SaveFileDialog();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
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
            relevance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1.SuspendLayout();
            this.grpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.pnlItemOps.SuspendLayout();
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
            // relevance
            // 
            relevance.Text = "Relevance";
            relevance.Width = 73;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(1245, 54);
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
            relevance,
            postId});
            this.lvItems.FullRowSelect = true;
            this.lvItems.GridLines = true;
            this.lvItems.HideSelection = false;
            this.lvItems.Location = new System.Drawing.Point(14, 112);
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(1373, 388);
            this.lvItems.TabIndex = 5;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvItems_ColumnClick);
            this.lvItems.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvItems_ItemSelectionChanged);
            this.lvItems.SelectedIndexChanged += new System.EventHandler(this.lvItems_SelectedIndexChanged);
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(1119, 564);
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
            this.btnStop.Location = new System.Drawing.Point(1314, 564);
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
            this.btnStopScrape.Location = new System.Drawing.Point(1336, 55);
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
            this.btnGetPost.Location = new System.Drawing.Point(350, 559);
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
            this.txtPostId.Location = new System.Drawing.Point(496, 561);
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
            this.lblDateTime,
            this.lblSelection});
            this.statusStrip1.Location = new System.Drawing.Point(0, 593);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1444, 22);
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
            // lblSelection
            // 
            this.lblSelection.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(68, 17);
            this.lblSelection.Text = "lblSelection";
            // 
            // btnSetTitle
            // 
            this.btnSetTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetTitle.Location = new System.Drawing.Point(629, 558);
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
            this.grpTop.Size = new System.Drawing.Size(1198, 66);
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
            this.chkSites.Location = new System.Drawing.Point(590, 24);
            this.chkSites.MultiColumn = true;
            this.chkSites.Name = "chkSites";
            this.chkSites.Size = new System.Drawing.Size(300, 34);
            this.chkSites.TabIndex = 33;
            this.chkSites.SelectedIndexChanged += new System.EventHandler(this.chkSites_SelectedIndexChanged);
            // 
            // chkAllPages
            // 
            this.chkAllPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllPages.AutoSize = true;
            this.chkAllPages.Location = new System.Drawing.Point(1112, 31);
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
            this.lblPageTo.Location = new System.Drawing.Point(1011, 32);
            this.lblPageTo.Name = "lblPageTo";
            this.lblPageTo.Size = new System.Drawing.Size(20, 13);
            this.lblPageTo.TabIndex = 31;
            this.lblPageTo.Text = "To";
            // 
            // numPageTo
            // 
            this.numPageTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPageTo.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numPageTo.Location = new System.Drawing.Point(1039, 28);
            this.numPageTo.Maximum = new decimal(new int[] {
            5000,
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
            this.numPage.Location = new System.Drawing.Point(938, 27);
            this.numPage.Maximum = new decimal(new int[] {
            5000,
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
            this.label2.Location = new System.Drawing.Point(900, 30);
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
            this.txtUrl.Size = new System.Drawing.Size(563, 20);
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
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1444, 24);
            this.menuStrip1.TabIndex = 36;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.toolStripMenuItem5,
            this.settingsToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exportToolStripMenuItem,
            this.ImportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.exportToolStripMenuItem.Text = "Export to Excel...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // ImportToolStripMenuItem
            // 
            this.ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
            this.ImportToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.ImportToolStripMenuItem.Text = "Import from Excel...";
            this.ImportToolStripMenuItem.Click += new System.EventHandler(this.ImportToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(174, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createAuthorsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.publishToolStripMenuItem,
            this.toolStripMenuItem1,
            this.addUpdateExtraFilesToolStripMenuItem,
            this.cleanupBlogToolStripMenuItem,
            this.toolStripMenuItem4,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // createAuthorsToolStripMenuItem
            // 
            this.createAuthorsToolStripMenuItem.Name = "createAuthorsToolStripMenuItem";
            this.createAuthorsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.createAuthorsToolStripMenuItem.Text = "Edit Authors";
            this.createAuthorsToolStripMenuItem.Click += new System.EventHandler(this.createAuthorsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(190, 6);
            // 
            // publishToolStripMenuItem
            // 
            this.publishToolStripMenuItem.Name = "publishToolStripMenuItem";
            this.publishToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.publishToolStripMenuItem.Text = "Publish";
            this.publishToolStripMenuItem.Click += new System.EventHandler(this.publishToolStripMenuItem_Click_1);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(190, 6);
            // 
            // addUpdateExtraFilesToolStripMenuItem
            // 
            this.addUpdateExtraFilesToolStripMenuItem.Name = "addUpdateExtraFilesToolStripMenuItem";
            this.addUpdateExtraFilesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.addUpdateExtraFilesToolStripMenuItem.Text = "Add/Update Extra Files";
            this.addUpdateExtraFilesToolStripMenuItem.Click += new System.EventHandler(this.addUpdateExtraFilesToolStripMenuItem_Click);
            // 
            // cleanupBlogToolStripMenuItem
            // 
            this.cleanupBlogToolStripMenuItem.Name = "cleanupBlogToolStripMenuItem";
            this.cleanupBlogToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.cleanupBlogToolStripMenuItem.Text = "Cleanup Blog...";
            this.cleanupBlogToolStripMenuItem.Click += new System.EventHandler(this.cleanupBlogToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(190, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // lblTotalResults
            // 
            this.lblTotalResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotalResults.AutoSize = true;
            this.lblTotalResults.Location = new System.Drawing.Point(12, 564);
            this.lblTotalResults.Name = "lblTotalResults";
            this.lblTotalResults.Size = new System.Drawing.Size(13, 13);
            this.lblTotalResults.TabIndex = 37;
            this.lblTotalResults.Tag = "";
            this.lblTotalResults.Text = "0";
            // 
            // totalCountTooltip
            // 
            this.totalCountTooltip.IsBalloon = true;
            // 
            // pnlItemOps
            // 
            this.pnlItemOps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlItemOps.Controls.Add(this.btnPublish);
            this.pnlItemOps.Controls.Add(this.btnMultiplyPrice);
            this.pnlItemOps.Controls.Add(this.txtPriceCoeff);
            this.pnlItemOps.Controls.Add(this.btnDown);
            this.pnlItemOps.Controls.Add(this.btnRelevanceScramble);
            this.pnlItemOps.Controls.Add(this.btnNavigate);
            this.pnlItemOps.Controls.Add(this.btnUp);
            this.pnlItemOps.Controls.Add(this.btnRemoveDuplicates);
            this.pnlItemOps.Controls.Add(this.btnRemoveSelected);
            this.pnlItemOps.Controls.Add(this.txtFindDuplicatePosts);
            this.pnlItemOps.Controls.Add(this.btnScrumble);
            this.pnlItemOps.Controls.Add(this.btnSelectAll);
            this.pnlItemOps.Location = new System.Drawing.Point(15, 506);
            this.pnlItemOps.Name = "pnlItemOps";
            this.pnlItemOps.Size = new System.Drawing.Size(1426, 46);
            this.pnlItemOps.TabIndex = 39;
            // 
            // btnPublish
            // 
            this.btnPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublish.Location = new System.Drawing.Point(1018, 13);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(126, 23);
            this.btnPublish.TabIndex = 49;
            this.btnPublish.Text = "Publish Selected...";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // btnMultiplyPrice
            // 
            this.btnMultiplyPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMultiplyPrice.Location = new System.Drawing.Point(1158, 13);
            this.btnMultiplyPrice.Name = "btnMultiplyPrice";
            this.btnMultiplyPrice.Size = new System.Drawing.Size(126, 23);
            this.btnMultiplyPrice.TabIndex = 48;
            this.btnMultiplyPrice.Text = "Multiply Price By:";
            this.btnMultiplyPrice.UseVisualStyleBackColor = true;
            this.btnMultiplyPrice.Click += new System.EventHandler(this.btnMultiplyPrice_Click);
            // 
            // txtPriceCoeff
            // 
            this.txtPriceCoeff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPriceCoeff.Location = new System.Drawing.Point(1292, 14);
            this.txtPriceCoeff.Name = "txtPriceCoeff";
            this.txtPriceCoeff.Size = new System.Drawing.Size(56, 20);
            this.txtPriceCoeff.TabIndex = 40;
            this.txtPriceCoeff.Text = "0.04";
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnDown.Location = new System.Drawing.Point(304, 10);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(74, 24);
            this.btnDown.TabIndex = 47;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnRelevanceScramble
            // 
            this.btnRelevanceScramble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRelevanceScramble.Location = new System.Drawing.Point(91, 9);
            this.btnRelevanceScramble.Name = "btnRelevanceScramble";
            this.btnRelevanceScramble.Size = new System.Drawing.Size(129, 23);
            this.btnRelevanceScramble.TabIndex = 45;
            this.btnRelevanceScramble.Text = "Relevance Scramble ";
            this.btnRelevanceScramble.UseVisualStyleBackColor = true;
            this.btnRelevanceScramble.Click += new System.EventHandler(this.btnRelevanceScramble_Click);
            // 
            // btnNavigate
            // 
            this.btnNavigate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNavigate.Location = new System.Drawing.Point(886, 13);
            this.btnNavigate.Name = "btnNavigate";
            this.btnNavigate.Size = new System.Drawing.Size(126, 23);
            this.btnNavigate.TabIndex = 44;
            this.btnNavigate.Text = "Go to link...";
            this.btnNavigate.UseVisualStyleBackColor = true;
            this.btnNavigate.Click += new System.EventHandler(this.btnNavigate_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnUp.Location = new System.Drawing.Point(226, 11);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(72, 23);
            this.btnUp.TabIndex = 46;
            this.btnUp.Text = "Up";
            this.btnUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnRemoveDuplicates
            // 
            this.btnRemoveDuplicates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveDuplicates.Location = new System.Drawing.Point(753, 11);
            this.btnRemoveDuplicates.Name = "btnRemoveDuplicates";
            this.btnRemoveDuplicates.Size = new System.Drawing.Size(126, 23);
            this.btnRemoveDuplicates.TabIndex = 43;
            this.btnRemoveDuplicates.Text = "Remove Duplicates";
            this.btnRemoveDuplicates.UseVisualStyleBackColor = true;
            this.btnRemoveDuplicates.Click += new System.EventHandler(this.btnRemoveDuplicates_Click);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveSelected.Location = new System.Drawing.Point(475, 11);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(126, 23);
            this.btnRemoveSelected.TabIndex = 42;
            this.btnRemoveSelected.Text = "Remove Selected";
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // txtFindDuplicatePosts
            // 
            this.txtFindDuplicatePosts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtFindDuplicatePosts.Location = new System.Drawing.Point(615, 11);
            this.txtFindDuplicatePosts.Name = "txtFindDuplicatePosts";
            this.txtFindDuplicatePosts.Size = new System.Drawing.Size(132, 23);
            this.txtFindDuplicatePosts.TabIndex = 41;
            this.txtFindDuplicatePosts.Text = "Find Duplicates";
            this.txtFindDuplicatePosts.UseVisualStyleBackColor = true;
            this.txtFindDuplicatePosts.Click += new System.EventHandler(this.txtFindDuplicatePosts_Click);
            // 
            // btnScrumble
            // 
            this.btnScrumble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnScrumble.Location = new System.Drawing.Point(0, 9);
            this.btnScrumble.Name = "btnScrumble";
            this.btnScrumble.Size = new System.Drawing.Size(85, 23);
            this.btnScrumble.TabIndex = 40;
            this.btnScrumble.Text = "Scramble";
            this.btnScrumble.UseVisualStyleBackColor = true;
            this.btnScrumble.Click += new System.EventHandler(this.btnScrumble_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.Location = new System.Drawing.Point(384, 11);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(85, 23);
            this.btnSelectAll.TabIndex = 39;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // openSettingFile
            // 
            this.openSettingFile.FileName = "openFileDialog1";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(174, 6);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1444, 615);
            this.Controls.Add(this.pnlItemOps);
            this.Controls.Add(this.lblTotalResults);
            this.Controls.Add(this.grpTop);
            this.Controls.Add(this.btnSetTitle);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.txtPostId);
            this.Controls.Add(this.btnGetPost);
            this.Controls.Add(this.btnStopScrape);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.btnStart);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Wordpress Scraper";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpTop.ResumeLayout(false);
            this.grpTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlItemOps.ResumeLayout(false);
            this.pnlItemOps.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListView lvItems;
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label lblTotalResults;
        private System.Windows.Forms.ToolTip totalCountTooltip;
        private System.Windows.Forms.Panel pnlItemOps;
        private System.Windows.Forms.Button btnRelevanceScramble;
        private System.Windows.Forms.Button btnNavigate;
        private System.Windows.Forms.Button btnRemoveDuplicates;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.Button txtFindDuplicatePosts;
        private System.Windows.Forms.Button btnScrumble;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.ToolStripMenuItem addUpdateExtraFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createAuthorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Button btnMultiplyPrice;
        private System.Windows.Forms.TextBox txtPriceCoeff;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem publishToolStripMenuItem;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.OpenFileDialog openSettingFile;
        private System.Windows.Forms.SaveFileDialog saveSettings;
        private System.Windows.Forms.ToolStripMenuItem cleanupBlogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripStatusLabel lblSelection;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
    }
}

