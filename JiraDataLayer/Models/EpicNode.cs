using System.Collections.Generic;

namespace JiraDataLayer.Models
{
    public class EpicNode : JiraGraph<IssueNode>
    {
        public JiraIssue Epic { get; }

        public EpicNode(JiraIssue epic, IEnumerable<IssueNode> children) :base(children)
        {
            Epic = epic;
        }
    }
}
