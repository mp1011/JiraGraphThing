using JiraDataLayer.Models;
using JiraDataLayer.Models.GraphModels;

namespace JiraGraphThing.Models
{
    public class NodeWithSprint
    {
        public JiraGraph Node { get; }
        public Sprint Sprint { get; }

        public NodeWithSprint(JiraGraph node, Sprint sprint)
        {
            Node = node;
            Sprint = sprint;
        }
    }
}
