using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PttLib.Helpers;
using WordpressScraper.Ftp;
using WordpressScraper.Helpers;
using WpsLib.Dal;
using WpsLib.ProgramOptions;
using Helper = WindowsFormsApplication1.Helper;

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
            //timer1.Enabled = true;
            lstPlugins.Items.Clear();
            var pluginFiles = GetPluginNames();
            foreach (var pluginFile in pluginFiles)
            {
                lstPlugins.Items.Add(pluginFile);
                lstPlugins.SetItemChecked(lstPlugins.Items.Count - 1, true);
            }

#if DEBUG
            btnPluginData.Visible = true;

#endif

        }

        public FtpConfig FtpConfiguration
        {
            get
            {
                return new FtpConfig() { Url = _options.FtpUrl, UserName = _options.FtpUser, Password = _options.FtpPassword };
            }
        }
        private string MySqlConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3}; Allow User Variables=True", _options.DatabaseUrl, _options.DatabaseName, _options.DatabaseUser, _options.DatabasePassword);
            }
        }
        private IEnumerable<string> _files = new List<string>();
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
            btnStart.Enabled = true;
            lstPlugins.Enabled = true;
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblStatus.Text = (e.UserState + " ");
            progressBar1.Maximum = _files.Count();
            progressBar1.Value = e.ProgressPercentage;
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var checkedSites = (from object checkedItem in lstPlugins.CheckedItems select checkedItem.ToString()).ToList();
            var directoriesCreated = new HashSet<string>();
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
                    if (!directoriesCreated.Contains(ftpDir))
                    {
                        ftp.MakeFtpDir(ftpDir);
                        bw.ReportProgress(fileUploaded, "Creating Ftp Directory " + ftpDir);
                        directoriesCreated.Add(ftpDir);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                    return;
                }

                try
                {
                    if (!checkedSites.Contains("ewww-image-optimizer") && fileInfo.Directory.Name == "ewww")
                    {
                        continue;
                    }

                    if (fileInfo.Directory.Name == "plugins")
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file);
                        if (!checkedSites.Contains(fileName))
                        {
                            continue;
                        }
                    }
                    ftp.UploadFileFtp(file, ftpDir);
                    bw.ReportProgress(fileUploaded, "Uploading " + file);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
            try
            {
                UnzipPlugins(fileUploaded);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                return;
            }

            try
            {
                SetPluginData();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                return;
            }

        }

        private void UnzipPlugins(int fileUploaded)
        {
            //make a request to unzip files
            var blogUrl = _options.BlogUrl;
            if (!_options.BlogUrl.EndsWith("/"))
            {
                blogUrl += "/";
            }
            var checkedSites = (from object checkedItem in lstPlugins.CheckedItems select checkedItem.ToString()).ToList();

            var zipFiles = Directory.EnumerateFiles("blog", "*.zip", SearchOption.AllDirectories);
            foreach (var zipFile in zipFiles)
            {
                var zipFileInfo = new FileInfo(zipFile);

                if (zipFileInfo.Directory.Name == "plugins")
                {
                    var fileName = Path.GetFileNameWithoutExtension(zipFile);
                    if (!checkedSites.Contains(fileName))
                    {
                        continue;
                    }
                }

                var url = string.Format("{1}wp-unzip.php?file=wp-content/plugins/{0}", zipFileInfo.Name, blogUrl);
                var result = WebHelper.CurlSimple(url);
                if (string.IsNullOrEmpty(result))
                {
                    MessageBox.Show(string.Format("Error unpacking {0}", zipFileInfo.Name));
                    return;
                }
                if (result.StartsWith("OK"))
                {
                    bw.ReportProgress(fileUploaded, string.Format(" Finished unpacking {0}", zipFileInfo.Name));
                }
                else
                {
                    MessageBox.Show(string.Format("Error unpacking {0}", zipFileInfo.Name));
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StartWorker();
        }

        private void SetPluginData()
        {
            var checkedSites = (from object checkedItem in lstPlugins.CheckedItems select checkedItem.ToString()).ToList();
            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();

            using (var dal = new Dal(MySqlConnectionString))
            {
                var optionsDal = new OptionsDal(dal);
                var currentActivePluginsValue = optionsDal.GetValue("active_plugins");
                var currentActivePlugins = new HashSet<string>(PhpSerializer.Deserialize(currentActivePluginsValue));
                var pluginsToPut = new HashSet<string>();

                var pluginPaths = GetPluginFiles();
                var pluginNames = GetPluginNames();
                for (int i = 0; i < pluginNames.Count; i++)
                {
                    if (checkedSites.Contains(pluginNames[i]))
                    {
                        pluginsToPut.Add(pluginPaths[i]);
                    }
                }

                currentActivePlugins.UnionWith(pluginsToPut);
                var newActivePluginData = PhpSerializer.Serialize(currentActivePlugins.ToList());
                optionsDal.SetValue("active_plugins", newActivePluginData);

                var optionFiles = Directory.EnumerateFiles("blog", "*.bosf", SearchOption.AllDirectories);
                foreach (var optionFile in optionFiles)
                {
                    var optionFileInfo = new FileInfo(optionFile);

                    if (optionFileInfo.Directory.Name == "plugins")
                    {
                        var fileName = Path.GetFileNameWithoutExtension(optionFile);
                        if (!checkedSites.Contains(fileName))
                        {
                            continue;
                        }
                    }

                    var contentLines = File.ReadAllLines(optionFileInfo.FullName);
                    foreach (var content in contentLines)
                    {
                        var splitted = content.Split('\t');
                        if (splitted.Length > 0)
                        {
                            optionsDal.SetValue(splitted[0], splitted[1]);
                        }
                        else
                        {
                            optionsDal.SetValue(content, "");
                        }
                    }
                }

            }

        }
        private static IList<string> GetPluginFiles()
        {
            var plugins = new List<string>();
            var pluginListFile = "blog/wp-content/plugins/plugins.txt";
            var pluginListFileContent = File.ReadAllLines(pluginListFile);
            foreach (var pluginPath in pluginListFileContent)
            {
                plugins.Add(pluginPath);
            }
            return plugins;
        }

        private static IList<string> GetPluginNames()
        {
            var result = new List<string>();
            var pluginFiles = GetPluginFiles();
            foreach (var pluginFile in pluginFiles)
            {
                result.Add(pluginFile.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]);
            }
            return result;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            lstPlugins.Enabled = false;
            StartWorker();
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            CheckUncheckAll(true);
        }

        private void CheckUncheckAll(bool state)
        {
            for (int i = 0; i < lstPlugins.Items.Count; i++)
            {
                lstPlugins.SetItemChecked(i, state);
            }
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            CheckUncheckAll(false);
        }
    }

}
