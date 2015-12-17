using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PttLib;
using PttLib.Helpers;
using WordPressSharp;
using WordpressScraper.Dal;

namespace WindowsFormsApplication1
{
    public partial class frmMain : Form
    {
        private const string DefaultKey = "baby spoon";
        private BlogCache _blogCache;
        private ListViewColumnSorter lvwColumnSorter;
        private Stopwatch _stopWatch = new Stopwatch();
        private PostFactory _postFactory = null;
        private SourceItemFactory _sourceItemFactory = null;


        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            txtUrl.Text = DefaultKey;
            btnGo.Enabled = false;
            btnStop.Enabled = false;
            btnStopScrape.Enabled = false;
            System.IO.Directory.CreateDirectory("Logs");
            this.Text += " v" + Assembly.GetExecutingAssembly().GetName().Version;
            lblDateTime.Text = "";
            chkNoAPI_CheckedChanged(null, null);
            FillSites();

#if (DEBUG)

            txtPostId.Visible = true;
            btnSetTitle.Visible = true;
#endif

        }

        private void FillSites()
        {
            chkSites.Items.Clear();
            var siteFactory = new SiteFactory();
            foreach (var name in siteFactory.GetNames)
            {
                chkSites.Items.Add(name);
            }
            chkSites.SetItemChecked(0, true);
        }

        private void PageParsed(object sender, int e)
        {
            ResetBarStatus(true);
            barStatus.Maximum = e;
        }

        private void SourceItemsGot(object sender, IList<ListViewItem> e)
        {
            lvItems.ListViewItemSorter = null;
            lvItems.BeginUpdate();
            lvItems.Items.AddRange(e.ToArray());
            lvItems.EndUpdate();
        }

        private void SourceItemGot(object sender, ListViewItem e)
        {
            SetStatus("Got "+ e.SubItems[1].Text);
            barStatus.PerformStep();
        }

        private void GettingSourceItemsFinished(object sender, EventArgs e)
        {
            ResetBarStatus();
            grpTop.Enabled = true;
            btnGo.Enabled = lvItems.Items.Count > 0;
            btnStart.Enabled = true;
            Cursor.Current = Cursors.Default;
            btnStopScrape.Enabled = false;
            lvwColumnSorter = new ListViewColumnSorter();
            lvItems.ListViewItemSorter = lvwColumnSorter;
            SetStatus("Getting source items finished");

        }

        private void GettingSourceItemsStopped(object sender, EventArgs e)
        {
            ResetBarStatus();
            GettingSourceItemsFinished(null, null);
            SetStatus("Getting source items stopped!");
        }

        private void NoSourceFound(object sender, string e)
        {
            MessageBox.Show(e);
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtUrl.Text == "")
            {
                MessageBox.Show("Enter Keyword!");
                return;
            }
            var sitesCount = chkSites.CheckedItems.Count;
            if (sitesCount == 0)
            {
                MessageBox.Show("Select sites!");
                return;
            }

            grpTop.Enabled = false;
            numPage_ValueChanged(null, null);
            Cursor.Current = Cursors.WaitCursor;
            btnStopScrape.Enabled = true;
            ResetBarStatus(true);
            btnStart.Enabled = false;
            btnGo.Enabled = false;

            var pageStart = (int)numPage.Value;
            var pageEnd = chkAllPages.Checked ? (int)numPageTo.Maximum : (int)numPageTo.Value;
            barStatus.Maximum = pageEnd - pageStart;

