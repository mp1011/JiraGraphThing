using Atlassian.Jira;

namespace JiraDataLayer.Models
{
    public class JiraIssue
    {
        public string Key { get; }

        public string Project { get; }

        internal JiraIssue(Issue issue)
        {
            Key = issue.Key.Value;
            Project = issue.Project;
        }

        public override string ToString()
        {
            return Key;
        }
    }
}
