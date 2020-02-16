using JiraDataLayer.Models;
using System;

namespace JiraGraphThing.Models
{
    public class UIWorkLog
    {
        private readonly WorkLog _workLog;

        public DateTime Start => _workLog.Start;

        public TimeSpan TimeSpent => _workLog.TimeSpent;

        public TimeSpan TimeSpentSoFar { get; }

        public UIWorkLog(WorkLog workLog, TimeSpan timeSpentSoFar)
        {
            _workLog = workLog;
            TimeSpentSoFar = timeSpentSoFar;
        }
    }
}
