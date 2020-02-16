using System;

namespace JiraDataLayer.Models
{
    public class DailyWorkHistory
    {
        public DateTime Date { get; }
        public TimeSpan TimeSpent { get; }
        public TimeSpan TimeSpentSoFar { get; }
        public TimeSpan TimeEstimated { get; }
        public TimeSpan TotalTimeSpent { get; }

        public TimeSpan MaxSpentOrEstimated => TimeSpan.FromMinutes(Math.Max(TimeEstimated.TotalMinutes, TotalTimeSpent.TotalMinutes));

        public DailyWorkHistory(DateTime date, TimeSpan timeSpent, TimeSpan timeSpentSoFar, TimeSpan timeEstimated, TimeSpan totalTimeSpent)
        {
            Date = date;
            TimeSpent = timeSpent;
            TimeSpentSoFar = timeSpentSoFar;
            TimeEstimated = timeEstimated;
            TotalTimeSpent = totalTimeSpent;
        }
    }
}
