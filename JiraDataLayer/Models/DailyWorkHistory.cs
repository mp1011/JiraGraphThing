using System;

namespace JiraDataLayer.Models
{
    public class DailyWorkHistory
    {
        public DateTime Date { get; }
        public TimeSpan TimeSpent { get; }
        public TimeSpan TimeSpentSoFar { get; }

        public DailyWorkHistory(DateTime date, TimeSpan timeSpent, TimeSpan timeSpentSoFar)
        {
            Date = date;
            TimeSpent = timeSpent;
            TimeSpentSoFar = timeSpentSoFar;
        }
    }
}
