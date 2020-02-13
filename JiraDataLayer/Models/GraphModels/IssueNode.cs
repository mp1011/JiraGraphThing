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

        public override string Name => Issue.ToString();

        public override string[] GetAssociatedUsers()
        {
            List<string> users = new List<string>();
            users.Add(Issue.Assignee);
            users.AddRange(base.GetAssociatedUsers());
            return users.Distinct().ToArray();
        }
        public bool IsAnyAssignedTo(string user)
        {
            if (Issue.Assignee == user)
                return true;
            else
                return Children.Any(c => c.IsAnyAssignedTo(user));
        }

        public override decimal GetTotalStoryPoints()
        {
            return Issue.StoryPoints.GetValueOrDefault() + base.GetTotalStoryPoints();
        }

        public override IEnumerable<WorkLog> GetWorkLogs()
        {
            return WorkLogs.Union(base.GetWorkLogs());
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
