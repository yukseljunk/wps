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
            this.grpFtp = new System.Windows.Forms.GroupBox();
            this.btnTestFtpConnection = new System.Windows.Forms.Button();
            this.txtFtpPassword = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFtpUserName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtFtpUrl = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
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
            this.grpBlogProp = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBlogUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoadSettings = new System.Windows.Forms.Button();
            this.grpFtp.SuspendLayout();
            this.grpMysql.SuspendLayout();
            this.grpBlogProp.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpFtp
            // 
            this.grpFtp.Controls.Add(this.btnTestFtpConnection);
            this.grpFtp.Controls.Add(this.txtFtpPassword);
            this.grpFtp.Controls.Add(this.label9);
            this.grpFtp.Controls.Add(this.txtFtpUserName);
            this.grpFtp.Controls.Add(this.label10);
            this.grpFtp.Controls.Add(this.txtFtpUrl);
            this.grpFtp.Controls.Add(this.label11);
            this.grpFtp.Location = new System.Drawing.Point(26, 424);
            this.grpFtp.Name = "grpFtp";
            this.grpFtp.Size = new System.Drawing.Size(285, 157);
            this.grpFtp.TabIndex = 27;
            this.grpFtp.TabStop = false;
            this.grpFtp.Text = "FTP to blog root folder";
            // 
            // btnTestFtpConnection
            // 
            this.btnTestFtpConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestFtpConnection.Location = new System.Drawing.Point(116, 117);
            this.btnTestFtpConnection.Name = "btnTestFtpConnection";
            this.btnTestFtpConnection.Size = new System.Drawing.Size(141, 23);
            this.btnTestFtpConnection.TabIndex = 13;
            this.btnTestFtpConnection.Text = "Test FTP connection";
            this.btnTestFtpConnection.UseVisualStyleBackColor = true;
            this.btnTestFtpConnection.Click += new System.EventHandler(this.btnTestFtpConnection_Click);
            // 
            // txtFtpPassword
            // 
            this.txtFtpPassword.Location = new System.Drawing.Point(75, 85);
            this.txtFtpPassword.Name = "txtFtpPassword";
            this.txtFtpPassword.PasswordChar = '*';
            this.txtFtpPassword.Size = new System.Drawing.Size(182, 20);
            this.txtFtpPassword.TabIndex = 6;
            this.txtFtpPassword.Text = "x6b8a[o!bxFe";
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
            this.txtFtpUserName.Text = "bloggon2@nalgorithm.com";
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
            // grpMysql
            // 
            this.grpMysql.Controls.Add(this.btnTestMySqlConnection);
            this.grpMysql.Controls.Add(this.txtMySqlDatabase);
            this.grpMysql.Controls.Add(this.lblDatabase);
            this.grpMysql.Controls.Add(this.txtMySqlPass);
            this.grpMysql.Controls.Add(this.label6);
            this.grpMysql.Controls.Add(this.txtMysqlUser);
            this.grpMysql.Controls.Add(this.label7);
            this.grpMysql.Controls.Add(this.txtMySqlIp);
            this.grpMysql.Controls.Add(this.label8);
            this.grpMysql.Location = new System.Drawing.Point(26, 194);
            this.grpMysql.Name = "grpMysql";
            this.grpMysql.Size = new System.Drawing.Size(285, 212);
            this.grpMysql.TabIndex = 26;
            this.grpMysql.TabStop = false;
            this.grpMysql.Text = "Target Blog MySql Settings";
            this.grpMysql.Enter += new System.EventHandler(this.grpMysql_Enter);
            // 
            // btnTestMySqlConnection
            // 
            this.btnTestMySqlConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestMySqlConnection.Location = new System.Drawing.Point(116, 167);
            this.btnTestMySqlConnection.Name = "btnTestMySqlConnection";
            this.btnTestMySqlConnection.Size = new System.Drawing.Size(141, 23);
            this.btnTestMySqlConnection.TabIndex = 12;
            this.btnTestMySqlConnection.Text = "Test mysql connection";
            this.btnTestMySqlConnection.UseVisualStyleBackColor = true;
            this.btnTestMySqlConnection.Click += new System.EventHandler(this.btnTestMySqlConnection_Click);
            // 
            // txtMySqlDatabase
            // 
            this.txtMySqlDatabase.Location = new System.Drawing.Point(75, 89);
            this.txtMySqlDatabase.Name = "txtMySqlDatabase";
            this.txtMySqlDatabase.Size = new System.Drawing.Size(182, 20);
            this.txtMySqlDatabase.TabIndex = 8;
            this.txtMySqlDatabase.Text = "nalgor5_wpgonbl";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(12, 92);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(53, 13);
            this.lblDatabase.TabIndex = 7;
            this.lblDatabase.Text = "Database";
            // 
            // txtMySqlPass
            // 
            this.txtMySqlPass.Location = new System.Drawing.Point(75, 126);
            this.txtMySqlPass.Name = "txtMySqlPass";
            this.txtMySqlPass.PasswordChar = '*';
            this.txtMySqlPass.Size = new System.Drawing.Size(182, 20);
            this.txtMySqlPass.TabIndex = 6;
            this.txtMySqlPass.Text = "S]P-a588C3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Password";
            // 
            // txtMysqlUser
            // 
            this.txtMysqlUser.Location = new System.Drawing.Point(75, 54);
            this.txtMysqlUser.Name = "txtMysqlUser";
            this.txtMysqlUser.Size = new System.Drawing.Size(182, 20);
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
            // grpBlogProp
            // 
            this.grpBlogProp.Controls.Add(this.txtPassword);
            this.grpBlogProp.Controls.Add(this.label5);
            this.grpBlogProp.Controls.Add(this.txtUserName);
            this.grpBlogProp.Controls.Add(this.label4);
            this.grpBlogProp.Controls.Add(this.txtBlogUrl);
            this.grpBlogProp.Controls.Add(this.label3);
            this.grpBlogProp.Location = new System.Drawing.Point(26, 48);
            this.grpBlogProp.Name = "grpBlogProp";
            this.grpBlogProp.Size = new System.Drawing.Size(285, 128);
            this.grpBlogProp.TabIndex = 25;
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
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(146, 608);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 58;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(236, 608);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 57;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(26, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(119, 23);
            this.btnSave.TabIndex = 60;
            this.btnSave.Text = "Save Settings...";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.Location = new System.Drawing.Point(179, 12);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.Size = new System.Drawing.Size(132, 23);
            this.btnLoadSettings.TabIndex = 59;
            this.btnLoadSettings.Text = "Load Settings...";
            this.btnLoadSettings.UseVisualStyleBackColor = true;
            this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(323, 649);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoadSettings);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpFtp);
            this.Controls.Add(this.grpMysql);
            this.Controls.Add(this.grpBlogProp);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.grpFtp.ResumeLayout(false);
            this.grpFtp.PerformLayout();
            this.grpMysql.ResumeLayout(false);
            this.grpMysql.PerformLayout();
            this.grpBlogProp.ResumeLayout(false);
            this.grpBlogProp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFtp;
        private System.Windows.Forms.Button btnTestFtpConnection;
        private System.Windows.Forms.TextBox txtFtpPassword;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFtpUserName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtFtpUrl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox grpMysql;
        private System.Windows.Forms.Button btnTestMySqlConnection;
        private System.Windows.Forms.TextBox txtMySqlDatabase;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.TextBox txtMySqlPass;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMysqlUser;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMySqlIp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox grpBlogProp;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBlogUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoadSettings;
    }
}