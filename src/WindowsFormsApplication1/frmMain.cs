using System;
using System.Collections;
using System.Collections.Generic;
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
        private const string DefaultUrl = "https://www.etsy.com/search/accessories?q=baby+chair";
        private BlogCache _blogCache;
        private bool StopToken = false;
        private ListViewColumnSorter lvwColumnSorter;

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
            System.IO.Directory.CreateDirectory("Logs");
            this.Text += " v" + Assembly.GetExecutingAssembly().GetName().Version;
            lblDateTime.Text = "";
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
                var results = GetEtsyItems(txtUrl.Text, page, out pageCount);
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
            var itemIndex = lvItems.Items.Count + 1;
            Cursor.Current = Cursors.WaitCursor;
            var blockIndex = 0;

            do
            {
                var subResults = allResults.Skip(20 * blockIndex).Take(20);
                if (!subResults.Any()) break;
                blockIndex++;

                var listViewItems = new List<ListViewItem>();

                foreach (var etsyResult in subResults)
                {
                    Application.DoEvents();

                    var item = GetEtsyItem(etsyResult.Item1, etsyResult.Item2);
                    string[] row1 =
                        {
                            item.Id.ToString(), item.Url, item.Title, item.MetaDescription, item.Content,
                            item.Price.ToString(CultureInfo.GetCultureInfo("en-US")),
                            string.Join(",", item.Images), string.Join(",", item.Tags), ""
                        };

                    var listViewitem = new ListViewItem(itemIndex.ToString());
                    listViewitem.SubItems.AddRange(row1);
                    listViewItems.Add(listViewitem);
                    barStatus.PerformStep();
                    itemIndex++;
                    if (StopToken)
                    {
                        break;
                    }

                }

                lvItems.ListViewItemSorter = null;
                lvItems.BeginUpdate();
                lvItems.Items.AddRange(listViewItems.ToArray());
                lvItems.EndUpdate();
                if (StopToken)
                {
                    break;
                }
            } while (true);

            SetStatus("Ready");
            lvwColumnSorter = new ListViewColumnSorter();
            lvItems.ListViewItemSorter = lvwColumnSorter;

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

        private static IEnumerable<Tuple<string, string>> GetDawandaItems(string url, int page, out int pageCount)
        {
            var dawanda = new Dawanda();
            return page == 0 ? dawanda.GetItems(url, out pageCount) : dawanda.GetItems(url, out pageCount, page);
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
        private static Item GetDawandaItem(string title, string url)
        {
            var dawanda = new Dawanda();
            return dawanda.GetItem(title, url);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvItems.Items)
            {
                item.Selected = true;
            }
            lvItems.Focus();
        }


        private void btnGo_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select items to transfer!");
                return;
            }

            CreateAuthors();
            using (var dal = new Dal(MySqlConnectionString))
            {
                _blogCache = new BlogCache(SiteConfig, dal);

                EnDisItems(false);
                StopToken = false;
                bool errorFound = false;

                lblDateTime.Text = DateTime.Now.ToShortTimeString();
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
                var etsyFactory = new EtsyFactory(SiteConfig, _blogCache, dal, chkNoAPI.Checked);

                foreach (ListViewItem item in lvItems.SelectedItems)
                {
                    if (StopToken) break;
                    SetStatus("Creating item on the blog:" + item.Text);
                    Application.DoEvents();
                    Item itemObject = ItemFromListView(item);
                    var itemNo = etsyFactory.Create(itemObject, txtBlogUrl.Text, chkCache.Checked,
                        chkFeatureImage.Checked);
                    Application.DoEvents();
                    barStatus.PerformStep();

                    var status = "";
                    switch (itemNo)
                    {
                        case -1:
                            errorFound = true;
                            status = "Error";
                            break;
                        case 0:
                            status = "Exists";
                            break;
                        case -2:
                            status = "Invalid";
                            break;
                        default:
                            status = itemNo.ToString();
                            break;
                    }
                    item.SubItems[9].Text = status;
                    item.EnsureVisible();
                }
                SetStatus("Transfer finished" + (errorFound ? " with errors" : ""));
            }
            ResetBarStatus();
            EnDisItems(true);
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

        private Item ItemFromListView(ListViewItem item)
        {
            return new Item()
            {
                Id = int.Parse(item.SubItems[1].Text),
                Url = item.SubItems[2].Text,
                Title = item.SubItems[3].Text,
                MetaDescription = item.SubItems[4].Text,
                Content = item.SubItems[5].Text,
                Price = double.Parse(item.SubItems[6].Text, CultureInfo.InvariantCulture),
                Tags = item.SubItems[8].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                Images = item.SubItems[7].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)

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
            grpBlogProp.Enabled = enabled;
            grpMysql.Enabled = enabled;
            grpAuthors.Enabled = enabled;
            btnStopScrape.Enabled = enabled;
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

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvItems.Items)
            {
                foreach (ListViewItem item2 in lvItems.Items)
                {
                    if (item2.SubItems[1].Text == item.SubItems[1].Text && item2.Text != item.Text)
                    {
                        var color = Color.FromArgb(255, Helper.GetRandomNumber(1, 255), Helper.GetRandomNumber(1, 255), Helper.GetRandomNumber(1, 255));
                        item.BackColor = color;
                        item2.BackColor = color;
                        break;
                    }
                }
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
        }
    }

    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Compare the two items
            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

    }

}
