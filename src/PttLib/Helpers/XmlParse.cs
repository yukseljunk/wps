using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;

namespace PttLib.Helpers
{
    public class XmlParse
    {
        public static List<int> GetIntegerListNodeValue(HtmlNode htmlNode, string xpath)
        {
            var nodes = htmlNode.SelectNodes(xpath);
            if (nodes == null || !nodes.Any()) return null;
            var result = new List<int>();
            foreach (var node in nodes)
            {
                int parsedValue;
                if (Int32.TryParse(node.InnerText.Trim(), out parsedValue))
                {
                    result.Add(parsedValue);
                }

            }
            return result;
        }

        public static List<KeyValuePair<string,string>> GetKeyValueListNodeValue(HtmlNode htmlNode, string xpath)
        {
            var nodes = htmlNode.SelectNodes(xpath);
            if (nodes == null || !nodes.Any()) return null;
            var result = new List<KeyValuePair<string,string>>();
            foreach (var node in nodes)
            {
                var key=node.GetAttributeValue("key","");
                var value = node.GetAttributeValue("value", "");
                result.Add(new KeyValuePair<string, string>(key,value));
            }
            return result;
        }
        

        public static int GetIntegerNodeValue(HtmlNode htmlNode, string xpath, int defaultValue)
        {
            var node = htmlNode.SelectSingleNode(xpath);
            if (node == null) return defaultValue;

            int parsedValue;
            if (Int32.TryParse(node.InnerText.Trim(), out parsedValue))
            {
                return parsedValue;
            }
            return defaultValue;
        }
        public static T GetEnumNodeValue<T>(HtmlNode htmlNode, string xpath, T defaultValue) where T: struct
        {
            var node = htmlNode.SelectSingleNode(xpath);
            if (node == null) return defaultValue;

            T parsedValue;
            if (Enum.TryParse(node.InnerText.Trim(),true, out parsedValue))
            {
                return parsedValue;
            }
            return defaultValue;
        }

        public static List<T> GetEnumListNodeValue<T>(HtmlNode htmlNode, string xpath) where T : struct
        {
            var nodes = htmlNode.SelectNodes(xpath);
            if (nodes == null || !nodes.Any()) return null;
            var result = new List<T>();
            foreach (var node in nodes)
            {
                T parsedValue;
                if (Enum.TryParse(node.InnerText.Trim(), true, out parsedValue))
                {
                    result.Add(parsedValue);
                }

            }
            return result;
        }

        public static bool GetBooleanNodeValue(HtmlNode htmlNode, string xpath, bool defaultValue)
        {
            var node = htmlNode.SelectSingleNode(xpath);
            if (node == null) return defaultValue;
            bool parsedValue;
            if (bool.TryParse(node.InnerText.Trim().ToLower(), out parsedValue))
            {
                return parsedValue;
            }
            return defaultValue;
        }
        public static DateTime GetDateTimeNodeValue(HtmlNode htmlNode, string xpath, DateTime defaultValue, CultureInfo cultureInfo)
        {
            var node = htmlNode.SelectSingleNode(xpath);
            if (node == null) return defaultValue;

            DateTime parsedValue;
            if (DateTime.TryParse(node.InnerText.Trim(),cultureInfo, DateTimeStyles.None, out parsedValue))
            {
                return parsedValue;
            }
            return defaultValue;
        }
        public static string GetStringNodeValue(HtmlNode htmlNode, string xpath, string defaultValue, bool returnDefaultIfEmpty=false)
        {
            var node = htmlNode.SelectSingleNode(xpath);
            if (node == null) return defaultValue;
            var trimmed = node.InnerText.Trim();
            if (trimmed == "" && returnDefaultIfEmpty) return defaultValue;
            return trimmed;
        }
        public static List<string> GetStringListNodeValue(HtmlNode htmlNode, string xpath)
        {
            var nodes = htmlNode.SelectNodes(xpath);
            if (nodes == null || !nodes.Any()) return null;
            var result = new List<string>();
            foreach (var node in nodes)
            {
                result.Add(node.InnerText.Trim());
            }
            return result;
        }
    }
}
