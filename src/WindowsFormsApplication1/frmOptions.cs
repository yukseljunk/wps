using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordpressScraper.Helpers;

namespace WordpressScraper
{
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var settings = new List<Tuple<string, string>>();
            settings.Add(new Tuple<string, string>("MakeFirstImageAsFeature", chkFeatureImage.Checked.ToString()));
            settings.Add(new Tuple<string, string>("MergeBlockSize", numMerge.Value.ToString()));
            settings.Add(new Tuple<string, string>("ThumbnailSize", numThumbnailSize.Value.ToString()));
            settings.Add(new Tuple<string, string>("ResizeImages", chkResizeImages.Checked.ToString()));
            settings.Add(new Tuple<string, string>("ResizeSize", numMaxImageDimension.Value.ToString()));
            settings.Add(new Tuple<string, string>("UseFtp", chkNoAPI.Checked.ToString()));
            settings.Add(new Tuple<string, string>("UseCache", chkCache.Checked.ToString()));
            settings.Add(new Tuple<string, string>("ShowMessageBoxes", chkShowMessageBox.Checked.ToString()));
            ConfigurationHelper.UpdateSettings(settings);
            this.Dispose();
            this.Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            var programOptionsFactory = new ProgramOptionsFactory();
            var options = programOptionsFactory.Get();

            chkFeatureImage.Checked = options.MakeFirstImageAsFeature;
            numMerge.Value = options.MergeBlockSize;
            numThumbnailSize.Value = options.ThumbnailSize;
            chkResizeImages.Checked = options.ResizeImages;
            numMaxImageDimension.Value = options.ResizeSize;
            chkNoAPI.Checked = options.UseFtp;
            chkCache.Checked = options.UseCache;
            chkShowMessageBox.Checked = options.ShowMessageBoxes;
        }

    }
}
