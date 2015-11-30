namespace PttLib.Helpers
{
    public static class Extensions
    {
        public static string Surround(this string input, string surround = ",")
        {
            return surround+input+surround;
        }

        public static string EscapeSql(this string input)
        {
            return input.Replace("'", "''").Replace("\\", "\\\\");
        }


        public static string TrimMiddle(this string input, string leftToken, string rightToken, bool includeTokens = false)
        {
            var result = input;
            var leftTokenStartIndex = result.IndexOf(leftToken);
            if (leftTokenStartIndex == -1) return result;
            var rightTokenStartIndex = result.IndexOf(rightToken, leftTokenStartIndex);
            if (rightTokenStartIndex == -1) return result;
            return includeTokens ? result.Substring(leftTokenStartIndex, rightTokenStartIndex + rightToken.Length - leftTokenStartIndex) : result.Substring(leftTokenStartIndex + leftToken.Length, rightTokenStartIndex - leftTokenStartIndex - leftToken.Length);
        }
        public static string TrimMiddleWithLeftCalc(this string input, string leftToken, string rightToken)
        {
            var result = input;
            var leftTokenStartIndex = result.IndexOf(leftToken);
            if (leftTokenStartIndex == -1) return result;
            var rightTokenStartIndex = result.IndexOf(rightToken, leftTokenStartIndex + leftToken.Length);
            if (rightTokenStartIndex == -1) return result;
            return result.Substring(leftTokenStartIndex + leftToken.Length, rightTokenStartIndex - leftTokenStartIndex - leftToken.Length);
        }

    }
}
