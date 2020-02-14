using System.Collections.Generic;

namespace JiraDataLayer.Models.GraphModels
{
    public class UserSprintNode : JiraGraph<UserNode>
    {
        public Sprint Sprint { get; }

        public override string Name => Sprint.Name;

        public UserSprintNode(Sprint sprint, IEnumerable<UserNode> users) : base(users)
        {
            Sprint = sprint;
        }       
    }
}
