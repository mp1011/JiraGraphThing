using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiraDataLayer.Models
{
    public abstract class JiraGraph
    {
        public abstract IEnumerable<JiraGraph> GetChildren();
    }

    public abstract class JiraGraph<TChild> : JiraGraph
        where TChild : JiraGraph
    {
        private readonly TChild[] _children;

        protected JiraGraph(IEnumerable<TChild> children)
        {
            _children = children.ToArray();
        }

        public override IEnumerable<JiraGraph> GetChildren()
        {
            return _children;
        }
    }
}
