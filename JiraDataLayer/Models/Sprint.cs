using JiraDataLayer.Models.JiraPocos;
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
            Start = sprintModel.startDate;
            End = sprintModel.endDate;
            TimePerStoryPoint = timePerStoryPoint;
        }
    }
}
