using System;

namespace WordpressScraper
{
    public class ProgramOptions
    {
        public int MergeBlockSize { get; set; }
        public int ThumbnailSize { get; set; }
        public bool ResizeImages { get; set; }
        public int ResizeSize { get; set; }
        public bool UseFtp { get; set; }
        public bool MakeFirstImageAsFeature { get; set; }
        public bool UseCache { get; set; }
        public bool ShowMessageBoxes { get; set; }
    }

    public class ProgramOptionsFactory
    {
        public ProgramOptions Get()
        {
            var options = new ProgramOptions()
                              {
                                  MakeFirstImageAsFeature =bool.Parse(System.Configuration.ConfigurationManager.AppSettings["MakeFirstImageAsFeature"]),
                                  MergeBlockSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["MergeBlockSize"]),
                                  ThumbnailSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["ThumbnailSize"]),
                                  ResizeImages = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["ResizeImages"]),
                                  ResizeSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["ResizeSize"]),
                                  UseFtp = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseFtp"]),
                                  UseCache = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseCache"]),
                                  ShowMessageBoxes = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["ShowMessageBoxes"])
                              };
            return options;
        }
    }
}