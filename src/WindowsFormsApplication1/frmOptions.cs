using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PttLib;
using WordpressScraper.Helpers;
using WpsLib.ProgramOptions;

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
            settings.Add(new Tuple<string, string>("ScrambleLeadPosts", chkScrambleLeadPosts.Checked.ToString()));
            
            settings.Add(new Tuple<string, string>("TitleContainsKeywordScore", numTitleContainsKeyword.Value.ToString()));
            settings.Add(new Tuple<string, string>("TitleStartsWithKeywordScore", numTitleStartsKeyword.Value.ToString()));
            settings.Add(new Tuple<string, string>("ContentContainsKeywordScore", numContentContainsKeyword.Value.ToString()));
            settings.Add(new Tuple<string, string>("ContentFirst100ContainsKeywordScore", numFirst100Content.Value.ToString()));
            settings.Add(new Tuple<string, string>("KeywordRatioScore", numKeywordContentRatio.Value.ToString()));

            settings.Add(new Tuple<string, string>("NonExactTitleContainsKeywordScore", numNETitleContainsKeyword.Value.ToString()));
            settings.Add(new Tuple<string, string>("NonExactContentContainsKeywordScore", numNEContentContainsKeyword.Value.ToString()));
            settings.Add(new Tuple<string, string>("NonExactKeywordRatioScore", numNEKeywordContentRatio.Value.ToString()));
            settings.Add(new Tuple<string, string>("TagsAsText", chkTagsAsText.Checked.ToString()));
            settings.Add(new Tuple<string, string>("UseRemoteDownloading", chkUseRemoteDownloading.Checked.ToString()));
            settings.Add(new Tuple<string, string>("SkipSearchingPosted", chkSkipSearchingPosted.Checked.ToString()));
            settings.Add(new Tuple<string, string>("PriceSign", txtPriceSign.Text));
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

            FixEmptyNumericUpDown(numMerge);
            FixEmptyNumericUpDown(numThumbnailSize);
            FixEmptyNumericUpDown(numMaxImageDimension);
            FixEmptyNumericUpDown(numTitleContainsKeyword);
            FixEmptyNumericUpDown(numTitleStartsKeyword);
            FixEmptyNumericUpDown(numContentContainsKeyword);
            FixEmptyNumericUpDown(numFirst100Content);
            FixEmptyNumericUpDown(numKeywordContentRatio);
            FixEmptyNumericUpDown(numNEContentContainsKeyword);
            FixEmptyNumericUpDown(numNETitleContainsKeyword);
            FixEmptyNumericUpDown(numNEKeywordContentRatio);

            chkFeatureImage.Checked = options.MakeFirstImageAsFeature;
            chkTagsAsText.Checked = options.TagsAsText;
            numMerge.Value = options.MergeBlockSize;
            numThumbnailSize.Value = options.ThumbnailSize;
            chkResizeImages.Checked = options.ResizeImages;
            numMaxImageDimension.Value = options.ResizeSize;
            chkNoAPI.Checked = options.UseFtp;
            chkCache.Checked = options.UseCache;
            chkShowMessageBox.Checked = options.ShowMessageBoxes;
            chkScrambleLeadPosts.Checked = options.ScrambleLeadPosts;

            numTitleContainsKeyword.Value = options.TitleContainsKeywordScore;
            numTitleStartsKeyword.Value = options.TitleStartsWithKeywordScore;
            numContentContainsKeyword.Value = options.ContentContainsKeywordScore;
            numFirst100Content.Value = options.ContentFirst100ContainsKeywordScore;
            numKeywordContentRatio.Value = options.KeywordRatioScore;

            numNETitleContainsKeyword.Value = options.NonExactTitleContainsKeywordScore;
            numNEContentContainsKeyword.Value = options.NonExactContentContainsKeywordScore;
            numNEKeywordContentRatio.Value = options.NonExactKeywordRatioScore;
            chkUseRemoteDownloading.Checked = options.UseRemoteDownloading;
            chkSkipSearchingPosted.Checked = options.SkipSearchingPosted;
            txtPriceSign.Text = string.IsNullOrEmpty(options.PriceSign) ? "$":options.PriceSign;
            toolTip.SetToolTip(lblKeywordContentRatio, "For every percentage, give this much of score");
        }

        private void FixEmptyNumericUpDown(NumericUpDown control)
        {
            if (control.Text == "")
            {
                control.Text = "0";
            }
        }

        private void numMerge_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
