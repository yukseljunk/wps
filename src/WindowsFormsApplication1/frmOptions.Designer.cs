namespace WordpressScraper
{
    partial class frmOptions
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkCache = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.numMerge = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.numThumbnailSize = new System.Windows.Forms.NumericUpDown();
            this.numMaxImageDimension = new System.Windows.Forms.NumericUpDown();
            this.lblResizePix = new System.Windows.Forms.Label();
            this.chkResizeImages = new System.Windows.Forms.CheckBox();
            this.chkNoAPI = new System.Windows.Forms.CheckBox();
            this.chkFeatureImage = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.chkShowMessageBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMerge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThumbnailSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxImageDimension)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkShowMessageBox);
            this.panel1.Controls.Add(this.chkCache);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.numMerge);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.numThumbnailSize);
            this.panel1.Controls.Add(this.numMaxImageDimension);
            this.panel1.Controls.Add(this.lblResizePix);
            this.panel1.Controls.Add(this.chkResizeImages);
            this.panel1.Controls.Add(this.chkNoAPI);
            this.panel1.Controls.Add(this.chkFeatureImage);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(354, 282);
            this.panel1.TabIndex = 54;
            // 
            // chkCache
            // 
            this.chkCache.AutoSize = true;
            this.chkCache.Checked = true;
            this.chkCache.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCache.Enabled = false;
            this.chkCache.Location = new System.Drawing.Point(27, 250);
            this.chkCache.Name = "chkCache";
            this.chkCache.Size = new System.Drawing.Size(87, 17);
            this.chkCache.TabIndex = 65;
            this.chkCache.Text = "Use Caching";
            this.chkCache.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(193, 28);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 13);
            this.label15.TabIndex = 64;
            this.label15.Text = "words";
            // 
            // numMerge
            // 
            this.numMerge.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numMerge.Location = new System.Drawing.Point(132, 23);
            this.numMerge.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMerge.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numMerge.Name = "numMerge";
            this.numMerge.Size = new System.Drawing.Size(55, 20);
            this.numMerge.TabIndex = 63;
            this.numMerge.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(24, 26);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(106, 13);
            this.label14.TabIndex = 62;
            this.label14.Text = "Merge Post less than";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(26, 62);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 61;
            this.label13.Text = "Thumbnail Size";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(169, 65);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(18, 13);
            this.label12.TabIndex = 60;
            this.label12.Text = "px";
            // 
            // numThumbnailSize
            // 
            this.numThumbnailSize.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numThumbnailSize.Location = new System.Drawing.Point(111, 60);
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
            this.numThumbnailSize.TabIndex = 59;
            this.numThumbnailSize.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // numMaxImageDimension
            // 
            this.numMaxImageDimension.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numMaxImageDimension.Location = new System.Drawing.Point(186, 99);
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
            this.numMaxImageDimension.TabIndex = 58;
            this.numMaxImageDimension.Value = new decimal(new int[] {
            750,
            0,
            0,
            0});
            // 
            // lblResizePix
            // 
            this.lblResizePix.AutoSize = true;
            this.lblResizePix.Location = new System.Drawing.Point(247, 101);
            this.lblResizePix.Name = "lblResizePix";
            this.lblResizePix.Size = new System.Drawing.Size(18, 13);
            this.lblResizePix.TabIndex = 57;
            this.lblResizePix.Text = "px";
            // 
            // chkResizeImages
            // 
            this.chkResizeImages.AutoSize = true;
            this.chkResizeImages.Checked = true;
            this.chkResizeImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkResizeImages.Location = new System.Drawing.Point(27, 99);
            this.chkResizeImages.Name = "chkResizeImages";
            this.chkResizeImages.Size = new System.Drawing.Size(153, 17);
            this.chkResizeImages.TabIndex = 56;
            this.chkResizeImages.Text = "Resize images bigger than ";
            this.chkResizeImages.UseVisualStyleBackColor = true;
            // 
            // chkNoAPI
            // 
            this.chkNoAPI.AutoSize = true;
            this.chkNoAPI.Checked = true;
            this.chkNoAPI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNoAPI.Location = new System.Drawing.Point(27, 145);
            this.chkNoAPI.Name = "chkNoAPI";
            this.chkNoAPI.Size = new System.Drawing.Size(181, 17);
            this.chkNoAPI.TabIndex = 55;
            this.chkNoAPI.Text = "Don\'t use Api but Mysql and FTP";
            this.chkNoAPI.UseVisualStyleBackColor = true;
            // 
            // chkFeatureImage
            // 
            this.chkFeatureImage.AutoSize = true;
            this.chkFeatureImage.Location = new System.Drawing.Point(27, 184);
            this.chkFeatureImage.Name = "chkFeatureImage";
            this.chkFeatureImage.Size = new System.Drawing.Size(192, 17);
            this.chkFeatureImage.TabIndex = 54;
            this.chkFeatureImage.Text = "Make First Image as Feature Image";
            this.chkFeatureImage.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(291, 312);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 55;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(201, 312);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 56;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chkShowMessageBox
            // 
            this.chkShowMessageBox.AutoSize = true;
            this.chkShowMessageBox.Checked = true;
            this.chkShowMessageBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowMessageBox.Location = new System.Drawing.Point(27, 217);
            this.chkShowMessageBox.Name = "chkShowMessageBox";
            this.chkShowMessageBox.Size = new System.Drawing.Size(197, 17);
            this.chkShowMessageBox.TabIndex = 66;
            this.chkShowMessageBox.Text = "Show message boxes when needed";
            this.chkShowMessageBox.UseVisualStyleBackColor = true;
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(380, 347);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMerge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThumbnailSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxImageDimension)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkCache;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numMerge;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numThumbnailSize;
        private System.Windows.Forms.NumericUpDown numMaxImageDimension;
        private System.Windows.Forms.Label lblResizePix;
        private System.Windows.Forms.CheckBox chkResizeImages;
        private System.Windows.Forms.CheckBox chkNoAPI;
        private System.Windows.Forms.CheckBox chkFeatureImage;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkShowMessageBox;

    }
}