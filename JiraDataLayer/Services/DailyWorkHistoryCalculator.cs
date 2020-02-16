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
            var day = sprint.Start.GetMorningTime();
            TimeSpan totalTimeSpent = TimeSpan.Zero;

            var workLogs = node.GetWorkLogs().ToArray();

            while(day <= sprint.End)
            {
                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                {
                    var historyForDay = CalculateWorkHistoryForDay(day, workLogs, totalTimeSpent);
                    totalTimeSpent = historyForDay.TimeSpentSoFar;
                    yield return historyForDay;
                }

                day = day.AddDays(1);
            }
        }

        private DailyWorkHistory CalculateWorkHistoryForDay(DateTime day, WorkLog[] workLogs, TimeSpan timeSpentSoFar)
        {
            var logsForDay = workLogs
                .Where(p => p.Start >= day && p.Start < day.AddDays(1))
                .ToArray();

            var timeSpent = TimeSpan.FromMinutes(logsForDay.Sum(p => p.TimeSpent.TotalMinutes));

            return new DailyWorkHistory(day, timeSpent, timeSpent + timeSpentSoFar);
        }
    }
}
