using System.Text.RegularExpressions;

namespace JiraGraphThing.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool NotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static string AlphaNumericOnly(this string s)
        {
            return Regex.Replace(s,@"[^\w]", "");
        }
    }
}
