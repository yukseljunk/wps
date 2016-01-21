using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PttLib;
using WordpressScraper.Dal;
using WordPressSharp.Models;

namespace WordpressScraper
{
    //TODO: kategori bos geliyor
    //TODO: 3 kelimede relevancy patliyor diyor atik
    //TODO: title indeksleme hatali


    public partial class frmPublish : Form
    {
        private ProgramOptions _options;
        private IList<Post> _posts;

        public frmPublish()
        {
            InitializeComponent();
        }

        private string MySqlConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3}; Allow User Variables=True", _options.DatabaseUrl, _options.DatabaseName, _options.DatabaseUser, _options.DatabasePassword);
            }
        }

        public IList<Post> Posts
        {
            get { return _posts; }
            set
            {
                _posts = value;
            }
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();

            var postDal = new PostDal(new Dal.Dal(MySqlConnectionString));
            var posts = _posts;
            if (_posts == null)
            {
                posts = postDal.GetPosts((PostOrder) cbCriteria.SelectedIndex, (int) numNumberOfPosts.Value);
            }
            else
            {
                posts = postDal.GetPosts(_posts.Select(p => int.Parse(p.Id)).ToList());

            }
            if (posts == null)
            {
                return;
            }

            foreach (var post in posts)
            {
                lblStatus.Text = string.Format("Publishing '{0}'", post.Title);
                Application.DoEvents();
                postDal.PublishPost(post);
            }

            lblStatus.Text = "Done";
        }

        private void chkCreateSlide_CheckedChanged(object sender, EventArgs e)
        {
            pnlSlideShow.Enabled = chkCreateSlide.Checked;
        }

        private void frmPublish_Load(object sender, EventArgs e)
        {
            cbCriteria.Items.Clear();           
            cbCriteria.Items.AddRange(new object[] { "Newest", "Oldest", "Random"});
            cbCriteria.SelectedIndex = 0;
            lblStatus.Text = "";
            if (_posts != null)
            {
                cbCriteria.Items.Add("Selected Items");
                cbCriteria.Enabled = false;
                cbCriteria.SelectedIndex = 3;
                numNumberOfPosts.Enabled = false;
                numNumberOfPosts.Value = _posts.Count;
            }
        }

        private void numNumberOfPosts_ValueChanged(object sender, EventArgs e)
        {
            numVideoPerPost.Maximum = numNumberOfPosts.Value;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch
            {
                
            }
        }
    }
}
