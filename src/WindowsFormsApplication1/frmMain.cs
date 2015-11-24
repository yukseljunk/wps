using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using PttLib;
using WordPressSharp;
using WordPressSharp.Models;

namespace WindowsFormsApplication1
{
    public partial class frmMain : Form
    {
        private const string DefaultUrl = "https://www.etsy.com/c/books-movies-and-music/music/instrument-straps";

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
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
            var pageEnd = (int)numPageTo.Value;
            ResetBarStatus(true);
            barStatus.Maximum = pageEnd - pageStart;
            for (var page = pageStart; page <= pageEnd; page++)
            {
                Application.DoEvents();
                lblStatus.Text = string.Format("Getting Page {0}", page);
                var results = GetEtsyItems(txtUrl.Text, (int)numPage.Value);
                barStatus.PerformStep();
                if (results == null)
                {
                    continue;
                }
                allResults.AddRange(results);
            }
            lblStatus.Text = "Ready";
            ResetBarStatus();
            if (!allResults.Any())
            {
                MessageBox.Show("No results found for url: " + txtUrl.Text);
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
            lblStatus.Text = "Filling items....";
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
            lblStatus.Text = "Ready";
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

        private static IEnumerable<Tuple<string, string>> GetEtsyItems(string url, int page)
        {
            var etsy = new Etsy();
            return page == 0 ? etsy.GetItems(url) : etsy.GetItems(url, page);
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
            lblStatus.Text = "Loading present posts in the blog(this may take some time)...";
            Application.DoEvents();
            var dummy = IdsPresent;//lazy load post ids
            Application.DoEvents();
            lblStatus.Text = "Loading present tags in the blog(this may take some time)...";            
            Application.DoEvents();
            var dummy2 = TagsPresent;//lazy load post ids
            Application.DoEvents();
            lblStatus.Text = "Ready";
            ResetBarStatus(true);
            barStatus.Maximum = lvItems.SelectedItems.Count;
            foreach (ListViewItem item in lvItems.SelectedItems)
            {
                if (StopToken) break;
                lblStatus.Text = "Creating item on the blog:"+ item.Text;
                Application.DoEvents();
                var itemNo = CreateItem(item);
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
            lblStatus.Text = "Transfer finished" + (errorFound ? " with errors" : "");
            ResetBarStatus();
            EnDisItems(true);
        }

        private void EnDisItems(bool enabled)
        {
            btnStop.Enabled = !enabled;
            btnGo.Enabled = enabled;
            lvItems.Enabled = enabled;
            grpBlogProp.Enabled = enabled;
        }

        private Dictionary<string, HashSet<string>> _idsPresent = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, HashSet<Term>> _tagsPresent = new Dictionary<string, HashSet<Term>>();

        protected HashSet<string> IdsPresent
        {
            get
            {
                if (!_idsPresent.ContainsKey(txtBlogUrl.Text))
                {
                    _idsPresent.Add(txtBlogUrl.Text, null);
                }
                if (_idsPresent[txtBlogUrl.Text] == null)
                {
                    _idsPresent[txtBlogUrl.Text] = GetPostIds();
                }
                return _idsPresent[txtBlogUrl.Text];
            }
        }

        protected HashSet<Term> TagsPresent
        {
            get
            {
                if (!_tagsPresent.ContainsKey(txtBlogUrl.Text))
                {
                    _tagsPresent.Add(txtBlogUrl.Text, null);
                }
                if (_tagsPresent[txtBlogUrl.Text] == null)
                {
                    _tagsPresent[txtBlogUrl.Text] = GetTags();
                }
                return _tagsPresent[txtBlogUrl.Text];
            }
        }

        private HashSet<Term> GetTags()
        {
            var result = new HashSet<Term>();
            using (var client = new WordPressClient(SiteConfig))
            {
                var tags=  client.GetTerms("post_tag", null);
                foreach (var tag in tags)
                {
                    result.Add(tag);
                }

            }
            return result;
        }

        private HashSet<string> GetPostIds()
        {
            var blockSize = 10;
            var result = new HashSet<string>();
            using (var client = new WordPressClient(SiteConfig))
            {
                for (var i = 0; i < 1000; i++)
                {
                    var posts =
                        client.GetPosts(new PostFilter() { Number = blockSize, Offset = blockSize * (i - 1) });
                    foreach (var post in posts)
                    {
                        var foreignKeyCustomField =
                            post.CustomFields.FirstOrDefault(cf => cf.Key == "foreignkey");
                        if (foreignKeyCustomField != null)
                        {
                            result.Add(foreignKeyCustomField.Value);
                        }

                    }
                    if (!posts.Any())
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
            }
            return result;
        }


        public WordPressSiteConfig SiteConfig
        {
            get
            {
                return new WordPressSiteConfig() { BaseUrl = txtBlogUrl.Text, BlogId = 1, Username = txtUserName.Text, Password = txtPassword.Text };
            }
        }

        private int CreateItem(ListViewItem item)
        {
            using (var client = new WordPressClient(SiteConfig))
            {
                try
                {
                    var id = "etsy_" + item.SubItems[1].Text;
                    if (IdsPresent.Contains(id))
                    {
                        return 0;
                    }
                    var content = new StringBuilder("<div style=\"width: 300px; margin-right: 10px;\">");
                    IList<UploadResult> imageUploads = new List<UploadResult>();
                    var imageUrls = item.SubItems[7].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var imageUrl in imageUrls)
                    {
                        var uploaded = client.UploadFile(Data.CreateFromUrl(imageUrl));
                        imageUploads.Add(uploaded);
                        var thumbnailUrl = Path.GetDirectoryName(uploaded.Url).Replace("http:\\", "http:\\\\").Replace("\\", "/") + "/" + Path.GetFileNameWithoutExtension(uploaded.Url) + "-150x150" + Path.GetExtension(uploaded.Url);

                        content.Append(
                            string.Format(
                            "<div style=\"width: 70px; float: left; margin-right: 15px; margin-bottom: 3px;\"><a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" width=\"70px\" height=\"70px\" title=\"{2}\" /></a></div>",
                            uploaded.Url, thumbnailUrl, item.SubItems[3].Text));
                    }
                    content.Append(string.Format("</div><h4>Price:${0}</h4>", item.SubItems[6].Text));
                    content.Append("<strong>Description: </strong>");
                    content.Append(item.SubItems[5].Text);

                    var post = new Post
                                   {
                                       PostType = "post",
                                       Title = item.SubItems[3].Text,
                                       Content = content.ToString(),
                                       PublishDateTime = DateTime.Now,
                                       CustomFields = new[] { new CustomField() { Key = "foreignkey", Value = id } }
                                   };

                    if (chkFeatureImage.Checked && imageUploads.Any())
                    {
                        post.FeaturedImageId = imageUploads[0].Id;
                    }

                    var terms = new List<Term>();
                    var tags = item.SubItems[8].Text;
                    var tagsSplitted = tags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tag in tagsSplitted)
                    {

                        var tagOnBlog =
                            TagsPresent.FirstOrDefault(
                                t => HttpUtility.HtmlDecode(t.Name).Trim().ToLowerInvariant() == HttpUtility.HtmlDecode(tag).Trim().ToLowerInvariant());
                        if (tagOnBlog == null)
                        {
                            var t = new Term
                                        {
                                            Name = tag,
                                            Description = tag,
                                            Slug = tag.Replace(" ", "_"),
                                            Taxonomy = "post_tag"
                                        };

                            var termId = client.NewTerm(t);
                            t.Id = termId;
                            _tagsPresent[txtBlogUrl.Text].Add(t);
                            terms.Add(t);
                        }
                        else
                        {
                            terms.Add(tagOnBlog);
                        }
                    }
                    post.Terms = terms.ToArray();
                    var newPost = client.NewPost(post);

                    _idsPresent[txtBlogUrl.Text].Add(id);
                    return Convert.ToInt32(newPost);

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }


            }
            return -1;

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
