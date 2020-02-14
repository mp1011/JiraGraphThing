using System.Collections.Generic;
using System.Linq;

namespace JiraDataLayer.Models.GraphModels
{
    public class SprintNode : JiraGraph<IssueNode>
    {
        public Sprint Sprint { get; }

        public SprintNode(Sprint sprint, IEnumerable<IssueNode> children) : base(children)
        {
            Sprint = sprint;
        }

        public override string Name => Sprint.Name;

        

        public override string ToString()
        {
            return $"{Sprint} ({Children.Length} Issues)";
        }
    }
}
