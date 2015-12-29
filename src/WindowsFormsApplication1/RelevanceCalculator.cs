using System.Globalization;
using System.Text.RegularExpressions;
using PttLib;
using PttLib.Helpers;

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
            score += KeywordRatio(item, keyword) * keywordRatioScore;
            return score;
        }

       

        public int KeywordRatio(Item item, string keyword)
        {
            int occurenceCount = new Regex(Regex.Escape(keyword)).Matches(item.Content).Count;
            var contentWordCount = item.Content.WordCount();
                return (int)((occurenceCount / (double)contentWordCount) * 100);
        }

    }
}