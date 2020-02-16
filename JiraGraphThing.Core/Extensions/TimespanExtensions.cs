using System;
using System.Text;

namespace JiraGraphThing.Core.Extensions
{
    public static class TimespanExtensions
    {
        public static string FormatCompact(this TimeSpan time)
        {
            var hours = time.Hours;
            var minutes = time.Minutes;

            var sb = new StringBuilder();
            if (hours > 0)
                sb.Append(hours).Append("h");
            if (minutes > 0)
                sb.Append(minutes).Append("m");

            return sb.ToString();
        }
    }
}
