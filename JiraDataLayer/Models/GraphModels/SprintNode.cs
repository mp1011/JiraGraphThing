﻿using System.Collections.Generic;
using System.Linq;

namespace JiraDataLayer.Models.GraphModels
{
    public class SprintNode : JiraGraph<IssueNode>
    {
        public string Sprint { get; }

        public SprintNode(string sprint, IEnumerable<IssueNode> children) : base(children)
        {
            Sprint = sprint;
        }

        public override decimal GetTotalStoryPoints()
        {
            return Children.Sum(p => p.GetTotalStoryPoints());
        }

        public override IEnumerable<WorkLog> GetWorkLogs()
        {
            return Children.SelectMany(p => p.GetWorkLogs());
        }

        public override string ToString()
        {
            return $"{Sprint} ({Children.Length} Issues)";
        }
    }
}