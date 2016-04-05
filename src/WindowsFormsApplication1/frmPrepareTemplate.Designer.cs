namespace WordpressScraper
{
    partial class frmPrepareTemplate
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
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.chkUploadFiles = new System.Windows.Forms.CheckBox();
            this.chkActivateSetupPlugins = new System.Windows.Forms.CheckBox();
            this.lstPlugins = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(12, 297);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(37, 13);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "Status";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(17, 363);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 330);
            this.progressBar1.Maximum = 10000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(347, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(15, 262);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chkUploadFiles
            // 
            this.chkUploadFiles.AutoSize = true;
            this.chkUploadFiles.Checked = true;
            this.chkUploadFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUploadFiles.Location = new System.Drawing.Point(15, 26);
            this.chkUploadFiles.Name = "chkUploadFiles";
            this.chkUploadFiles.Size = new System.Drawing.Size(184, 17);
            this.chkUploadFiles.TabIndex = 6;
            this.chkUploadFiles.Text = "Upload Template and Plugin Files";
            this.chkUploadFiles.UseVisualStyleBackColor = true;
            // 
            // chkActivateSetupPlugins
            // 
            this.chkActivateSetupPlugins.AutoSize = true;
            this.chkActivateSetupPlugins.Checked = true;
            this.chkActivateSetupPlugins.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActivateSetupPlugins.Location = new System.Drawing.Point(15, 61);
            this.chkActivateSetupPlugins.Name = "chkActivateSetupPlugins";
            this.chkActivateSetupPlugins.Size = new System.Drawing.Size(154, 17);
            this.chkActivateSetupPlugins.TabIndex = 8;
            this.chkActivateSetupPlugins.Text = "Activate and Setup Plugins";
            this.chkActivateSetupPlugins.UseVisualStyleBackColor = true;
            // 
            // lstPlugins
            // 
            this.lstPlugins.CheckOnClick = true;
            this.lstPlugins.ColumnWidth = 250;
            this.lstPlugins.FormattingEnabled = true;
            this.lstPlugins.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.lstPlugins.Location = new System.Drawing.Point(15, 93);
            this.lstPlugins.MultiColumn = true;
            this.lstPlugins.Name = "lstPlugins";
            this.lstPlugins.Size = new System.Drawing.Size(742, 154);
            this.lstPlugins.TabIndex = 9;
            // 
            // frmPrepareTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 399);
            this.Controls.Add(this.lstPlugins);
            this.Controls.Add(this.chkActivateSetupPlugins);
            this.Controls.Add(this.chkUploadFiles);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblDesc);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPrepareTemplate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Prepare Template";
            this.Load += new System.EventHandler(this.frmPrepareTemplate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox chkUploadFiles;
        private System.Windows.Forms.CheckBox chkActivateSetupPlugins;
        private System.Windows.Forms.CheckedListBox lstPlugins;
    }
}