using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace WordpressScraper.Helpers
{
    public class ConfigurationHelper
    {
        public static void UpdateSettings(IList<Tuple<string, string>> keysValues)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (var keyValue in keysValues)
            {
                if (!configuration.AppSettings.Settings.AllKeys.Contains(keyValue.Item1))
                {
                    configuration.AppSettings.Settings.Add(new KeyValueConfigurationElement(keyValue.Item1,
                        keyValue.Item2));
                }
                else
                {
                    configuration.AppSettings.Settings[keyValue.Item1].Value = keyValue.Item2;
                }
            }
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");

        }

    }
}
