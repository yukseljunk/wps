using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace PttLib.TourInfo
{
    public class ConverterFunctions
    {
        public DateTime FormatDateTime(string input)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            string format = "dd.MM.yyyy";
            return DateTime.ParseExact(input, format, provider);
        }

        public DateTime FormatDateTimeWhenYearNotSpecified(string input)
        {
            return FormatDateTime(input + "." + DateTime.Now.Year);
        }
        
        public string RemoveNewLine(string input)
        {
            return input.Replace("\r", "").Replace("\n", "");
        }

        public string RemoveNewLineDoubleSlash(string input)
        {
            return input.Replace("\\r", "").Replace("\\n", "");
        }

        public string RemoveBackSlash(string input)
        {
            return input.Replace("\\", "");
        }

        public string RemoveTab(string input)
        {
            return input.Replace("\t", "");
        }
        public string RemoveStars(string input)
        {
            return input.Replace("*", "");
        }
        public string Trim(string input)
        {
            return input.Trim();
        }
        public string BeforeSlash(string input)
        {
            return input.Split('/')[0];
        }
        public string AfterSlash(string input)
        {
            return input.Split('/')[1];
        }
        public string BeforeStar(string input)
        {
            return input.Split('*')[0];
        }
        public string AfterStar(string input)
        {
            return input.Split('*')[1];
        }
        public string RemoveNbsp(string input)
        {
            return input.Replace("&nbsp;", "");
        }
        public string RemoveMoneyUnits(string input)
        {
            return input.Replace(" USD", "").Replace(" EUR", "").Replace("USD", "").Replace("EUR", "").Replace("€", "").Replace("$", "");
        }
        public Double ToDouble(string input)
        {
            return Convert.ToDouble(input, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));
        }

        public string RemoveBrAndAfter(string input)
        {
            var brIndex = input.IndexOf("<br",StringComparison.InvariantCultureIgnoreCase);
            if (brIndex < 0) return input;
            return input.Substring(0, brIndex);

        }

        public string SeoUrl(string input)
        {
            var result = input.Replace("ö", "o").Replace("ş", "s").Replace("ı", "i").Replace("ü", "u").Replace("ğ", "g").Replace("ç", "c").Replace("İ", "I").Replace("Ş", "S").Replace("Ğ", "G")
                .Replace("Ü", "U").Replace("Ö", "O").Replace("Ç", "C").Replace("&", "").Replace("<", "").Replace(">", "").Replace("+", "").Replace(" ", "");
            result = Regex.Replace(result, @"[^a-z0-9]+", "", RegexOptions.IgnoreCase);
            return result.ToLower();
        }

        public string SeoUrl(string input, int maxLength)
        {
            var seoUrl = SeoUrl(input);
            if (seoUrl.Length > maxLength) return seoUrl.Substring(0, maxLength-1);
            return seoUrl;
        }

        public string StripTags(HtmlNode documentNode, IList<string> keepTags)
        {
            var result = new StringBuilder();
            foreach (var childNode in documentNode.ChildNodes)
            {
                if (childNode.Name.ToLower() == "#text")
                {
                    result.Append(childNode.InnerText);
                }
                else
                {
                    if (!keepTags.Contains(childNode.Name.ToLower()))
                    {
                        result.Append(StripTags(childNode, keepTags));
                    }
                    else
                    {
                        result.Append(childNode.OuterHtml.Replace(childNode.InnerHtml, StripTags(childNode, keepTags)));
                    }
                }
            }
            return result.ToString();
        }

       
    }
}
