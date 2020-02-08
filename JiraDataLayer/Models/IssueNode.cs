using System.Collections.Generic;

namespace JiraDataLayer.Models
{
    public class IssueNode : JiraGraph<IssueNode>
    {
        public JiraIssue Issue { get; }
        public IssueNode(JiraIssue issue, IEnumerable<IssueNode> children) : base(children)
        {
            Issue = issue;
        }
    }
}
