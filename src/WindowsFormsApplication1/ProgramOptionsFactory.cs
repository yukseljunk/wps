using System;

namespace WordpressScraper
{
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
                                  ShowMessageBoxes = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["ShowMessageBoxes"]),
                                  BlogUrl = System.Configuration.ConfigurationManager.AppSettings["BlogUrl"],
                                  BlogUser = System.Configuration.ConfigurationManager.AppSettings["BlogUser"],
                                  BlogPassword = System.Configuration.ConfigurationManager.AppSettings["BlogPassword"],
                                  DatabaseUrl = System.Configuration.ConfigurationManager.AppSettings["DatabaseUrl"],
                                  DatabaseName = System.Configuration.ConfigurationManager.AppSettings["DatabaseName"],
                                  DatabaseUser = System.Configuration.ConfigurationManager.AppSettings["DatabaseUser"],
                                  DatabasePassword = System.Configuration.ConfigurationManager.AppSettings["DatabasePassword"],
                                  FtpUrl = System.Configuration.ConfigurationManager.AppSettings["FtpUrl"],
                                  FtpUser = System.Configuration.ConfigurationManager.AppSettings["FtpUser"],
                                  FtpPassword = System.Configuration.ConfigurationManager.AppSettings["FtpPassword"]
                              };
            return options;
        }
    }
}