            _sourceItemFactory = new SourceItemFactory();
            _sourceItemFactory.NoSourceFound += NoSourceFound;
            _sourceItemFactory.GettingSourceItemsStopped += GettingSourceItemsStopped;
            _sourceItemFactory.ProcessFinished += GettingSourceItemsFinished;
            _sourceItemFactory.SourceItemGot += SourceItemGot;
            _sourceItemFactory.PageParsed += PageParsed;
            _sourceItemFactory.SourceItemsGot += SourceItemsGot;
            var checkedSites = (from object checkedItem in chkSites.CheckedItems select checkedItem.ToString()).ToList();
            _sourceItemFactory.GetSourceItems(checkedSites, txtUrl.Text, pageStart, pageEnd, lvItems.Items.Count + 1);

        }

        private void ResetBarStatus(bool visible = false)
        {
            barStatus.Value = 0;
            barStatus.Visible = visible;
        }


        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvItems.Items)
            {
                item.Selected = true;
            }
            lvItems.Focus();
        }


        private void PostCreationStopped(object sender, EventArgs e)
        {
            ResetBarStatus();
            SetStatus("Post creation stopped");
        }

        private void PostBeingCreated(object sender, Item e)
        {
            SetStatus("Creating item on the blog: " + e.Order);
        }

        private void PostsCreated(object sender, IList<Item> e)
        {
            ResetBarStatus();
            EnDisItems(true);
            _stopWatch.Stop();
            lblDateTime.Text = string.Format("Took {0} mins", _stopWatch.Elapsed.TotalMinutes.ToString("0.00"));
        }

        private void PostCreated(object sender, Item e)
        {
            barStatus.PerformStep();
            var status = "";
            switch (e.PostId)
            {
                case -1:
                    status = "Error";
                    break;
                case 0:
                    status = "Exists";
                    break;
                case -2:
                    status = "Invalid";
                    break;
                default:
                    status = e.PostId.ToString();
                    break;
            }
            var lvItem = lvItems.FindItemWithText(e.Order.ToString());
            if (lvItem == null) return;
            lvItem.SubItems[11].Text = status;
            lvItem.EnsureVisible();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select items to transfer!");
                return;
            }

            _stopWatch = new Stopwatch();
            _stopWatch.Start();
            CreateAuthors();

            EnDisItems(false);
           
            lblDateTime.Text = "Started at " + DateTime.Now.ToLongTimeString();

            using (var dal = new Dal(MySqlConnectionString))
            {
                _blogCache = new BlogCache(dal);

                if (chkCache.Checked)
                {
                    SetStatus("Loading present posts and tags in the blog(this may take some time)...");
                    Application.DoEvents();
                    _blogCache.Start(txtBlogUrl.Text);
                    Application.DoEvents();
                }
                SetStatus("Ready");
                ResetBarStatus(true);
                barStatus.Maximum = lvItems.SelectedItems.Count;
                _postFactory = new PostFactory(
                        SiteConfig,
                        FtpConfiguration,
                        _blogCache,
                        dal,
                        txtBlogUrl.Text,
                        chkNoAPI.Checked,
                        chkResizeImages.Checked ? (int)numMaxImageDimension.Value : 0,
                        (int)numThumbnailSize.Value,
                        chkCache.Checked,
                        chkFeatureImage.Checked);
                var items = ItemsFromListView(lvItems.SelectedItems);
                _postFactory.PostCreated += PostCreated;
                _postFactory.PostBeingCreated += PostBeingCreated;
                _postFactory.PostsCreated += PostsCreated;
                _postFactory.PostCreationStopped += PostCreationStopped;
                _postFactory.Create(items);
            }

        }

        private void CreateAuthors()
        {
            if (string.IsNullOrEmpty(txtAuthors.Text))
            {
                return;
            }
            var authors = txtAuthors.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            using (var dal = new Dal(MySqlConnectionString))
            {
                var userDal = new UserDal(dal);
                foreach (var author in authors)
                {
                    if (string.IsNullOrEmpty(author.Trim()))
                    {
                        continue;
                    }
                    userDal.InsertUser(author);
                }
            }
        }

        private IList<Item> ItemsFromListView(ListView.SelectedListViewItemCollection items)
        {
            return (from object item in items select ItemFromListView((ListViewItem)item)).ToList();
        }

        private Item ItemFromListView(ListViewItem item)
        {
            var imageUrls=item.SubItems[7].Text.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            return new Item()
            {
                Order = int.Parse(item.Text),
                Id = int.Parse(item.SubItems[1].Text),
                Url = item.SubItems[2].Text,
                Title = item.SubItems[3].Text,
                MetaDescription = item.SubItems[4].Text,
                Content = item.SubItems[5].Text,
                Price = string.IsNullOrEmpty(item.SubItems[6].Text) ? 0 : double.Parse(item.SubItems[6].Text, CultureInfo.InvariantCulture),
                Tags = item.SubItems[8].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                ItemImages = imageUrls.Select(imageUrl => new ItemImage() {OriginalSource = imageUrl}).ToList(),
                Site = item.SubItems[9].Text,
                WordCount = int.Parse(item.SubItems[10].Text)

            };
        }

        private void SetStatus(string status)
        {
            lblStatus.Text = status;
            barStatus.Margin = new Padding(lblStatus.Width - 50, barStatus.Margin.Top, barStatus.Margin.Right,
                barStatus.Margin.Bottom);
        }

        private void EnDisItems(bool enabled)
        {
            btnStop.Enabled = !enabled;
            btnGo.Enabled = enabled;
            btnStart.Enabled = enabled;
            lvItems.Enabled = enabled;
            grpMysql.Enabled = enabled;
            grpAuthors.Enabled = enabled;
            btnStopScrape.Enabled = enabled;
            chkNoAPI.Enabled = enabled;
            chkFeatureImage.Enabled = enabled;
            chkResizeImages.Enabled = enabled;
            numMaxImageDimension.Enabled = enabled;
            numThumbnailSize.Enabled = enabled;
            if (!enabled)
            {
                grpBlogProp.Enabled = false;
                grpFtp.Enabled = false;

            }
            else
            {
                grpBlogProp.Enabled = !chkNoAPI.Checked;
                grpFtp.Enabled = chkNoAPI.Checked;
            }
        }

        public FtpConfig FtpConfiguration
        {
            get
            {
                return new FtpConfig() { Url = txtFtpUrl.Text, UserName = txtFtpUserName.Text, Password = txtFtpPassword.Text };
            }
        }

        public WordPressSiteConfig SiteConfig
        {
            get
            {
                return new WordPressSiteConfig() { BaseUrl = txtBlogUrl.Text, BlogId = 1, Username = txtUserName.Text, Password = txtPassword.Text };
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SetStatus("Waiting for the last operation to finish for stopping...");
            btnStop.Enabled = false;
            if (_postFactory != null)
            {
                _postFactory.CancelPostCreation();
            }
        }

        private void btnStopScrape_Click(object sender, EventArgs e)
        {
            _sourceItemFactory.CancelGettingSource();
            btnStopScrape.Enabled = false;
        }

        private void btnGetPost_Click(object sender, EventArgs e)
        {
            if (txtPostId.Text != "")
            {
                GetPost(txtPostId.Text);
                return;
            }

            if (lvItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select item to get!");
                return;
            }
            var item = lvItems.SelectedItems[0];
            var postId = item.SubItems[11].Text;
            if (postId == "" || postId == "Error" || postId == "Exists")
            {
                MessageBox.Show("No post id or problematic one!");
                return;
            }

            GetPost(postId);

        }

        private void GetPost(string postId)
        {
            using (var client = new WordPressClient(SiteConfig))
            {
                try
                {
                    var post = client.GetPost(int.Parse(postId));

                    var summary = post.Title + "\nCustom Fields:\n";
                    foreach (var cf in post.CustomFields)
                    {
                        summary += cf.Key + "=" + cf.Value + "\n";
                    }
                    MessageBox.Show(summary);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        private void chkAllPages_CheckedChanged(object sender, EventArgs e)
        {
            lblPageTo.Enabled = numPageTo.Enabled = !chkAllPages.Checked;

        }

        private void numPage_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numPageTo.Value < (int)numPage.Value)
            {
                numPageTo.Value = numPage.Value;
            }
        }

        private void numPageTo_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numPageTo.Value < (int)numPage.Value)
            {
                numPage.Value = numPageTo.Value;
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

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void lvItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lvItems.Sort();
            ArrangeOrder();
        }

        private void chkNoAPI_CheckedChanged(object sender, EventArgs e)
        {
            grpBlogProp.Enabled = !chkNoAPI.Checked;
            grpFtp.Enabled = chkNoAPI.Checked;
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

        private void btnScrumble_Click(object sender, EventArgs e)
        {
            var itemCount = lvItems.Items.Count;
            if (itemCount == 0) return;
            for (var i = 0; i < itemCount; i++)
            {
                var randomIndex = Helper.GetRandomNumber(0, itemCount);
                var moveRandomIndex = Helper.GetRandomNumber(0, itemCount);
                var item = lvItems.Items[randomIndex];
                lvItems.Items.RemoveAt(randomIndex);
                lvItems.Items.Insert(moveRandomIndex, item);

            }
            ArrangeOrder();
        }

        private void ArrangeOrder()
        {
            var itemCount = lvItems.Items.Count;
            for (var i = 0; i < itemCount; i++)
            {
                lvItems.Items[i].Text = (i + 1).ToString();
            }
        }

        private void WhiteBackground()
        {
            var itemCount = lvItems.Items.Count;
            for (var i = 0; i < itemCount; i++)
            {
                lvItems.Items[i].BackColor = Color.White;
            }
        }

        private void txtFindDuplicatePosts_Click(object sender, EventArgs e)
        {
            WhiteBackground();
            foreach (ListViewItem item in lvItems.Items)
            {
                var color = Color.FromArgb(255, Helper.GetRandomNumber(125, 225), Helper.GetRandomNumber(125, 225), Helper.GetRandomNumber(125, 225));

                foreach (ListViewItem item2 in lvItems.Items)
                {

                    if (item2.SubItems[3].Text == item.SubItems[3].Text && item2.SubItems[4].Text == item.SubItems[4].Text && item2.Text != item.Text)
                    {
                        item.BackColor = color;
                        item2.BackColor = color;

                    }
                }
            }
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select items to remove!");
                return;
            }
            foreach (ListViewItem selectedItem in lvItems.SelectedItems)
            {
                lvItems.Items.Remove(selectedItem);
            }
            ArrangeOrder();
        }

        private void btnRemoveDuplicates_Click(object sender, EventArgs e)
        {
            var itemsToRemove = new List<ListViewItem>();
            for (var i = 0; i < lvItems.Items.Count; i++)
            {
                var baseItem = lvItems.Items[i];
                for (var j = i + 1; j < lvItems.Items.Count; j++)
                {
                    var compareItem = lvItems.Items[j];
                    if (compareItem.SubItems[3].Text == baseItem.SubItems[3].Text && compareItem.SubItems[4].Text == baseItem.SubItems[4].Text)
                    {
                        itemsToRemove.Add(compareItem);
                    }
                }
            }

            foreach (var item in itemsToRemove)
            {
                lvItems.Items.Remove(item);
            }

            WhiteBackground();
            ArrangeOrder();
        }

        private void btnNavigate_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select item to navigate!");
                return;
            }
            foreach (ListViewItem selectedItem in lvItems.SelectedItems)
            {
                var url = selectedItem.SubItems[2].Text;
                if (string.IsNullOrWhiteSpace(url))
                {
                    return;
                }
                if (!url.StartsWith("http")) url = "http://" + url;
                Process.Start(url);

            }

        }

        private void btnSetTitle_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count == 0) return;
            lvItems.SelectedItems[0].SubItems[3].Text = txtPostId.Text;

        }

    }
}
