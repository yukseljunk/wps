using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using PttLib;
using PttLib.Helpers;
using PttLib.TourInfo;
using WpsLib.Item;
using WpsLib.ProgramOptions;

namespace WordpressScraper
{
    class RelevanceCalculator
    {
        private ProgramOptions _programOptions = null;

        public RelevanceCalculator()
        {
            var programOptionsFactory = new ProgramOptionsFactory();
            _programOptions = programOptionsFactory.Get();
        }

        public int GetRelevance(Item item, string keyword)
        {
            var titleContainsKeywordScore = _programOptions.TitleContainsKeywordScore;
            var titleStartsWithKeywordScore = _programOptions.TitleStartsWithKeywordScore;
            var contentContainsKeywordScore = _programOptions.ContentContainsKeywordScore;
            var contentFirst100ContainsKeywordScore = _programOptions.ContentFirst100ContainsKeywordScore;
            var keywordRatioScore = _programOptions.KeywordRatioScore;

            var score = 0;
            if (Regex.IsMatch(item.Title, keyword, RegexOptions.IgnoreCase))
            {
                score += titleContainsKeywordScore;
            }
            if (item.Title.StartsWith(keyword, true, CultureInfo.InvariantCulture))
            {
                score += titleStartsWithKeywordScore;
            }
            if (Regex.IsMatch(item.Content, keyword, RegexOptions.IgnoreCase))
            {
                score += contentContainsKeywordScore;
            }
            if (item.Content.Length > 100 && Regex.IsMatch(item.Content.Substring(0, 100), keyword, RegexOptions.IgnoreCase))
            {
                score += contentFirst100ContainsKeywordScore;
            }
            if (KeywordRatio(item, keyword) >= 1)
            {
                score += keywordRatioScore;
            }

            score += GetRelevanceForNonExactMatch(item, keyword);

            if (item.Site == "Bonanza")
            {
                var imageCount = item.Content.ToLower()
                    .Split(new string[] {"<img "}, StringSplitOptions.RemoveEmptyEntries);
                var cf = new ConverterFunctions();
                var stripped = cf.StripTags(item.Content, new List<string>() { "font", "p", "span", "div", "h2", "h3", "h4", "tr", "td" });
                int rate = (int)((stripped.Length / (double)item.Content.Length) * 100);
                rate -= imageCount.Length * 5;
                if (rate < 10) score = -1;
            }

            return score;
        }

        private int GetRelevanceForNonExactMatch(Item item, string keyword)
        {
            var nonExactTitleContainsKeywordScore = _programOptions.NonExactTitleContainsKeywordScore;
            var nonExactContentContainsKeywordScore = _programOptions.NonExactContentContainsKeywordScore;
            var nonExactKeywordRatioScore = _programOptions.NonExactKeywordRatioScore;
            var score = 0;
            var keywords = keyword.Split(new [] {" "}, StringSplitOptions.RemoveEmptyEntries);
            
            if(NonExactMatch(item.Title, keywords))
            {
                score += nonExactTitleContainsKeywordScore;
            }
            if (NonExactMatch(item.Content, keywords))
            {
                score += nonExactContentContainsKeywordScore;
            }
            var minRatio = 0;
            foreach (var kw in keywords)
            {
                var ratio = KeywordRatio(item, kw);
                if (minRatio > ratio)
                {
                    minRatio = ratio;
                }
            }
            if (minRatio >= 1)
            {
                score += nonExactKeywordRatioScore;
            }
            return score;
        }

        private static bool NonExactMatch(string input, string[] keywords)
        {
            var titleMatches = true;
            foreach (var kw in keywords)
            {
                if (!Regex.IsMatch(input, kw, RegexOptions.IgnoreCase))
                {
                    titleMatches = false;
                }
            }
            return titleMatches;
        }


        public int KeywordRatio(Item item, string keyword)
        {
            int occurenceCount = new Regex(Regex.Escape(keyword)).Matches(item.Content).Count;
            var contentWordCount = item.Content.WordCount();
                return (int)((occurenceCount / (double)contentWordCount) * 100);
        }

    }
}