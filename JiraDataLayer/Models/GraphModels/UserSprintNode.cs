using System.Collections.Generic;

namespace JiraDataLayer.Models.GraphModels
{
    public class UserSprintNode : JiraGraph<UserNode>
    {
        public string Sprint { get; }

        public override string Name => Sprint;

        public UserSprintNode(string sprint, IEnumerable<UserNode> users) : base(users)
        {
            Sprint = sprint;
        }       
    }
}
