using Atlassian.Jira;
using JiraDataLayer.Models.JiraPocos;
using System;

namespace JiraDataLayer.Models
{
    public class WorkLog
    {
        public string Author { get; private set; }

        public DateTime Start { get; private set; }

        public TimeSpan TimeSpent { get; private set; }

        internal WorkLog()
        {

        }

        public WorkLog(Worklog log)
        {
            Author = log.Author;
            Start = log.StartDate.GetValueOrDefault();
            TimeSpent = TimeSpan.FromSeconds(log.TimeSpentInSeconds);
        }

        internal WorkLog(WorkLogV2 log)
        {
            Author = log.Author.displayName;
            Start = log.started;
            TimeSpent = TimeSpan.FromSeconds(log.timeSpentSeconds);
        }


        public WorkLog(string author, DateTime start, TimeSpan timeSpent)
        {
            Author = author;
            Start = start;
            TimeSpent = timeSpent;
        }
    }
}
