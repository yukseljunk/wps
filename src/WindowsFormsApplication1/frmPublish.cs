using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PttLib;
using WordpressScraper.Dal;

namespace WordpressScraper
{
    public partial class frmPublish : Form
    {
        private ProgramOptions _options;

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

        private void btnPublish_Click(object sender, EventArgs e)
        {
            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();

            var postDal = new PostDal(new Dal.Dal(MySqlConnectionString));
            var posts = postDal.GetPosts((PostOrder)cbCriteria.SelectedIndex, (int)numNumberOfPosts.Value);
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
            cbCriteria.SelectedIndex = 0;
            lblStatus.Text = "";
        }

        private void numNumberOfPosts_ValueChanged(object sender, EventArgs e)
        {
            numVideoPerPost.Maximum = numNumberOfPosts.Value;
        }
    }
}
