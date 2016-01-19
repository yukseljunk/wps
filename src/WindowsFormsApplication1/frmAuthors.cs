using System;
using System.Windows.Forms;
using PttLib;
using WordpressScraper.Dal;

namespace WordpressScraper
{
    public partial class frmAuthors : Form
    {
        private ProgramOptions _options;

        public frmAuthors()
        {
            InitializeComponent();
        }

        private void frmAuthors_Load(object sender, EventArgs e)
        {
            var optionsFactory = new ProgramOptionsFactory();
            _options = optionsFactory.Get();
        }

        private string MySqlConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3}; Allow User Variables=True", _options.DatabaseUrl, _options.DatabaseName, _options.DatabaseUser, _options.DatabasePassword);
            }
        }


        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuthors.Text))
            {
                return;
            }
            var authors = txtAuthors.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            using (var dal = new Dal.Dal(MySqlConnectionString))
            {
                var userDal = new UserDal(dal);
                foreach (var author in authors)
                {
                    if (string.IsNullOrEmpty(author.Trim()))
                    {
                        continue;
                    }
                    userDal.InsertUser(author.Trim(),_options.BlogHost);
                }
            }
            MessageBox.Show("Done");

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch
            {
            }

        }
    }
}
