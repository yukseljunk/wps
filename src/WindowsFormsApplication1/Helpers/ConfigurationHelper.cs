using System;
using System.Collections.Generic;
using System.Configuration;

namespace WordpressScraper.Helpers
{
    public class ConfigurationHelper
    {
        public static void UpdateSettings(IList<Tuple<string, string>> keysValues)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (var keyValue in keysValues)
            {
                configuration.AppSettings.Settings[keyValue.Item1].Value = keyValue.Item2;
            }
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");

        }

    }
}
