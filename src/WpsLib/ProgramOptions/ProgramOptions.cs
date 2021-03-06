using System;

namespace WpsLib.ProgramOptions
{
    public class ProgramOptions
    {
        public int MergeBlockSize { get; set; }
        public int ThumbnailSize { get; set; }
        public bool ResizeImages { get; set; }
        public int ResizeSize { get; set; }
        public bool UseFtp { get; set; }
        public bool MakeFirstImageAsFeature { get; set; }
        public bool TagsAsText { get; set; }
        public bool UseCache { get; set; }
        public bool ShowMessageBoxes { get; set; }
        public bool ScrambleLeadPosts { get; set; }

        public string BlogUrl { get; set; }
        public string BlogHost { get
        {
            if(string.IsNullOrEmpty(BlogUrl))
            {
                return BlogUrl;
            }
            var uri = new Uri(BlogUrl);
            var result = uri.Host;
            if(result.ToLower().StartsWith("www."))
            {
                result = result.Replace("www.", "");
            }
            return result;

        }}
        public string BlogUser { get; set; }
        public string BlogPassword { get; set; }

        public string DatabaseUrl { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseUser { get; set; }
        public string DatabasePassword { get; set; }

        public string FtpUrl { get; set; }
        public string FtpUser { get; set; }
        public string FtpPassword { get; set; }

        public bool UseRemoteDownloading { get; set; }

        public int TitleContainsKeywordScore { get; set; }
        public int TitleStartsWithKeywordScore { get; set; }
        public int ContentContainsKeywordScore { get; set; }
        public int ContentFirst100ContainsKeywordScore { get; set; }
        public int KeywordRatioScore { get; set; }

        public int NonExactTitleContainsKeywordScore { get; set; }
        public int NonExactContentContainsKeywordScore { get; set; }
        public int NonExactKeywordRatioScore { get; set; }

        public string ProxyAddress { get; set; }
        public int ProxyPort { get; set; }
        public bool UseProxy { get; set; }

        public string YoutubeClient { get; set; }
        public string YoutubeClientSecret { get; set; }

        public bool SkipSearchingPosted { get; set; }

        public string PriceSign { get; set; }

    }
}