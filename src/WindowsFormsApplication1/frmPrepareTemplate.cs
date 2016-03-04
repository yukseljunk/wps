using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;
using WordpressScraper.Ftp;
using WpsLib.ProgramOptions;

namespace WordpressScraper
{
    public partial class frmPrepareTemplate : Form
    {
        private BackgroundWorker bw = new BackgroundWorker();
        private ProgramOptions _options;
        public frmPrepareTemplate()
        {
            InitializeComponent();
        }

        private void frmPrepareTemplate_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;

        }

        public FtpConfig FtpConfiguration
        {
            get
            {
                return new FtpConfig() { Url = _options.FtpUrl, UserName = _options.FtpUser, Password = _options.FtpPassword };
            }
        }
        private IEnumerable<string> _files= new List<string>(); 
        private void StartWorker()
        {
            timer1.Enabled = false;

            _files = Directory.EnumerateFiles("blog", "*.*", SearchOption.AllDirectories);

            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled))
            {
                lblStatus.Text = "Canceled!";
            }

            else if (e.Error != null)
            {
                lblStatus.Text = ("Error: " + e.Error.Message);
            }

            else
            {
                lblStatus.Text = "Finished!";
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblStatus.Text = (e.UserState + " ");
            progressBar1.Maximum = _files.Count();
            progressBar1.Value = e.ProgressPercentage;
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();
            var ftp = new Ftp.Ftp(FtpConfiguration);
            
            var fileUploaded = 0;
            
            foreach (var file in _files)
            {
                fileUploaded++;
                if ((bw.CancellationPending))
                {
                    e.Cancel = true;
                    break;
                }
                var fileInfo = new FileInfo(file);
                var dir = fileInfo.Directory;
                if (dir == null)
                {
                    continue;
                }
                var ftpDir = dir.FullName.Replace(Helper.AssemblyDirectory + "\\blog", "").Replace("\\", "/");
                if (ftpDir.StartsWith("/"))
                {
                    ftpDir = ftpDir.Substring(1);
                }
                try
                {
                    
                    ftp.MakeFtpDir(ftpDir);
                    bw.ReportProgress(fileUploaded, "Creating Ftp Directory " + ftpDir);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                    return;
                }

                try
                {
                    
                    ftp.UploadFileFtp(file, ftpDir);
                    bw.ReportProgress(fileUploaded, "Uploading " + file);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StartWorker();
        }

    }
}
