using System;
using System.Collections.Generic;
using System.Text;

namespace JiraDataLayer.Models.GraphModels
{
    public class UserNode : JiraGraph<IssueNode>
    {
        public override string Name { get; }

        public UserNode(string name, IEnumerable<IssueNode> issues) : base(issues)
        {
            Name = name;
        }
    }
}
