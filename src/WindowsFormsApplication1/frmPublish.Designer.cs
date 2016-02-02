namespace WordpressScraper
{
    partial class frmPublish
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
            this.btnPublish = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numNumberOfPosts = new System.Windows.Forms.NumericUpDown();
            this.cbCriteria = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkCreateSlide = new System.Windows.Forms.CheckBox();
            this.pnlSlideShow = new System.Windows.Forms.Panel();
            this.numImagePerPost = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numVideoHeight = new System.Windows.Forms.NumericUpDown();
            this.numVideoWidth = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numDurationForEachImage = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numVideoPerPost = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlYoutube = new System.Windows.Forms.Panel();
            this.txtYoutubeTags = new System.Windows.Forms.TextBox();
            this.txtYoutubeDescription = new System.Windows.Forms.TextBox();
            this.txtYoutubeTitle = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.chkYoutube = new System.Windows.Forms.CheckBox();
            this.txtRefreshToken = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numNumberOfPosts)).BeginInit();
            this.pnlSlideShow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numImagePerPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVideoHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVideoWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDurationForEachImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVideoPerPost)).BeginInit();
            this.pnlYoutube.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPublish
            // 
            this.btnPublish.Location = new System.Drawing.Point(244, 552);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(75, 23);
            this.btnPublish.TabIndex = 0;
            this.btnPublish.Text = "Publish";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Publish";
            // 
            // numNumberOfPosts
            // 
            this.numNumberOfPosts.Location = new System.Drawing.Point(207, 30);
            this.numNumberOfPosts.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numNumberOfPosts.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNumberOfPosts.Name = "numNumberOfPosts";
            this.numNumberOfPosts.Size = new System.Drawing.Size(68, 20);
            this.numNumberOfPosts.TabIndex = 2;
            this.numNumberOfPosts.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numNumberOfPosts.ValueChanged += new System.EventHandler(this.numNumberOfPosts_ValueChanged);
            // 
            // cbCriteria
            // 
            this.cbCriteria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCriteria.FormattingEnabled = true;
            this.cbCriteria.Items.AddRange(new object[] {
            "Newest",
            "Oldest",
            "Random",
            "Selected Items"});
            this.cbCriteria.Location = new System.Drawing.Point(80, 30);
            this.cbCriteria.Name = "cbCriteria";
            this.cbCriteria.Size = new System.Drawing.Size(121, 21);
            this.cbCriteria.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(281, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "posts";
            // 
            // chkCreateSlide
            // 
            this.chkCreateSlide.AutoSize = true;
            this.chkCreateSlide.Location = new System.Drawing.Point(26, 71);
            this.chkCreateSlide.Name = "chkCreateSlide";
            this.chkCreateSlide.Size = new System.Drawing.Size(113, 17);
            this.chkCreateSlide.TabIndex = 5;
            this.chkCreateSlide.Text = "Create Slide Show";
            this.chkCreateSlide.UseVisualStyleBackColor = true;
            this.chkCreateSlide.CheckedChanged += new System.EventHandler(this.chkCreateSlide_CheckedChanged);
            // 
            // pnlSlideShow
            // 
            this.pnlSlideShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSlideShow.Controls.Add(this.numImagePerPost);
            this.pnlSlideShow.Controls.Add(this.label11);
            this.pnlSlideShow.Controls.Add(this.label12);
            this.pnlSlideShow.Controls.Add(this.label10);
            this.pnlSlideShow.Controls.Add(this.label9);
            this.pnlSlideShow.Controls.Add(this.numVideoHeight);
            this.pnlSlideShow.Controls.Add(this.numVideoWidth);
            this.pnlSlideShow.Controls.Add(this.label8);
            this.pnlSlideShow.Controls.Add(this.label7);
            this.pnlSlideShow.Controls.Add(this.label6);
            this.pnlSlideShow.Controls.Add(this.numDurationForEachImage);
            this.pnlSlideShow.Controls.Add(this.label5);
            this.pnlSlideShow.Controls.Add(this.numVideoPerPost);
            this.pnlSlideShow.Controls.Add(this.label4);
            this.pnlSlideShow.Controls.Add(this.label3);
            this.pnlSlideShow.Enabled = false;
            this.pnlSlideShow.Location = new System.Drawing.Point(26, 95);
            this.pnlSlideShow.Name = "pnlSlideShow";
            this.pnlSlideShow.Size = new System.Drawing.Size(374, 160);
            this.pnlSlideShow.TabIndex = 6;
            // 
            // numImagePerPost
            // 
            this.numImagePerPost.Location = new System.Drawing.Point(180, 40);
            this.numImagePerPost.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numImagePerPost.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numImagePerPost.Name = "numImagePerPost";
            this.numImagePerPost.Size = new System.Drawing.Size(68, 20);
            this.numImagePerPost.TabIndex = 18;
            this.numImagePerPost.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(254, 42);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "image(s)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(138, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "# of images From each post";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(253, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "px";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(253, 100);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "px";
            // 
            // numVideoHeight
            // 
            this.numVideoHeight.Location = new System.Drawing.Point(179, 119);
            this.numVideoHeight.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numVideoHeight.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numVideoHeight.Name = "numVideoHeight";
            this.numVideoHeight.Size = new System.Drawing.Size(68, 20);
            this.numVideoHeight.TabIndex = 13;
            this.numVideoHeight.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // numVideoWidth
            // 
            this.numVideoWidth.Location = new System.Drawing.Point(179, 93);
            this.numVideoWidth.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numVideoWidth.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numVideoWidth.Name = "numVideoWidth";
            this.numVideoWidth.Size = new System.Drawing.Size(68, 20);
            this.numVideoWidth.TabIndex = 12;
            this.numVideoWidth.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Video Height";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Video Width";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(253, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "sec";
            // 
            // numDurationForEachImage
            // 
            this.numDurationForEachImage.Location = new System.Drawing.Point(179, 67);
            this.numDurationForEachImage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDurationForEachImage.Name = "numDurationForEachImage";
            this.numDurationForEachImage.Size = new System.Drawing.Size(68, 20);
            this.numDurationForEachImage.TabIndex = 8;
            this.numDurationForEachImage.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Duration for a single image";
            // 
            // numVideoPerPost
            // 
            this.numVideoPerPost.Location = new System.Drawing.Point(179, 13);
            this.numVideoPerPost.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numVideoPerPost.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numVideoPerPost.Name = "numVideoPerPost";
            this.numVideoPerPost.Size = new System.Drawing.Size(68, 20);
            this.numVideoPerPost.TabIndex = 6;
            this.numVideoPerPost.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(253, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "posts";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "1 video from each ";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(325, 552);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlYoutube
            // 
            this.pnlYoutube.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlYoutube.Controls.Add(this.txtYoutubeTags);
            this.pnlYoutube.Controls.Add(this.txtYoutubeDescription);
            this.pnlYoutube.Controls.Add(this.txtYoutubeTitle);
            this.pnlYoutube.Controls.Add(this.label15);
            this.pnlYoutube.Controls.Add(this.label14);
            this.pnlYoutube.Controls.Add(this.label13);
            this.pnlYoutube.Enabled = false;
            this.pnlYoutube.Location = new System.Drawing.Point(26, 294);
            this.pnlYoutube.Name = "pnlYoutube";
            this.pnlYoutube.Size = new System.Drawing.Size(374, 140);
            this.pnlYoutube.TabIndex = 9;
            // 
            // txtYoutubeTags
            // 
            this.txtYoutubeTags.Location = new System.Drawing.Point(97, 104);
            this.txtYoutubeTags.Name = "txtYoutubeTags";
            this.txtYoutubeTags.Size = new System.Drawing.Size(263, 20);
            this.txtYoutubeTags.TabIndex = 8;
            // 
            // txtYoutubeDescription
            // 
            this.txtYoutubeDescription.Location = new System.Drawing.Point(97, 43);
            this.txtYoutubeDescription.Multiline = true;
            this.txtYoutubeDescription.Name = "txtYoutubeDescription";
            this.txtYoutubeDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtYoutubeDescription.Size = new System.Drawing.Size(263, 55);
            this.txtYoutubeDescription.TabIndex = 7;
            // 
            // txtYoutubeTitle
            // 
            this.txtYoutubeTitle.Location = new System.Drawing.Point(97, 10);
            this.txtYoutubeTitle.Name = "txtYoutubeTitle";
            this.txtYoutubeTitle.Size = new System.Drawing.Size(263, 20);
            this.txtYoutubeTitle.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(18, 104);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(31, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = "Tags";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 43);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 13);
            this.label14.TabIndex = 4;
            this.label14.Text = "Description";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(19, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(27, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Title";
            // 
            // chkYoutube
            // 
            this.chkYoutube.AutoSize = true;
            this.chkYoutube.Location = new System.Drawing.Point(27, 271);
            this.chkYoutube.Name = "chkYoutube";
            this.chkYoutube.Size = new System.Drawing.Size(113, 17);
            this.chkYoutube.TabIndex = 10;
            this.chkYoutube.Text = "Upload to youtube";
            this.chkYoutube.UseVisualStyleBackColor = true;
            this.chkYoutube.CheckedChanged += new System.EventHandler(this.chkYoutube_CheckedChanged);
            // 
            // txtRefreshToken
            // 
            this.txtRefreshToken.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefreshToken.Location = new System.Drawing.Point(169, 261);
            this.txtRefreshToken.Multiline = true;
            this.txtRefreshToken.Name = "txtRefreshToken";
            this.txtRefreshToken.Size = new System.Drawing.Size(231, 27);
            this.txtRefreshToken.TabIndex = 11;
            this.txtRefreshToken.Text = "1/VucS1d90PKiyPqDQOantnd9kj9tkG1ibJGXGJJr3tWZIgOrJDtdun6zK6XiATCKT";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(26, 452);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(374, 82);
            this.txtStatus.TabIndex = 12;
            this.txtStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStatus_KeyPress);
            // 
            // frmPublish
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(416, 587);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.txtRefreshToken);
            this.Controls.Add(this.chkYoutube);
            this.Controls.Add(this.pnlYoutube);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlSlideShow);
            this.Controls.Add(this.chkCreateSlide);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCriteria);
            this.Controls.Add(this.numNumberOfPosts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPublish);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPublish";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Publish Posts";
            this.Load += new System.EventHandler(this.frmPublish_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numNumberOfPosts)).EndInit();
            this.pnlSlideShow.ResumeLayout(false);
            this.pnlSlideShow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numImagePerPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVideoHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVideoWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDurationForEachImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVideoPerPost)).EndInit();
            this.pnlYoutube.ResumeLayout(false);
            this.pnlYoutube.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numNumberOfPosts;
        private System.Windows.Forms.ComboBox cbCriteria;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkCreateSlide;
        private System.Windows.Forms.Panel pnlSlideShow;
        private System.Windows.Forms.NumericUpDown numVideoPerPost;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numDurationForEachImage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numVideoHeight;
        private System.Windows.Forms.NumericUpDown numVideoWidth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numImagePerPost;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlYoutube;
        private System.Windows.Forms.CheckBox chkYoutube;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtYoutubeTags;
        private System.Windows.Forms.TextBox txtYoutubeDescription;
        private System.Windows.Forms.TextBox txtYoutubeTitle;
        private System.Windows.Forms.TextBox txtRefreshToken;
        private System.Windows.Forms.TextBox txtStatus;
    }
}