﻿using System;
using System.Collections.Generic;
using System.Data;
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
using WordpressScraper.Ftp;
using WordpressScraper.Helpers;
using WordPressSharp.Models;
using WpsLib.Dal;
using WpsLib.Item;
using WpsLib.ProgramOptions;

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
            SetFormCaption();
            btnGo.Enabled = false;
            btnStop.Enabled = false;
            btnStopScrape.Enabled = false;
            Directory.CreateDirectory("Logs");
            lblDateTime.Text = "";
            lblSelection.Text = "";
            FillSites();
            FillBlogs();

#if (DEBUG)

            txtPostId.Visible = true;
            btnSetTitle.Visible = true;
#endif

        }

        private void SetFormCaption(string blogName = "")
        {
            this.Text = "Wordpress Scraper v" + Assembly.GetExecutingAssembly().GetName().Version;
            if (!string.IsNullOrEmpty(blogName))
            {
                this.Text += " - " + blogName;
            }
        }

        private void FillBlogs()
        {
            var blogIndex = 1;
            connectToolStripMenuItem.DropDownItems.Clear();
            foreach (var site in BlogsSettings.Sites)
            {
                var blogMenu = new ToolStripMenuItem()
                {
                    Text = site,
                    Name = "blogItem" + blogIndex

                };
                blogMenu.Click += new EventHandler(SelectBlog);
                connectToolStripMenuItem.DropDownItems.Add(blogMenu);
                blogIndex++;
            }
        }

        private bool _blogSelected = false;

        private void SelectBlog(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            var options = BlogsSettings.ProgramOptionsForBlog(clickedItem.Text);

            var settings = new List<Tuple<string, string>>();

            settings.Add(new Tuple<string, string>("BlogUrl", options.BlogUrl));
            settings.Add(new Tuple<string, string>("BlogUser", options.BlogUser));
            settings.Add(new Tuple<string, string>("BlogPassword", options.BlogPassword));
            settings.Add(new Tuple<string, string>("DatabaseUrl", options.DatabaseUrl));
            settings.Add(new Tuple<string, string>("DatabaseName", options.DatabaseName));
            settings.Add(new Tuple<string, string>("DatabaseUser", options.DatabaseUser));
            settings.Add(new Tuple<string, string>("DatabasePassword", options.DatabasePassword));
            settings.Add(new Tuple<string, string>("FtpUrl", options.FtpUrl));
            settings.Add(new Tuple<string, string>("FtpUser", options.FtpUser));
            settings.Add(new Tuple<string, string>("FtpPassword", options.FtpPassword));
            settings.Add(new Tuple<string, string>("ProxyAddress", options.ProxyAddress));
            settings.Add(new Tuple<string, string>("ProxyPort", options.ProxyPort.ToString()));
            settings.Add(new Tuple<string, string>("UseProxy", options.UseProxy.ToString()));

            settings.Add(new Tuple<string, string>("YoutubeClient", options.YoutubeClient));
            settings.Add(new Tuple<string, string>("YoutubeClientSecret", options.YoutubeClientSecret));

            ConfigurationHelper.UpdateSettings(settings);

            foreach (ToolStripMenuItem dropDownItem in connectToolStripMenuItem.DropDownItems)
            {
                dropDownItem.Checked = false;
            }

            clickedItem.Checked = true;
            _blogSelected = true;
            SetFormCaption(clickedItem.Text);
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

        private static object _lock = new Object();
        private void SourceItemsGot(object sender, IList<ListViewItem> e)
        {
            lock (_lock)
            {
                lvItems.ListViewItemSorter = null;
                lvItems.BeginUpdate();
                var itemCountFirst = lvItems.Items.Count;
                lvItems.Items.AddRange(e.ToArray());

                var itemCount = lvItems.Items.Count;
                for (var i = itemCountFirst; i < itemCount; i++)
                {
                    lvItems.Items[i].Text = (i + 1).ToString();
                }

                lvItems.EndUpdate();
            }
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
            SetStatus("Getting source items finished");

            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();

            if (_options.ShowMessageBoxes)
            {
                MessageBox.Show("Getting source items finished");
            }
            _stopWatch.Stop();
            var timeTook = _stopWatch.Elapsed.TotalMinutes.ToString("0.00");
            lblDateTime.Text = string.Format("Took {0} mins", timeTook);

            _sourceItemFactory.NoSourceFound -= NoSourceFound;
            _sourceItemFactory.GettingSourceItemsStopped -= GettingSourceItemsStopped;
            _sourceItemFactory.ProcessFinished -= GettingSourceItemsFinished;
            _sourceItemFactory.SourceItemGot -= SourceItemGot;
            _sourceItemFactory.TotalResultsFound -= TotalResultsFound;
            _sourceItemFactory.SourceItemsGot -= SourceItemsGot;


            _sourceItemFactory = null;

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
            ///barStatus.Maximum = pageEnd - pageStart;
            barStatus.Maximum = 0;
            lblDateTime.Text = "";
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();
            HashSet<string> existingIds = null;
            if (_options.SkipSearchingPosted)
            {
                using (var dal = new Dal(MySqlConnectionString))
                {
                    _blogCache = new BlogCache(dal);
                    if (_options.UseCache)
                    {
                        SetStatus("Loading present posts and tags in the blog(this may take some time)...");
                        Application.DoEvents();
                        existingIds = _blogCache.IdsPresent(_options.BlogUrl);
                        Application.DoEvents();
                    }
                }

            }
            _sourceItemFactory = new SourceItemFactory();
            _sourceItemFactory.NoSourceFound += NoSourceFound;
            _sourceItemFactory.GettingSourceItemsStopped += GettingSourceItemsStopped;
            _sourceItemFactory.ProcessFinished += GettingSourceItemsFinished;
            _sourceItemFactory.SourceItemGot += SourceItemGot;
            _sourceItemFactory.TotalResultsFound += TotalResultsFound;
            _sourceItemFactory.SourceItemsGot += SourceItemsGot;
            _sourceItemFactory.ExceptionOccured += ExceptionOccuredWhileGettingItems;
            var checkedSites = (from object checkedItem in chkSites.CheckedItems select checkedItem.ToString()).ToList();
            _sourceItemFactory.GetSourceItems(checkedSites, txtUrl.Text, pageStart, pageEnd, lvItems.Items.Count + 1, existingIds);

        }

        private void ExceptionOccuredWhileGettingItems(object sender, Exception e)
        {
            MessageBox.Show("Exception: " + e.ToString());
            Logger.LogExceptions(e);
        }

        private void TotalResultsFound(object sender, TotalResultsFoundEventArgs e)
        {
            if (lblTotalResults.Text != "")
            {
                lblTotalResults.Text += ", ";
            }
            barStatus.Maximum += e.TotalPageInRange;
            lblTotalResults.Text += string.Format("{0}:{1}/{2}", e.Site, e.TotalPageInRange, e.TotalForKeyword);
            totalCountTooltip.SetToolTip(lblTotalResults, lblTotalResults.Text);
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
                case -3:
                    status = "Image error";
                    break;
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
            if (!_blogSelected)
            {
                MessageBox.Show("First connect to blog from File>Connect!");
                return;
            }

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
            connectToolStripMenuItem.Enabled = enabled;
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
            if (_sourceItemFactory == null) return;
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
            var sortType = SortType.NotKnown;
            switch (e.Column)
            {
                case 2:
                case 3:
                case 4:
                case 5:
                case 7:
                case 8:
                case 9:
                case 13:
                    sortType = SortType.String;
                    break;
                case 0:
                case 1:
                case 10:
                case 12:
                    sortType = SortType.Numeric;
                    break;
                case 11:
                    sortType = SortType.Date;
                    break;
            }

            if (lvwColumnSorter == null || lvItems.ListViewItemSorter == null)
            {

                lvwColumnSorter = new ListViewColumnSorter();
                lvItems.ListViewItemSorter = lvwColumnSorter;
            }
            lvwColumnSorter.SortType = sortType;

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
                var randomIndex = PttLib.Helpers.Helper.GetRandomNumber(startIndex, endIndex);
                var moveRandomIndex = PttLib.Helpers.Helper.GetRandomNumber(startIndex, endIndex);
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
                var color = Color.FromArgb(255, PttLib.Helpers.Helper.GetRandomNumber(125, 225), PttLib.Helpers.Helper.GetRandomNumber(125, 225), PttLib.Helpers.Helper.GetRandomNumber(125, 225));

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
            var tags = new HashSet<string>();
            foreach (ListViewItem item in lvItems.Items)
            {
                // HashSet.Add() returns false if it already contains the key.
                if (!tags.Add(item.SubItems[3].Text + "***" + item.SubItems[4].Text))
                    itemsToRemove.Add(item);
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
            _blogSelected = false;
            SetFormCaption();
            FillBlogs();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wordpress Scraper v" + Assembly.GetExecutingAssembly().GetName().Version);
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
            //relevance e gore sirala
            var ccea = new ColumnClickEventArgs(2);

            lvItems_ColumnClick(null, ccea);
            ccea = new ColumnClickEventArgs(12);
            lvItems_ColumnClick(null, ccea);
            lvItems_ColumnClick(null, ccea);
            lvItems.ListViewItemSorter = null;

            var programOptionsFactory = new ProgramOptionsFactory();
            var programOptions = programOptionsFactory.Get();
            var mergeBlockSize = programOptions.MergeBlockSize;

            //zero relevance baslayan yeri bul
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
                return;
            }

            // mergeBlockSize indan buyuk olan herhangi bir eleman her zaman tek basina gidecek.... bunu unuttun...
            try
            {
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

                        while (true)
                        {
                            if (zeroRelevanceStartIndex >= lvItems.Items.Count) break;
                            var itemToMove = lvItems.Items[zeroRelevanceStartIndex];
                            var zeroRelWordCount = int.Parse(itemToMove.SubItems[10].Text);
                            if (zeroRelWordCount < mergeBlockSize)
                            {
                                lvItems.Items.RemoveAt(zeroRelevanceStartIndex);
                                lvItems.Items.Insert(primaryPostIndex + itemBlockIndex, itemToMove);
                                zeroRelevanceStartIndex++;
                                break;
                            }
                            zeroRelevanceStartIndex++;
                        }
                    }

                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                Logger.LogExceptions(exception);
            }

            ArrangeOrder();
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


        private void addUpdateExtraFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_blogSelected)
            {
                MessageBox.Show("First connect to blog from File>Connect!");
                return;
            }

            PrepareTemplates();
        }

        private void PrepareTemplates()
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
            var frmPrepareTemplate = new frmPrepareTemplate();
            frmPrepareTemplate.ShowDialog();
        }

        private void createAuthorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_blogSelected)
            {
                MessageBox.Show("First connect to blog from File>Connect!");
                return;
            }

            var frmAuthors = new frmAuthors();
            frmAuthors.ShowDialog();
        }

        private void chkSites_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnMultiplyPrice_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select items to multiply the price!");
                return;
            }
            double coeff;
            if (!double.TryParse(txtPriceCoeff.Text, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out coeff))
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
            if (!_blogSelected)
            {
                MessageBox.Show("First connect to blog from File>Connect!");
                return;
            }

            var frmPublish = new frmPublish();
            frmPublish.ShowDialog();
        }


        private void btnPublish_Click(object sender, EventArgs e)
        {
            var posts = new List<Post>();
            foreach (ListViewItem selectedItem in lvItems.SelectedItems)
            {
                var postId = selectedItem.SubItems[PostIdColumnIndex].Text;
                int postIdNumeric;
                if (!Int32.TryParse(postId, out postIdNumeric))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(postId))
                {
                    posts.Add(new Post()
                    {
                        Id = postId,
                        Title = selectedItem.SubItems[3].Text
                    });
                }
            }
            if (posts.Count == 0)
            {
                MessageBox.Show("Select the items already inserted as draft, to publish them");
                return;
            }

            var frmPublish = new frmPublish();
            frmPublish.Posts = posts;
            frmPublish.ShowDialog();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvItems.Items.Count == 0)
            {
                MessageBox.Show("Nothing to export!");
                return;
            }

            saveSettings.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveSettings.FilterIndex = 1;
            saveSettings.RestoreDirectory = true;
            saveSettings.FileName = "";

            if (saveSettings.ShowDialog() != DialogResult.OK) return;
            if (string.IsNullOrEmpty(saveSettings.FileName)) return;

            try
            {
                ExcelReport.ExportFromListView(lvItems, saveSettings.FileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }

        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openSettingFile.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openSettingFile.FilterIndex = 1;
            openSettingFile.RestoreDirectory = true;
            openSettingFile.FileName = "";

            if (openSettingFile.ShowDialog() != DialogResult.OK) return;
            if (string.IsNullOrEmpty(openSettingFile.FileName)) return;
            try
            {
                lvwColumnSorter = null;
                lvItems.ListViewItemSorter = null;
                var result = ExcelImportHelper.ImportToListView(openSettingFile.FileName, lvItems);
                if (result == -1)
                {
                    MessageBox.Show("Empty sheet!");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            ArrangeOrder();

            btnGo.Enabled = true;

        }

        private void cleanupBlogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_blogSelected)
            {
                MessageBox.Show("First connect to blog from File>Connect!");
                return;
            }

            var frmCleanUp = new frmCleanup();
            frmCleanUp.ShowDialog();

        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lvItems_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lvItems.SelectedItems.Count == 0)
            {
                lblSelection.Text = "";
                return;
            }
            if (lvItems.SelectedItems.Count > 50)
            {
                lblSelection.Text = string.Format("Selected {0} items", lvItems.SelectedItems.Count); ;
                return;
            }
            var totalWordCount = 0;
            foreach (ListViewItem selectedItem in lvItems.SelectedItems)
            {
                var wc = selectedItem.SubItems[10].Text;
                if (!string.IsNullOrEmpty(wc))
                {
                    totalWordCount += Int32.Parse(wc);
                }
            }
            lblSelection.Text = string.Format("Selected {0} items, total {1} words", lvItems.SelectedItems.Count, totalWordCount);
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveControl.GetType() == typeof(TextBox) || ActiveControl.GetType() == typeof(NumericUpDown))
            {
                return;
            }

            if (e.KeyCode == Keys.A && e.Control)
            {
                btnSelectAll_Click(null, null);
            }
        }

        private void fixFeatureImageErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_blogSelected)
            {
                MessageBox.Show("First connect to blog from File>Connect!");
                return;
            }
            var thumbnailMetaData = new Dictionary<int, string>();
            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();
            using (var dal = new Dal(MySqlConnectionString))
            {
                var postDal = new PostDal(dal);
                var allYoastMeta = postDal.GetAllPostMeta("_yoast_wpseo_focuskw_text_input");
                if (allYoastMeta.Tables.Count == 0) return;
                if (allYoastMeta.Tables[0].Rows.Count == 0) return;

                var thumbnailMeta = postDal.GetAllPostMeta("_thumbnail_id");
                if (thumbnailMeta.Tables.Count > 0)
                {
                    if (thumbnailMeta.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in thumbnailMeta.Tables[0].Rows)
                        {
                            var postId = Int32.Parse(row["post_id"].ToString());
                            var meta_value = row["meta_value"].ToString();
                            if (!thumbnailMetaData.ContainsKey(postId))
                            {
                                thumbnailMetaData.Add(postId, meta_value);
                            }
                        }
                    }
                }

                barStatus.Maximum = allYoastMeta.Tables[0].Rows.Count;
                barStatus.Visible = true;
                foreach (DataRow row in allYoastMeta.Tables[0].Rows)
                {
                    barStatus.PerformStep();

                    var postId = Int32.Parse(row["post_id"].ToString());
                    var meta_value = row["meta_value"].ToString();
                    if (thumbnailMetaData.ContainsKey(postId) && !string.IsNullOrEmpty(thumbnailMetaData[postId]))
                    {
                        continue;
                    }
                    Application.DoEvents();
                    SetStatus(string.Format("Fixing {0} ", postId));
                    postDal.SetPostMetaData(postId, "_thumbnail_id", meta_value);
                }

                barStatus.Visible = false;

            }
            MessageBox.Show("Finished");
        }

    }
}
