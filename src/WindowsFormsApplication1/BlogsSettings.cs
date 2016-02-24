using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PttLib;
using PttLib.Helpers;
using WpsLib.ProgramOptions;

namespace WordpressScraper
{
    public class BlogsSettings
    {
        public static string ProgramSettingsFolder
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WordpressScraper");
            }
        }

        public static string SitesListFile
        {
            get
            {
                return ProgramSettingsFolder + "/sites.txt";
            }
        }

        public static void CheckCreateSitesListFile()
        {
            if (File.Exists(SitesListFile)) return;
            if (!Directory.Exists(ProgramSettingsFolder))
            {
                Directory.CreateDirectory(ProgramSettingsFolder);
            }
            using (File.Create(SitesListFile)) { }
        }

        public static ProgramOptions ProgramOptionsForBlog(string blog)
        {
            var fileName = BlogsSettings.ProgramSettingsFolder + "/" + blog + ".xml";
            var programOptionsFactory= new ProgramOptionsFactory();
            var programOptions = new ProgramOptions();
            if (File.Exists(fileName))
            {
                programOptions = programOptionsFactory.Get(fileName);
            }
            return programOptions;
        }

        public static List<string> Sites
        {
            get
            {
                if (!File.Exists(SitesListFile)) return new List<string>();
                return File.ReadAllLines(SitesListFile).ToList();
            }

        }

        public static  void UpdateSitesFile(List<string> sites)
        {
            if (!File.Exists(SitesListFile)) return;
            File.WriteAllText(SitesListFile, String.Empty);
            File.WriteAllLines(SitesListFile, sites);
        }

        public static void SaveSites(Dictionary<string, ProgramOptions> sitesSettings)
        {
            var files = Directory.GetFiles(ProgramSettingsFolder, "*.xml");
            foreach (var file in files)
            {
                File.Delete(file);
            }
            foreach (KeyValuePair<string, ProgramOptions> siteSetting in sitesSettings)
            {
                var fileName = ProgramSettingsFolder + "/" + siteSetting.Key + ".xml";
                var xmlSerializer = new XmlSerializer();
                xmlSerializer.Serialize(fileName, siteSetting.Value);
            }
        }
    }
}