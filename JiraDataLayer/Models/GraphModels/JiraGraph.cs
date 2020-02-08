using System.Collections.Generic;
using System.Linq;

namespace JiraDataLayer.Models.GraphModels
{
    public abstract class JiraGraph
    {
        public abstract IEnumerable<JiraGraph> GetChildren();

        public abstract decimal GetTotalStoryPoints();

        public abstract IEnumerable<WorkLog> GetWorkLogs();

        public bool Contains(JiraGraph search)
        {
            if (search.Equals(this))
                return true;

            foreach(var child in GetChildren())
            {
                if (child.Contains(search))
                    return true;
            }

            return false;
        }
    }

    public abstract class JiraGraph<TChild> : JiraGraph
        where TChild : JiraGraph
    {        
        public TChild[] Children { get; }

        protected JiraGraph(IEnumerable<TChild> children)
        {
            Children = children.ToArray();
        }

        public override IEnumerable<JiraGraph> GetChildren()
        {
            return Children;
        }
    }
}
