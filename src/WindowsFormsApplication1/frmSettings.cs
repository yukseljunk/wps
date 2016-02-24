using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PttLib;
using PttLib.Helpers;
using WordpressScraper.Ftp;
using WordpressScraper.Helpers;
using WpsLib.Dal;
using WpsLib.ProgramOptions;

namespace WordpressScraper
{
    public partial class frmSettings : Form
    {
        private List<Panel> _panels = new List<Panel>() { };
        private Dictionary<string, ProgramOptions> _sitesSettings = new Dictionary<string, ProgramOptions>();

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
            BlogsSettings.SaveSites(_sitesSettings);
            this.Dispose();
            this.Close();

        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            _panels.Clear();
            _panels.Add(pnlBlog);
            _panels.Add(pnlMysql);
            _panels.Add(pnlFtp);
            _panels.Add(pnlYoutube);
            _panels.Add(pnlProxy);
            var maxHeight = 0;
            foreach (var panel in _panels)
            {
                panel.Location = new Point(12, 12);
                if (maxHeight < panel.Height)
                {
                    maxHeight = panel.Height;
                }
            }
            this.Height = maxHeight + 250;
            this.Width = 850;
#if DEBUG
            //txtYoutubeClientSecret.Text = "pobSaKuo_5-xDDryquzDYKjS";
#endif

            BlogsSettings.CheckCreateSitesListFile();
            FillSites();
            FillValues(new ProgramOptions());
            AddOnChangeHandlerToInputControls(tabMain);

            ArrangeTabVisibility();

        }

        private void ArrangeTabVisibility()
        {
            tabMain.Visible = _sitesSettings.Count > 0 && lvSites.SelectedItems.Count>0;
        }

        private void FillSites()
        {
            lvSites.Items.Clear();
            foreach (var site in BlogsSettings.Sites)
            {
                lvSites.Items.Add(site);
                _sitesSettings.Add(site, BlogsSettings.ProgramOptionsForBlog(site));
            }
        }

        private void FillValues(ProgramOptions options)
        {
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
            txtProxyIp.Text = options.ProxyAddress;
            FixEmptyNumericUpDown(numProxyPort);
            numProxyPort.Value = options.ProxyPort;
            chkUseProxy.Checked = options.UseProxy;

            txtYoutubeClientId.Text = options.YoutubeClient;
            txtYoutubeClientSecret.Text = options.YoutubeClientSecret;
        }

        private void FixEmptyNumericUpDown(NumericUpDown control)
        {
            if (control.Text == "")
            {
                control.Text = "0";
            }
        }

