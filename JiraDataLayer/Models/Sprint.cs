using JiraDataLayer.Models.JiraPocos;
using JiraGraphThing.Core.Extensions;
using System;

namespace JiraDataLayer.Models
{
    public class Sprint
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public TimeSpan TimePerStoryPoint { get; }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        internal Sprint(SprintAPIModel sprintModel, TimeSpan timePerStoryPoint)
        {
            Name = sprintModel.name;

            //calculate because start date is whenever the start button was pushed
            if (sprintModel.endDate.Year > 1)
            {
                Start = sprintModel.endDate.AddDays(-13)
                                           .GetMorningTime();
            }
            End = sprintModel.endDate;
            TimePerStoryPoint = timePerStoryPoint;
        }
    }
}
