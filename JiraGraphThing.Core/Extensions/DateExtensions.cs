using System;

namespace JiraGraphThing.Core.Extensions
{
    public static class DateExtensions
    {
        public static DateTime GetMorningTime(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day);
        }

        public static TimeSpan TimeSince(this DateTime time)
        {
            return DateTime.Now - time;
        }
    }
}