        private string MySqlConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3}; Allow User Variables=True", txtMySqlIp.Text, txtMySqlDatabase.Text, txtMysqlUser.Text, txtMySqlPass.Text);

            }
        }


        private void btnTestMySqlConnection_Click(object sender, EventArgs e)
        {
            using (var dal = new Dal(MySqlConnectionString))
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
            var ftp = new Ftp.Ftp(FtpConfiguration);
            string result = ftp.TestConnection();
            if (string.IsNullOrEmpty(result))
            {
                MessageBox.Show("Successfull!");
                return;
            }
            MessageBox.Show("Failed: " + result);
        }


        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSettings.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveSettings.FilterIndex = 1;
            saveSettings.RestoreDirectory = true;
            saveSettings.FileName = "";

            if (saveSettings.ShowDialog() != DialogResult.OK) return;
            if (string.IsNullOrEmpty(saveSettings.FileName)) return;

            var options = GetOptionsFromControls();

            var xmlSerializer = new XmlSerializer();
            xmlSerializer.Serialize(saveSettings.FileName, options);
        }

        private ProgramOptions GetOptionsFromControls()
        {
            var options = new ProgramOptions()
            {
                BlogUrl = txtBlogUrl.Text,
                BlogUser = txtUserName.Text,
                BlogPassword = txtPassword.Text,
                DatabaseUrl = txtMySqlIp.Text,
                DatabaseName = txtMySqlDatabase.Text,
                DatabaseUser = txtMysqlUser.Text,
                DatabasePassword = txtMySqlPass.Text,
                FtpUrl = txtFtpUrl.Text,
                FtpUser = txtFtpUserName.Text,
                FtpPassword = txtFtpPassword.Text,
                ProxyAddress = txtProxyIp.Text,
                ProxyPort = (int) numProxyPort.Value,
                UseProxy = chkUseProxy.Checked,
                YoutubeClient = txtYoutubeClientId.Text,
                YoutubeClientSecret = txtYoutubeClientSecret.Text
            };
            return options;
        }

        private void loadSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvSites.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a site from sites list.");
                return;
            }

            openSettingFile.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
            openSettingFile.FilterIndex = 1;
            openSettingFile.RestoreDirectory = true;
            openSettingFile.FileName = "";

            if (openSettingFile.ShowDialog() != DialogResult.OK) return;
            if (string.IsNullOrEmpty(openSettingFile.FileName)) return;

            var programOptionsFactory = new ProgramOptionsFactory();
            var options = programOptionsFactory.Get(openSettingFile.FileName);

            _sitesSettings[lvSites.SelectedItems[0].Text] = options;

            FillValues(options);
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnTestFtpConnection_Click_1(object sender, EventArgs e)
        {

        }

        private string listviewOperation = "";
        private void btnNew_Click(object sender, EventArgs e)
        {
            ListViewItem item = lvSites.Items.Add("New site " + DateTime.Now.Ticks);
            listviewOperation = "ADD";
            // Place the newly-added item into edit mode immediately
            item.BeginEdit();
        }

        private void lvSites_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            //rename or add new
            string newName = e.Label;
            if (!string.IsNullOrEmpty(newName))
            {
                newName = newName.Trim();
                var invalidChars = Path.GetInvalidFileNameChars().ToList();
                foreach (var invalidChar in invalidChars)
                {
                    newName = newName.Replace(invalidChar.ToString(), "");
                }
            }
            var currentName = lvSites.Items[e.Item].Text.Trim();
            if (listviewOperation == "ADD" || listviewOperation.StartsWith("DUPLICATE"))
            {
                if (string.IsNullOrEmpty(newName))
                {
                    newName = currentName;
                }
                if (string.IsNullOrEmpty(newName))
                {
                    MessageBox.Show("Cannot be blank!");
                    e.CancelEdit = true;
                    return;
                }

                if (BlogsSettings.Sites.Contains(newName))
                {
                    MessageBox.Show("There is a site with that name!");
                    e.CancelEdit = true;
                    lvSites.Items[e.Item].BeginEdit();
                    return;
                }

                using (StreamWriter sw = File.AppendText(BlogsSettings.SitesListFile))
                {
                    sw.WriteLine(newName);
                }
                if (listviewOperation == "ADD")
                {
                    _sitesSettings.Add(newName, new ProgramOptions());
                }
                else
                {
                    var listIndex=listviewOperation.Substring("DUPLICATE".Length);
                    if (!string.IsNullOrEmpty(listIndex))
                    {
                        _sitesSettings.Add(newName, _sitesSettings[listIndex]);
                    }
                }
                listviewOperation = "";
                ArrangeTabVisibility();

                lvSites.Items[e.Item].Text = newName;
                lvSites_SelectedIndexChanged(null, null);
                return;
            }

            if (string.IsNullOrEmpty(newName.Trim()))
            {
                MessageBox.Show("Cannot be blank!");
                e.CancelEdit = true;
                return;
            }
            if (newName == currentName) return;
            if (BlogsSettings.Sites.Contains(newName))
            {
                MessageBox.Show("There is a site with that name!");
                e.CancelEdit = true;
                return;
            }
            lvSites.Items[e.Item].Text = newName;
            _sitesSettings.Add(newName, _sitesSettings[currentName]);
            _sitesSettings.Remove(currentName);
            var sites = BlogsSettings.Sites.ToList();
            sites[e.Item] = newName;
            BlogsSettings.UpdateSitesFile(sites);
            lvSites_SelectedIndexChanged(null, null);
        }


        private void lvSites_SelectedIndexChanged(object sender, EventArgs e)
        {
           if (lvSites.SelectedItems.Count == 0) return;
            ArrangeTabVisibility();
           FillValues(_sitesSettings[lvSites.SelectedItems[0].Text]);

        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (lvSites.SelectedItems.Count == 0) return;

            var item = lvSites.SelectedItems[0];
            item.BeginEdit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvSites.SelectedItems.Count == 0) return;
            var item = lvSites.SelectedItems[0];
            if (MessageBox.Show("Are you sure to delete this item?", "Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {

                var sites = BlogsSettings.Sites.ToList();
                sites.RemoveAt(item.Index);
                _sitesSettings.Remove(item.Text);

                BlogsSettings.UpdateSitesFile(sites);
                lvSites.Items.Remove(item);
                ArrangeTabVisibility();
                lvSites_SelectedIndexChanged(null, null);
            }
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (lvSites.SelectedItems.Count == 0) return;
            var item = lvSites.SelectedItems[0];
            ListViewItem newItem = lvSites.Items.Add(item.Text + " " + DateTime.Now.Ticks);
            listviewOperation = "DUPLICATE" + item.Text;
            // Place the newly-added item into edit mode immediately
            newItem.BeginEdit();
        }

        void AddOnChangeHandlerToInputControls(Control ctrl)
        {
            foreach (Control subctrl in ctrl.Controls)
            {
                if (subctrl is TextBox)
                    ((TextBox)subctrl).TextChanged +=
                        new EventHandler(InputControls_OnChange);
                else if (subctrl is CheckBox)
                    ((CheckBox)subctrl).CheckedChanged +=
                        new EventHandler(InputControls_OnChange);
                else if (subctrl is RadioButton)
                    ((RadioButton)subctrl).CheckedChanged +=
                        new EventHandler(InputControls_OnChange);
                else if (subctrl is ListBox)
                    ((ListBox)subctrl).SelectedIndexChanged +=
                        new EventHandler(InputControls_OnChange);
                else if (subctrl is ComboBox)
                    ((ComboBox)subctrl).SelectedIndexChanged +=
                        new EventHandler(InputControls_OnChange);
                else if (subctrl is NumericUpDown)
                    ((NumericUpDown)subctrl).ValueChanged+=
                        new EventHandler(InputControls_OnChange);
                else
                {
                    if (subctrl.Controls.Count > 0)
                        this.AddOnChangeHandlerToInputControls(subctrl);
                }
            }
        }

        private void InputControls_OnChange(object sender, EventArgs e)
        {
            if (lvSites.SelectedItems.Count == 0) return;
            _sitesSettings[lvSites.SelectedItems[0].Text] = GetOptionsFromControls();

        }
    }
}
