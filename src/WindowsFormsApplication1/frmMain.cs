using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            lvItems.Items.Clear();
            btnStart.Enabled = false;
            btnStopScrape.Enabled = true;
            btnGo.Enabled = false;
            StopToken = false;
            var itemIndex = 1;
            Cursor.Current = Cursors.WaitCursor;
            var etsyResults = GetEtsyItems(txtUrl.Text, (int)numPage.Value);
            foreach (var etsyResult in etsyResults)
            {
                var item = GetEtsyItem(etsyResult.Item1, etsyResult.Item2);
                string[] row1 = { item.Id.ToString(), item.Url, item.Title, item.MetaDescription, item.Content, item.Price.ToString(), string.Join(",", item.Images), string.Join(",", item.Tags), "" };
                lvItems.BeginUpdate();
                lvItems.Items.Add(itemIndex.ToString()).SubItems.AddRange(row1);
                lvItems.EndUpdate();
                Application.DoEvents();
                itemIndex++;
                if (StopToken)
                {
                    break;
                }
                //MessageBox.Show(item.ToString());
            }

            btnGo.Enabled = etsyResults.Any();

            btnStart.Enabled = true;
            Cursor.Current = Cursors.Default;
            btnStopScrape.Enabled = false;


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
            lvItems.Enabled = false;
            btnGo.Enabled = false;
            btnStop.Enabled = true;
            StopToken = false;
            bool errorFound = false;
            var dummy = IdsPresent;//lazy load post ids

            foreach (ListViewItem item in lvItems.SelectedItems)
            {
                Application.DoEvents();
                if (StopToken) break;
                var itemNo = CreateItem(item);
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

            MessageBox.Show("Transfer finished" + (errorFound ? " with errors" : ""), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnStop.Enabled = false;
            btnGo.Enabled = true;
            lvItems.Enabled = true;
            
        }

        private HashSet<string> _idsPresent = null;
        protected HashSet<string> IdsPresent
        {
            get
            {
                if (_idsPresent == null)
                {
                    _idsPresent = GetPostIds();
                }
                return _idsPresent;
            }
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
                    Thread.Sleep(1000);
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
                    var uploaded = client.UploadFile(Data.CreateFromUrl(item.SubItems[7].Text));
                    var type = uploaded.Type;
                    var allTags = client.GetTerms("post_tag", null);
                    var post = new Post
                                   {
                                       PostType = "post",
                                       Title = item.SubItems[3].Text,
                                       Content = item.SubItems[5].Text,
                                       PublishDateTime = DateTime.Now,
                                       CustomFields = new [] { new CustomField() { Key = "foreignkey", Value = id } },
                                       FeaturedImageId = uploaded.Id
                                   };

                    var terms = new List<Term>();
                    var tags = item.SubItems[8].Text;
                    var tagsSplitted = tags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tag in tagsSplitted)
                    {

                        var tagOnBlog =
                            allTags.FirstOrDefault(
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
                            terms.Add(t);
                        }
                        else
                        {
                            terms.Add(tagOnBlog);
                        }
                    }
                    post.Terms = terms.ToArray();
                    var newPost = client.NewPost(post);

                    _idsPresent.Add(id);
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
            if (postId == "" || postId == "Error" || postId=="Exists")
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
    }
}
