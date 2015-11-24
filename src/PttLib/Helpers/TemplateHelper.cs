using System;
using System.Collections.Generic;

namespace PttLib.Helpers
{
    enum TemplateTokenType
    {
        HOTELID,
        HOTELIDMINUS1,
        HOTELCOMMONNAME,
        PAGENO,
        PAGENO5,
        PAGENO25,
        PAGENO100,
        CAPTCHA,
        URL,
        LASTPRICE,
        STARTDATESHORT,
        ENDDATESHORT,
        NEXTPAGEURL,
        RANDOM30,
        RANDOM100
    }

    internal class TemplateHelper
    {
        private const string TokenFormat = "{{{0}}}";

        private Dictionary<TemplateTokenType, string> _defaultReplacements = new Dictionary<TemplateTokenType, string>()
                                                                                 {
                                                                                     {
                                                                                         TemplateTokenType.RANDOM30,new Random().Next(1, 30).ToString()
                                                                                     },
                                                                                     {
                                                                                         TemplateTokenType.RANDOM100,new Random().Next(1, 100).ToString()
                                                                                     }
                                                                                 };

        public string ReplaceTokens(string input, IDictionary<TemplateTokenType, string> replacements)
        {
            var result = input;
            foreach (var replacement in _defaultReplacements)
            {
                result = result.Replace(string.Format(TokenFormat, replacement.Key.ToString("G")), replacement.Value);
            }
            foreach (var replacement in replacements)
            {
                result = result.Replace(string.Format(TokenFormat, replacement.Key.ToString("G")), replacement.Value);
            }
            return result;
        }
    }
}