using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PttLib;
using PttLib.Helpers;
using WordPressSharp;
using WordpressScraper;
using WordpressScraper.Dal;
using WordpressScraper.Ftp;
using WordpressScraper.Helpers;

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
        private const int PostIdColumnIndex = 13;
        private ProgramOptions _options;

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
            FillSites();

            ArrangeTemplateMenu();

#if (DEBUG)

            txtPostId.Visible = true;
            btnSetTitle.Visible = true;
#endif

        }

        private void ArrangeTemplateMenu()
        {
            var template = new ToolStripMenuItem("Hellish Simplicity", null, FixTemplate);
            fixWordpressTemplatesToolStripMenuItem.DropDownItems.Add(template);
        }

        private void FixTemplate(object sender, EventArgs e)
        {
            var menuClicked = sender as ToolStripMenuItem;
            if (menuClicked == null) return;
            //MessageBox.Show(menuClicked.Text + " clicked");

            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();
            if (string.IsNullOrEmpty(_options.FtpUrl))
            {
                MessageBox.Show("In order to fix templates, please set up FTP account from settings.");
                return;
            }
            var ftp = new Ftp(FtpConfiguration) ;
            if (!string.IsNullOrEmpty(ftp.TestConnection()))
            {
                MessageBox.Show("Cannot connect to FTP, please check your settings.");
                return;
            }

            var ftpDir = "wp-content/themes/hellish-simplicity-child";
            try
            {
                ftp.MakeFtpDir(ftpDir);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                return;
            }
            var files = Directory.GetFiles("ChildTemplates/hellish-simplicity-child");
            foreach (var file in files)
            {
                try
                {
                    ftp.UploadFileFtp(file, ftpDir);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
            MessageBox.Show("Fixing finished");

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
            SetStatus("Got " + e.SubItems[1].Text);
            barStatus.PerformStep();
        }

        private void EnDis(bool enabled)
        {
            grpTop.Enabled = enabled;
            pnlItemOps.Enabled = enabled;
            btnStart.Enabled = enabled;
            btnStopScrape.Enabled = !enabled;


        }

        private void GettingSourceItemsFinished(object sender, EventArgs e)
        {
            ResetBarStatus();
            EnDis(true);
            btnGo.Enabled = lvItems.Items.Count > 0;
            Cursor.Current = Cursors.Default;
            lvwColumnSorter = new ListViewColumnSorter();
            lvItems.ListViewItemSorter = lvwColumnSorter;
            SetStatus("Getting source items finished");

            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();

            if (_options.ShowMessageBoxes)
            {
                MessageBox.Show("Getting source items finished");
            }

            _sourceItemFactory.NoSourceFound -= NoSourceFound;
            _sourceItemFactory.GettingSourceItemsStopped -= GettingSourceItemsStopped;
            _sourceItemFactory.ProcessFinished -= GettingSourceItemsFinished;
            _sourceItemFactory.SourceItemGot -= SourceItemGot;
            _sourceItemFactory.PageParsed -= PageParsed;
            _sourceItemFactory.TotalResultsFound -= TotalResultsFound;
            _sourceItemFactory.SourceItemsGot -= SourceItemsGot;

            _sourceItemFactory = null;

        }

        private void TotalResultsFound(object sender, string e)
        {
            if (lblTotalResults.Text != "")
            {
                lblTotalResults.Text += ", ";
            }
            lblTotalResults.Text += e;
            totalCountTooltip.SetToolTip(lblTotalResults, lblTotalResults.Text);

        }

        private void GettingSourceItemsStopped(object sender, EventArgs e)
        {
            ResetBarStatus();
            GettingSourceItemsFinished(null, null);
            SetStatus("Getting source items stopped!");
        }

        private void NoSourceFound(object sender, string e)
        {
            SetStatus(e);

            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();

            if (_options.ShowMessageBoxes)
            {
                MessageBox.Show(e);
            }
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
            EnDis(false);
            numPage_ValueChanged(null, null);
            Cursor.Current = Cursors.WaitCursor;
            ResetBarStatus(true);
            btnGo.Enabled = false;
            lblTotalResults.Text = "";

            var pageStart = (int)numPage.Value;
            var pageEnd = chkAllPages.Checked ? (int)numPageTo.Maximum : (int)numPageTo.Value;
            barStatus.Maximum = pageEnd - pageStart;

            _sourceItemFactory = new SourceItemFactory();
            _sourceItemFactory.NoSourceFound += NoSourceFound;
            _sourceItemFactory.GettingSourceItemsStopped += GettingSourceItemsStopped;
            _sourceItemFactory.ProcessFinished += GettingSourceItemsFinished;
            _sourceItemFactory.SourceItemGot += SourceItemGot;
            _sourceItemFactory.TotalResultsFound += TotalResultsFound;
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
            var timeTook = _stopWatch.Elapsed.TotalMinutes.ToString("0.00");
            lblDateTime.Text = string.Format("Took {0} mins", timeTook);
            if (_options.ShowMessageBoxes)
            {
                MessageBox.Show(string.Format("Post creation finished, took {0} mins", timeTook));
            }
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
            lvItem.SubItems[PostIdColumnIndex].Text = status;
            lvItem.EnsureVisible();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select items to transfer!");
                return;
            }

            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();

            _stopWatch = new Stopwatch();
            _stopWatch.Start();

            EnDisItems(false);

            lblDateTime.Text = "Started at " + DateTime.Now.ToLongTimeString();

            using (var dal = new Dal(MySqlConnectionString))
            {
                _blogCache = new BlogCache(dal);

                if (_options.UseCache)
                {
                    SetStatus("Loading present posts and tags in the blog(this may take some time)...");
                    Application.DoEvents();
                    _blogCache.Start(_options.BlogUrl);
                    Application.DoEvents();
                }
                SetStatus("Ready");
                ResetBarStatus(true);
                barStatus.Maximum = lvItems.SelectedItems.Count;
                _postFactory = new PostFactory(
                        SiteConfig,
                        new Ftp(FtpConfiguration),
                        _blogCache,
                        dal,
                        _options);
                var items = ItemsFromListView(lvItems.SelectedItems);
                _postFactory.PostCreated += PostCreated;
                _postFactory.PostBeingCreated += PostBeingCreated;
                _postFactory.PostsCreated += PostsCreated;
                _postFactory.PostCreationStopped += PostCreationStopped;
                _postFactory.Create(items);
            }

        }

        private IList<Item> ItemsFromListView(ListView.SelectedListViewItemCollection items)
        {
            return (from object item in items select ItemFromListView((ListViewItem)item)).ToList();
        }

        private Item ItemFromListView(ListViewItem item)
        {

            var imageUrls = item.SubItems[7].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var postItem = new Item()
            {
                Order = int.Parse(item.Text),
                Id = int.Parse(item.SubItems[1].Text),
                Url = item.SubItems[2].Text,
                Title = item.SubItems[3].Text,
                MetaDescription = item.SubItems[4].Text,
                Content = item.SubItems[5].Text,
                Price = string.IsNullOrEmpty(item.SubItems[6].Text) ? 0 : double.Parse(item.SubItems[6].Text, CultureInfo.InvariantCulture),
                Tags = item.SubItems[8].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                ItemImages = imageUrls.Select(imageUrl => new ItemImage() { OriginalSource = imageUrl, Primary = true }).ToList(),
                Site = item.SubItems[9].Text,
                WordCount = int.Parse(item.SubItems[10].Text, new CultureInfo("en-US")),
                Relevance = int.Parse(item.SubItems[12].Text)

            };
            if (postItem.ItemImages != null)
            {
                foreach (var itemImage in postItem.ItemImages)
                {
                    itemImage.ContainingItem = postItem;
                }
            }
            return postItem;
        }

        private void SetStatus(string status)
        {
            lblStatus.Text = status;
            barStatus.Margin = new Padding(lblStatus.Width - 50, barStatus.Margin.Top, barStatus.Margin.Right,
                barStatus.Margin.Bottom);
        }

        private void EnDisItems(bool enabled)
        {
            optionsToolStripMenuItem.Enabled = enabled;
            settingsToolStripMenuItem.Enabled = enabled;
            settingsToolStripMenuItem.Enabled = enabled;
            btnStop.Enabled = !enabled;
            btnGo.Enabled = enabled;
            btnStart.Enabled = enabled;
            lvItems.Enabled = enabled;
            btnStopScrape.Enabled = enabled;
            pnlItemOps.Enabled = enabled;
        }

        public FtpConfig FtpConfiguration
        {
            get
            {
                return new FtpConfig() { Url = _options.FtpUrl, UserName = _options.FtpUser, Password = _options.FtpPassword };
            }
        }

        public WordPressSiteConfig SiteConfig
        {
            get
            {
                return new WordPressSiteConfig() { BaseUrl = _options.BlogUrl, BlogId = 1, Username = _options.BlogUser, Password = _options.BlogPassword };
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
            var postId = item.SubItems[PostIdColumnIndex].Text;
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

        private void FixEmptyNumericUpDown(NumericUpDown control)
        {
            if (control.Text == "")
            {
                control.Text = "0";
            }
        }


        private void numPage_ValueChanged(object sender, EventArgs e)
        {
            FixEmptyNumericUpDown(numPageTo);
            if ((int)numPageTo.Value < (int)numPage.Value)
            {
                numPageTo.Value = numPage.Value;
            }
        }

        private void numPageTo_ValueChanged(object sender, EventArgs e)
        {
            FixEmptyNumericUpDown(numPage);
            if ((int)numPageTo.Value < (int)numPage.Value)
            {
                numPage.Value = numPageTo.Value;
            }
        }

        private string MySqlConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3}; Allow User Variables=True", _options.DatabaseUrl, _options.DatabaseName, _options.DatabaseUser, _options.DatabasePassword);
            }
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

        private void btnScrumble_Click(object sender, EventArgs e)
        {
            lvItems.ListViewItemSorter = null;

            var itemCount = lvItems.Items.Count;
            if (itemCount == 0) return;

            lvItems.ListViewItemSorter = null;
            ScrambleBlock(0, itemCount);
            ArrangeOrder();
            lvwColumnSorter = new ListViewColumnSorter();
            lvItems.ListViewItemSorter = lvwColumnSorter;

        }

        /// <summary>
        /// [startIndex, endIndex)
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        private void ScrambleBlock(int startIndex, int endIndex)
        {
            for (var i = startIndex; i < endIndex; i++)
            {
                var randomIndex = Helper.GetRandomNumber(startIndex, endIndex);
                var moveRandomIndex = Helper.GetRandomNumber(startIndex, endIndex);
                var item = lvItems.Items[randomIndex];
                lvItems.Items.RemoveAt(randomIndex);
                lvItems.Items.Insert(moveRandomIndex, item);
            }

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
                if (selectedItem is IDisposable)
                {
                    ((IDisposable)selectedItem).Dispose();
                }
            }
            ArrangeOrder();
            GC.Collect();
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

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmOptions = new frmOptions();
            frmOptions.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmSettings = new frmSettings();
            frmSettings.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.Text);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvItems.SelectedItems)
            {
                var index = item.Index;
                if (index == 0) break;
                lvItems.Items.RemoveAt(index);
                lvItems.Items.Insert(index - 1, item);
            }
        }


        private void btnRelevanceScramble_Click(object sender, EventArgs e)
        {
            var ccea = new ColumnClickEventArgs(12);
            if (lvwColumnSorter == null) return;
            lvwColumnSorter.SortColumn = 12;
            lvwColumnSorter.Order = SortOrder.Ascending;
            lvItems_ColumnClick(null, ccea);

            lvItems.ListViewItemSorter = null;

            var programOptionsFactory = new ProgramOptionsFactory();
            var programOptions = programOptionsFactory.Get();
            var mergeBlockSize = programOptions.MergeBlockSize;

            var zeroRelevanceStartIndex = -1;
            for (int i = 0; i < lvItems.Items.Count; i++)
            {
                var item = lvItems.Items[i];
                var relevanceScore = int.Parse(item.SubItems[12].Text);
                if (relevanceScore == 0)
                {
                    zeroRelevanceStartIndex = i;
                    break;
                }
            }
            if (zeroRelevanceStartIndex == -1)
            {
                MessageBox.Show("No 0 relevance item found, scramble not to be done!");
                lvwColumnSorter = new ListViewColumnSorter();
                lvItems.ListViewItemSorter = lvwColumnSorter;
                return;
            }
            if (programOptions.ScrambleLeadPosts)
            {
                ScrambleBlock(0, zeroRelevanceStartIndex);
            }
            ScrambleBlock(zeroRelevanceStartIndex, lvItems.Items.Count);

            var primaryPostIndex = 0;
            var cumulativeWordCount = 0;
            var itemBlockIndex = 0;
            for (int i = 0; i < lvItems.Items.Count; i++)
            {
                var item = lvItems.Items[i];
                var wordCount = int.Parse(item.SubItems[10].Text);
                cumulativeWordCount += wordCount;

                if (cumulativeWordCount >= mergeBlockSize)
                {
                    //finish this block, start a new block
                    primaryPostIndex = i + 1;
                    cumulativeWordCount = 0;
                    itemBlockIndex = 0;
                }
                else
                {
                    if (zeroRelevanceStartIndex >= lvItems.Items.Count) continue; //can be break?

                    //continue to this block
                    itemBlockIndex++;
                    var itemToMove = lvItems.Items[zeroRelevanceStartIndex];
                    lvItems.Items.RemoveAt(zeroRelevanceStartIndex);
                    lvItems.Items.Insert(primaryPostIndex + itemBlockIndex, itemToMove);
                    zeroRelevanceStartIndex++;
                }

            }

            ArrangeOrder();
            lvwColumnSorter = new ListViewColumnSorter();
            lvItems.ListViewItemSorter = lvwColumnSorter;


        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count == 0) return;
            for (int i = lvItems.SelectedItems.Count - 1; i >= 0; i--)
            {
                var item = lvItems.SelectedItems[i];
                var index = item.Index;
                if (index == lvItems.Items.Count - 1)
                {
                    break;
                }
                lvItems.Items.RemoveAt(index);
                lvItems.Items.Insert(index + 1, item);

            }
        }

        private void fixWordpressTemplatesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addUpdateExtraFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();
            if (string.IsNullOrEmpty(_options.FtpUrl))
            {
                MessageBox.Show("In order to update wp files, please set up FTP account from settings.");
                return;
            }
            var ftp = new Ftp(FtpConfiguration);
            if (!string.IsNullOrEmpty(ftp.TestConnection()))
            {
                MessageBox.Show("Cannot connect to FTP, please check your settings.");
                return;
            }

            var files = Directory.GetFiles("blog");
            foreach (var file in files)
            {
                try
                {
                    ftp.UploadFileFtp(file, "");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
            MessageBox.Show("Finished");

        }

        private void createAuthorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmAuthors = new frmAuthors();
            frmAuthors.ShowDialog();
        }

        private void chkSites_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnMultiplyPrice_Click(object sender, EventArgs e)
        {
            if(lvItems.SelectedItems.Count==0)
            {
                MessageBox.Show("Please select items to multiply the price!");
                return;
            }
            double coeff;
            if(!double.TryParse(txtPriceCoeff.Text,NumberStyles.AllowDecimalPoint,new CultureInfo("en-US"),  out coeff))
            {
                MessageBox.Show("Invalid coefficient!");
                return;
                
            }


            foreach (ListViewItem selectedItem in lvItems.SelectedItems)
            {
                var price = string.IsNullOrEmpty(selectedItem.SubItems[6].Text)
                                ? 0
                                : double.Parse(selectedItem.SubItems[6].Text, CultureInfo.InvariantCulture);

                selectedItem.SubItems[6].Text = Math.Round(price * coeff, 2).ToString(new CultureInfo("en-US"));
            }
        }

  

        private void publishToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var frmPublish = new frmPublish();
            frmPublish.ShowDialog();
        }

    }
}
