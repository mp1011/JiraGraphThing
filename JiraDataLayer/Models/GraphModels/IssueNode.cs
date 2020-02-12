using System;
using System.Collections.Generic;
using System.Linq;

namespace JiraDataLayer.Models.GraphModels
{
    public class IssueNode : JiraGraph<IssueNode>
    {
        public JiraIssue Issue { get; }

        public WorkLog[] WorkLogs { get; }

        public IssueNode(JiraIssue issue, WorkLog[] workLogs, IEnumerable<IssueNode> children) : base(children)
        {
            Issue = issue;
            WorkLogs = workLogs;
        }

        public override string Name => Issue.Key;

        public override decimal GetTotalStoryPoints()
        {
            return Issue.StoryPoints.GetValueOrDefault() +
                Children.Sum(p => p.GetTotalStoryPoints());
        }

        public override IEnumerable<WorkLog> GetWorkLogs()
        {
            foreach (var log in WorkLogs)
                yield return log;

            foreach(var child in Children)
            {
                foreach(var log in child.GetWorkLogs())
                {
                    yield return log;
                }
            }
        }

        public override string ToString()
        {
            return Issue.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is IssueNode i)
                return i.Issue.Key == Issue.Key;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Issue.Key.GetHashCode();
        }
    }
}
