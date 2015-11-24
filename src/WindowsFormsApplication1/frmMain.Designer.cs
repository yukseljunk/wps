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
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).BeginInit();
            this.grpBlogProp.SuspendLayout();
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
            this.txtUrl.Size = new System.Drawing.Size(731, 20);
            this.txtUrl.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(852, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Page";
            // 
            // numPage
            // 
            this.numPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPage.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numPage.Location = new System.Drawing.Point(901, 11);
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
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(974, 11);
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
            this.lvItems.Size = new System.Drawing.Size(1144, 430);
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
            this.btnSelectAll.Location = new System.Drawing.Point(15, 488);
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
            this.grpBlogProp.Location = new System.Drawing.Point(15, 528);
            this.grpBlogProp.Name = "grpBlogProp";
            this.grpBlogProp.Size = new System.Drawing.Size(1157, 71);
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
            this.btnGo.Location = new System.Drawing.Point(764, 614);
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
            this.btnStop.Location = new System.Drawing.Point(996, 614);
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
            this.btnStopScrape.Location = new System.Drawing.Point(1083, 13);
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
            this.btnGetPost.Location = new System.Drawing.Point(247, 612);
            this.btnGetPost.Name = "btnGetPost";
            this.btnGetPost.Size = new System.Drawing.Size(214, 23);
            this.btnGetPost.TabIndex = 11;
            this.btnGetPost.Text = "Get Selected Post";
            this.btnGetPost.UseVisualStyleBackColor = true;
            this.btnGetPost.Click += new System.EventHandler(this.btnGetPost_Click);
            // 
            // txtPostId
            // 
            this.txtPostId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPostId.Location = new System.Drawing.Point(30, 613);
            this.txtPostId.Name = "txtPostId";
            this.txtPostId.Size = new System.Drawing.Size(182, 20);
            this.txtPostId.TabIndex = 12;
            // 
            // chkFeatureImage
            // 
            this.chkFeatureImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFeatureImage.AutoSize = true;
            this.chkFeatureImage.Location = new System.Drawing.Point(555, 616);
            this.chkFeatureImage.Name = "chkFeatureImage";
            this.chkFeatureImage.Size = new System.Drawing.Size(192, 17);
            this.chkFeatureImage.TabIndex = 13;
            this.chkFeatureImage.Text = "Make First Image as Feature Image";
            this.chkFeatureImage.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 663);
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
    }
}

