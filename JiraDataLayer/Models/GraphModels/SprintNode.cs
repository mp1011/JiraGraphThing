using System.Collections.Generic;
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

        public override string Name => Sprint;

        

        public override string ToString()
        {
            return $"{Sprint} ({Children.Length} Issues)";
        }
    }
}
