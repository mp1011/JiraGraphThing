using JiraDataLayer.Models;
using JiraDataLayer.Models.GraphModels;
using JiraGraphThing.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JiraDataLayer.Services
{
    public class DailyWorkHistoryCalculator
    {

        public IEnumerable<DailyWorkHistory> CalculateWorkHistory(Sprint sprint, JiraGraph node)
        {
            var sprintDays = sprint.GetDays().ToArray();
            var workLogs = node.GetWorkLogs().ToArray();
            TimeSpan timeSpentSoFar = TimeSpan.Zero;

            var totalTimeSpent = TimeSpan.FromMinutes(workLogs
                .Where(p => p.Start >= sprint.Start && p.Start < sprint.End)
                .Sum(p => p.TimeSpent.TotalMinutes));

            var timeEstimated = TimeSpan.FromMinutes((double)node.GetTotalStoryPoints() * sprint.TimePerStoryPoint.TotalMinutes);

            foreach(var day in sprintDays)
            {
                var historyForDay = CalculateWorkHistoryForDay(day, workLogs, timeSpentSoFar, timeEstimated, totalTimeSpent);
                timeSpentSoFar = historyForDay.TimeSpentSoFar;
                yield return historyForDay;
            }
        }

        private DailyWorkHistory CalculateWorkHistoryForDay(DateTime day, WorkLog[] workLogs, TimeSpan timeSpentSoFar,
            TimeSpan timeEstimated, TimeSpan totalTimeSpent)
        {
            var logsForDay = workLogs
                .Where(p => p.Start >= day && p.Start < day.AddDays(1))
                .ToArray();

            var timeSpent = TimeSpan.FromMinutes(logsForDay.Sum(p => p.TimeSpent.TotalMinutes));

            return new DailyWorkHistory(day, timeSpent, timeSpent + timeSpentSoFar, timeEstimated, totalTimeSpent);
        }
    }
}
