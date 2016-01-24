using System;
using System.Windows.Forms;
using PttLib;
using PttLib.Helpers;
using WordpressScraper.Dal;
using WordpressScraper.Ftp;

namespace WordpressScraper
{
    public partial class frmCleanup : Form
    {
        private ProgramOptions _options;
        private Ftp.Ftp _ftp;
        public frmCleanup()
        {
            InitializeComponent();
        }
        public FtpConfig FtpConfiguration
        {
            get
            {
                return new FtpConfig() { Url = _options.FtpUrl, UserName = _options.FtpUser, Password = _options.FtpPassword };
            }
        }


        private void frmCleanup_Load(object sender, EventArgs e)
        {

        }

        private void btnCleanup_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to want to delete ALL POSTS and UPLOADS, including IMAGES from the blog?",
                         "Confirm Delete!!",
                         MessageBoxButtons.YesNo);
            if (confirmResult != DialogResult.Yes) return;


            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();
            if (string.IsNullOrEmpty(_options.FtpUrl))
            {
                MessageBox.Show("In order to delete images, please set up FTP account from settings.");
                return;
            }
            _ftp = new Ftp.Ftp(FtpConfiguration);
            if (!string.IsNullOrEmpty(_ftp.TestConnection()))
            {
                MessageBox.Show("Cannot connect to FTP, please check your settings.");
                return;
            }
            try
            {
                _ftp.DirectoryListingProgressing += FtpDirectoryListingProgressing;
                _ftp.DirectoryListingFetchFinished += FtpDirectoryListingFetchFinished;
                _ftp.DirectoryDeletionProgressing += FtpDirectoryDeletionProgress;
                _ftp.DirectoryDeletionFinished += FtpDirectoryDeletionFinished;
                EnDis(false);
                _ftp.DeleteDirectory("wp-content/uploads/");

            }
            catch (Exception exception)
            {
                EnDis(true);
                
                MessageBox.Show(exception.ToString());
                Logger.LogExceptions(exception);
            }
          

        }

        private void EnDis(bool enabled)
        {
            btnCleanup.Enabled = enabled;
            btnStop.Enabled = !enabled;
        }

        private void SetStatus(string status)
        {
            lblStatus.Text = status;
        }

        private string MySqlConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3}; Allow User Variables=True", _options.DatabaseUrl, _options.DatabaseName, _options.DatabaseUser, _options.DatabasePassword);
            }
        }

        private void FtpDirectoryDeletionFinished(object sender, EventArgs e)
        {
           
          if(btnCleanup.Enabled)//file deletion canceled, don't continue to mysql part before cleaning up files
          {
              return;
          }
          try
          {
              EnDis(false);

              using (var dal = new Dal.Dal(MySqlConnectionString))
              {
                  var postDal = new PostDal(dal);
                  postDal.DeleteAll();
              }

          }
          catch (Exception exception)
          {
               EnDis(true);

              MessageBox.Show(exception.ToString());
              Logger.LogExceptions(exception);
              return;
          }

          EnDis(true);
          MessageBox.Show("Finished!");
        }

        private void FtpDirectoryDeletionProgress(object sender, string e)
        {
            SetStatus(e);
        }

        private void FtpDirectoryListingFetchFinished(object sender, EventArgs e)
        {
        }

        private void FtpDirectoryListingProgressing(object sender, string e)
        {
            SetStatus(e);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_ftp == null) return;
            _ftp.CancelAsync();
            EnDis(true);
        }


    }
}
