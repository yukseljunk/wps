using System;
using PttLib.Helpers;

namespace PttLib
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

        public ProgramOptions Get(string configPath)
        {
            var html = new HtmlAgilityPack.HtmlDocument();
            html.Load(configPath);

            var options = new ProgramOptions();
            options.BlogUrl = XmlParse.GetStringNodeValue(html.DocumentNode, "/programoptions/blogurl", "", true);
            options.BlogUser = XmlParse.GetStringNodeValue(html.DocumentNode, "/programoptions/bloguser", "", true);
            options.BlogPassword = XmlParse.GetStringNodeValue(html.DocumentNode, "/programoptions/blogpassword", "", true);
            options.DatabaseUrl = XmlParse.GetStringNodeValue(html.DocumentNode, "/programoptions/databaseurl", "", true);
            options.DatabaseName = XmlParse.GetStringNodeValue(html.DocumentNode, "/programoptions/databasename", "", true);
            options.DatabaseUser = XmlParse.GetStringNodeValue(html.DocumentNode, "/programoptions/databaseuser", "", true);
            options.DatabasePassword = XmlParse.GetStringNodeValue(html.DocumentNode, "/programoptions/databasepassword", "", true);

            options.FtpUrl = XmlParse.GetStringNodeValue(html.DocumentNode, "/programoptions/ftpurl", "", true);
            options.FtpUser = XmlParse.GetStringNodeValue(html.DocumentNode, "/programoptions/ftpuser", "", true);
            options.FtpPassword = XmlParse.GetStringNodeValue(html.DocumentNode, "/programoptions/ftppassword", "", true);
            
            return options;
        }
    }
}