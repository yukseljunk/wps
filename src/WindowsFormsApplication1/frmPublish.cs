using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordpressScraper
{
    public partial class frmPublish : Form
    {
        public frmPublish()
        {
            InitializeComponent();
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            MessageBox.Show(cbCriteria.SelectedIndex.ToString());
        }

        private void chkCreateSlide_CheckedChanged(object sender, EventArgs e)
        {
            pnlSlideShow.Enabled = chkCreateSlide.Checked;
        }

        private void frmPublish_Load(object sender, EventArgs e)
        {
            cbCriteria.SelectedIndex = 0;
        }

        private void numNumberOfPosts_ValueChanged(object sender, EventArgs e)
        {
            numVideoPerPost.Maximum = numNumberOfPosts.Value;
        }
    }
}
