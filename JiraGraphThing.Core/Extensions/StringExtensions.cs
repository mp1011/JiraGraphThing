namespace JiraGraphThing.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool NotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }
    }
}
