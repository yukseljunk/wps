﻿using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WordpressScraper.Helpers
{
    class PhpSerializer
    {
        public static string Serialize(IDictionary<string,string> array)
        {
            var result = new StringBuilder();
            result.AppendFormat("a:{0}:{{", array.Count);
            foreach (KeyValuePair<string, string> entry in array)
            {
                result.Append(Serialize(entry.Key));
                result.Append(Serialize(entry.Value));
            }
            result.AppendFormat("}}");
            return result.ToString();
        }

        public static string Serialize(IList<string> array)
        {
            var result = new StringBuilder();
            var indexor = 0;
            result.AppendFormat("a:{0}:{{", array.Count);
            foreach (var str in array)
            {
                result.AppendFormat("i:{0};", indexor);
                result.Append(Serialize(str));
                indexor++;
            }
            result.AppendFormat("}}");
            return result.ToString();
        }

        public static IList<string> Deserialize(string input)
        {
            var result = new List<string>();
            var r = new Regex("\"[^\"]*\"");
            foreach (Match match in r.Matches(input))
            {
                result.Add(match.Value.Replace("\"", ""));
            }
            return result;

        }

        public static string Serialize(string str)
        {
            return string.Format("s:{0}:\"{1}\";", str.Length, str);
        }
    }
}
