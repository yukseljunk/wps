using System;
using System.Collections.Generic;
using System.Globalization;
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
            var brIndex = input.IndexOf("<br", StringComparison.InvariantCultureIgnoreCase);
            if (brIndex < 0) return input;
            return input.Substring(0, brIndex);

        }

        public string SeoUrl(string input)
        {
            var result = input.Replace("ö", "o").Replace("ş", "s").Replace("ı", "i").Replace("ü", "u").Replace("ğ", "g")
                .Replace("ç", "c").Replace("İ", "I").Replace("Ş", "S").Replace("Ğ", "G")
                .Replace("Ü", "U").Replace("Ö", "O").Replace("Ç", "C").Replace("&", "").Replace("<", "").Replace(">", "")
                .Replace("+", "");
            result = Regex.Replace(result, @"[^a-z 0-9]+", "", RegexOptions.IgnoreCase);
            result = RemoveDiacritics(result);
            return result.ToLower().Replace(" ", "-");
        }

        public string SeoPostUrl(string input)
        {
            var result = input.Replace("-", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");
            result = result.Replace("ö", "o").Replace("ş", "s").Replace("ı", "i").Replace("ü", "u").Replace("ğ", "g")
                .Replace("ç", "c").Replace("İ", "I").Replace("Ş", "S").Replace("Ğ", "G")
                .Replace("Ü", "U").Replace("Ö", "O").Replace("Ç", "C").Replace("&", "").Replace("<", "").Replace(">", "")
                .Replace("+", "");
            result = Regex.Replace(result, @"[^a-z 0-9]+", "", RegexOptions.IgnoreCase);
            result = RemoveDiacritics(result);
            result = result.Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");
            return result.ToLower().Trim().Replace(" ", "-");
        }

        public string SeoUrl(string input, int maxLength)
        {
            var seoUrl = SeoUrl(input);
            if (seoUrl.Length > maxLength) return seoUrl.Substring(0, maxLength - 1);
            return seoUrl;
        }

        public string StripTags(string input, IList<string> keepTags)
        {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(input);

            return StripTags(htmlDoc.DocumentNode, keepTags);
        }

        public string RemoveTags(string input, IList<string> tagsToRemove)
        {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(input);

            return RemoveTags(htmlDoc.DocumentNode, tagsToRemove);
        }

        private string RemoveTags(HtmlNode documentNode, IList<string> tagsToRemove)
        {
            foreach (var tag in tagsToRemove)
            {
                var tagNodes = documentNode.SelectNodes("//" + tag);
                if(tagNodes==null)
                {
                    continue;
                }
                foreach (var item in documentNode.SelectNodes("//" + tag))
                {
                    item.Remove();
                }

            }

            return documentNode.OuterHtml;
        }

        private string StripTags(HtmlNode documentNode, IList<string> keepTags)
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
                        if (childNode.InnerHtml == "")
                        {
                            result.Append(childNode.OuterHtml);
                        }
                        else
                        {
                            result.Append(childNode.OuterHtml.Replace(childNode.InnerHtml,
                                                                      StripTags(childNode, keepTags)));
                        }
                    }
                }
            }
            return result.ToString();
        }

        public string FirstNWords(string input, int number, bool stripParantheses = false)
        {
            if (input.Length <= number)
            {
                return input;
            }
            var result = input.Trim();
            if (stripParantheses)
            {
                result = result.Replace("(", " ").Replace(")", " ").Replace("[", " ").Replace("]", " ").Replace("{", " ").Replace("}", " ");
            }

            var nextSpaceIndex = result.IndexOf(' ', number);
            if (nextSpaceIndex == -1) return input;
            result = result.Substring(0, nextSpaceIndex);


            return result;
        }

        public string ArrangeContent(string input)
        {
            input = RemoveTags(input, new List<string>() { "a" });
            input = StripTags(input, new List<string>() {"table", "th", "tr", "td", "ul", "li", "p", "br", "strong"});
            input = input.Replace("{", "{{").Replace("}", "}}");
            return input;
        }

        public string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
