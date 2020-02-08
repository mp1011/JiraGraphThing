using Atlassian.Jira;
using System;

namespace JiraDataLayer.Models
{
    public class WorkLog
    {
        public string Author { get; }

        public DateTime Start { get; }

        public TimeSpan TimeSpent { get; }

        public WorkLog(Worklog log)
        {
            Author = log.Author;
            Start = log.StartDate.GetValueOrDefault();
            TimeSpent = TimeSpan.FromSeconds(log.TimeSpentInSeconds);
        }
    }
}
