using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;
using WordpressScraper.Helpers;

namespace WordpressScraper
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var settings = new List<Tuple<string, string>>();

            settings.Add(new Tuple<string, string>("BlogUrl", txtBlogUrl.Text));
            settings.Add(new Tuple<string, string>("BlogUser", txtUserName.Text));
            settings.Add(new Tuple<string, string>("BlogPassword", txtPassword.Text));
            settings.Add(new Tuple<string, string>("DatabaseUrl", txtMySqlIp.Text));
            settings.Add(new Tuple<string, string>("DatabaseName", txtMySqlDatabase.Text));
            settings.Add(new Tuple<string, string>("DatabaseUser", txtMysqlUser.Text));
            settings.Add(new Tuple<string, string>("DatabasePassword", txtMySqlPass.Text));
            settings.Add(new Tuple<string, string>("FtpUrl", txtFtpUrl.Text));
            settings.Add(new Tuple<string, string>("FtpUser", txtFtpUserName.Text));
            settings.Add(new Tuple<string, string>("FtpPassword", txtFtpPassword.Text));
            
            ConfigurationHelper.UpdateSettings(settings);

            this.Dispose();
            this.Close();

        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            var programOptionsFactory = new ProgramOptionsFactory();
            var options = programOptionsFactory.Get();

            txtBlogUrl.Text = options.BlogUrl;
            txtUserName.Text = options.BlogUser;
            txtPassword.Text = options.BlogPassword;
            txtMysqlUser.Text = options.DatabaseUser;
            txtMySqlIp.Text = options.DatabaseUrl;
            txtMySqlDatabase.Text = options.DatabaseName;
            txtMySqlPass.Text = options.DatabasePassword;
            txtFtpUrl.Text = options.FtpUrl;
            txtFtpUserName.Text = options.FtpUser;
            txtFtpPassword.Text = options.FtpPassword;

        }

        private string MySqlConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3}; Allow User Variables=True", txtMySqlIp.Text, txtMySqlDatabase.Text, txtMysqlUser.Text, txtMySqlPass.Text);

            }
        }


        private void grpMysql_Enter(object sender, EventArgs e)
        {

        }

        private void btnTestMySqlConnection_Click(object sender, EventArgs e)
        {
            using (var dal = new Dal.Dal(MySqlConnectionString))
            {
                var testConnection = dal.TestConnection();
                if (testConnection != null)
                {
                    foreach (var result in testConnection)
                    {
                        MessageBox.Show(result);
                    }
                }
            }
        }
        public FtpConfig FtpConfiguration
        {
            get
            {
                return new FtpConfig() { Url = txtFtpUrl.Text, UserName = txtFtpUserName.Text, Password = txtFtpPassword.Text };
            }
        }



        private void btnTestFtpConnection_Click(object sender, EventArgs e)
        {
            var ftp = new Ftp();
            string result = ftp.TestConnection(FtpConfiguration);
            if (string.IsNullOrEmpty(result))
            {
                MessageBox.Show("Successfull!");
                return;
            }
            MessageBox.Show("Failed: " + result);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not ready yet");
        }

        private void btnLoadSettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not ready yet");

        }
    }
}
