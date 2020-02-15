using JiraDataLayer.Models;
using JiraDataLayer.Models.GraphModels;
using System;

namespace JiraGraphThing.Models
{
    public class UINode
    {
        public JiraGraph Node { get; }
        public Sprint Sprint { get; }
        public decimal MaxStoryPointsWithinParent { get; }
        public bool EnableExpand { get; }

        public UINode(JiraGraph node, Sprint sprint, decimal maxStoryPointsWithinParent, bool enableExpand)
        {
            Node = node;
            Sprint = sprint;
            MaxStoryPointsWithinParent = maxStoryPointsWithinParent;
            EnableExpand = enableExpand;
        }
    }
}
