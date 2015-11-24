using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PttLib;
using WordPressSharp;

namespace WindowsFormsApplication1
{
    public partial class frmMain : Form
    {
        private const string DefaultUrl = "https://www.etsy.com/c/books-movies-and-music/music/instrument-straps";
        private BlogCache _blogCache;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _blogCache=new BlogCache(SiteConfig);
            txtUrl.Text = DefaultUrl;
            btnGo.Enabled = false;
            btnStop.Enabled = false;
            btnStopScrape.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtUrl.Text == "")
            {
                MessageBox.Show("Enter Url!");
                return;
            }
            numPage_ValueChanged(null, null);
            var allResults = new List<Tuple<string, string>>();
            var pageStart = (int)numPage.Value;
            var pageEnd = chkAllPages.Checked ? (int)numPageTo.Maximum : (int)numPageTo.Value;
            ResetBarStatus(true);
            barStatus.Maximum = pageEnd - pageStart;
            for (var page = pageStart; page <= pageEnd; page++)
            {
                Application.DoEvents();
                SetStatus(string.Format("Getting Page {0}", page));
                int pageCount;
                var results = GetEtsyItems(txtUrl.Text, (int)numPage.Value, out pageCount);
                if (page > pageCount)
                {
                    break;
                }
                barStatus.PerformStep();
                if (results == null)
                {
                    continue;
                }
                allResults.AddRange(results);
            }
            SetStatus("Ready");
            ResetBarStatus();
            if (!allResults.Any())
            {
                MessageBox.Show(string.Format("No results found for url {0} for pages {1}-{2} ", txtUrl.Text, numPage.Value, numPageTo.Value));
                return;
            }

            if (chkClearResults.Checked)
            {
                lvItems.Items.Clear();
            }
            btnStart.Enabled = false;
            btnStopScrape.Enabled = true;
            btnGo.Enabled = false;
            StopToken = false;
            ResetBarStatus(true);
            barStatus.Maximum = allResults.Count;
            SetStatus("Filling items....");
            var itemIndex = 1;
            Cursor.Current = Cursors.WaitCursor;

            foreach (var etsyResult in allResults)
            {
                Application.DoEvents();
                var item = GetEtsyItem(etsyResult.Item1, etsyResult.Item2);
                string[] row1 = { item.Id.ToString(), item.Url, item.Title, item.MetaDescription, item.Content, item.Price.ToString(), string.Join(",", item.Images), string.Join(",", item.Tags), "" };
                lvItems.BeginUpdate();
                lvItems.Items.Add(itemIndex.ToString()).SubItems.AddRange(row1);
                lvItems.EndUpdate();
                barStatus.PerformStep();
                itemIndex++;
                if (StopToken)
                {
                    break;
                }
                //MessageBox.Show(item.ToString());
            }
            SetStatus("Ready");
            ResetBarStatus();
            btnGo.Enabled = allResults.Any();

            btnStart.Enabled = true;
            Cursor.Current = Cursors.Default;
            btnStopScrape.Enabled = false;


        }

        private void ResetBarStatus(bool visible = false)
        {
            barStatus.Value = 0;
            barStatus.Visible = visible;
        }

        private static IEnumerable<Tuple<string, string>> GetEtsyItems(string url, int page, out int pageCount)
        {
            var etsy = new Etsy();
            return page == 0 ? etsy.GetItems(url, out pageCount) : etsy.GetItems(url, out pageCount, page);
        }

        private static Item GetEtsyItem(string title, string url)
        {
            var etsy = new Etsy();
            return etsy.GetItem(title, url);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvItems.Items)
            {
                item.Selected = true;
            }
            lvItems.Focus();
        }

        private bool StopToken = false;
        private void btnGo_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select items to transfer!");
                return;
            }
            EnDisItems(false);
            StopToken = false;
            bool errorFound = false;
           
            if (chkCache.Checked)
            {
                SetStatus("Loading present posts in the blog(this may take some time)...");
                Application.DoEvents();
                _blogCache.Start(txtBlogUrl.Text);
                Application.DoEvents();
            }

            SetStatus("Ready");
            ResetBarStatus(true);
            barStatus.Maximum = lvItems.SelectedItems.Count;
            var etsyFactory = new EtsyFactory(SiteConfig,_blogCache);

            foreach (ListViewItem item in lvItems.SelectedItems)
            {
                if (StopToken) break;
                SetStatus("Creating item on the blog:" + item.Text);
                Application.DoEvents();
                Item itemObject = ItemFromListView(item);
                var itemNo = etsyFactory.Create(itemObject, txtBlogUrl.Text, chkCache.Checked, chkFeatureImage.Checked);
                Application.DoEvents();
                barStatus.PerformStep();
                if (itemNo == -1)
                {
                    errorFound = true;
                    item.SubItems[9].Text = "Error";
                }
                else if (itemNo == 0)
                {
                    item.SubItems[9].Text = "Exists";
                }
                else
                {
                    item.SubItems[9].Text = itemNo.ToString();
                }
            }
            SetStatus("Transfer finished" + (errorFound ? " with errors" : ""));
            ResetBarStatus();
            EnDisItems(true);
        }

        private Item ItemFromListView(ListViewItem item)
        {
            return new Item()
            {
                Id = int.Parse(item.SubItems[1].Text),
                Url = item.SubItems[2].Text,
                Title = item.SubItems[3].Text,
                MetaDescription = item.SubItems[4].Text,
                Content = item.SubItems[5].Text,
                Price = double.Parse(item.SubItems[6].Text),
                Tags = item.SubItems[7].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                Images = item.SubItems[8].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)

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
            lvItems.Enabled = enabled;
            grpBlogProp.Enabled = enabled;
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
            StopToken = true;
            btnStop.Enabled = false;
        }

        private void btnStopScrape_Click(object sender, EventArgs e)
        {
            StopToken = true;
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
            var postId = item.SubItems[9].Text;
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
    }
}
