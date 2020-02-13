using System;
using System.Collections.Generic;
using System.Linq;

namespace JiraDataLayer.Models.GraphModels
{
    public abstract class JiraGraph
    {
        public abstract IEnumerable<JiraGraph> GetChildren();

        public abstract decimal GetTotalStoryPoints();

        public abstract IEnumerable<WorkLog> GetWorkLogs();

        public abstract string Name { get; }

        public abstract string[] GetAssociatedUsers();

        public TimeSpan GetTotalTimeSpent()
        {
            return TimeSpan.FromSeconds(GetWorkLogs().Sum(p => p.TimeSpent.TotalSeconds));
        }

        public IEnumerable<JiraGraph> TraverseAll()
        {
            List<JiraGraph> list = new List<JiraGraph>();
            TraverseAll(list);
            return list;
        }

        private void TraverseAll(List<JiraGraph> list)
        {
            list.Add(this);
            foreach (var child in GetChildren())
                TraverseAll(list);
        }

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

        public override decimal GetTotalStoryPoints()
        {
            return Children.Sum(p => p.GetTotalStoryPoints());
        }

        public override IEnumerable<WorkLog> GetWorkLogs()
        {
            return Children.SelectMany(p => p.GetWorkLogs());
        }

        public override string[] GetAssociatedUsers()
        {
            var usersFromWorkLogs = GetWorkLogs()
                .Select(p => p.Author)
                .Distinct()
                .ToArray();

            var usersFromChildren = Children
                .SelectMany(p => p.GetAssociatedUsers())
                .Distinct()
                .ToArray();

            return usersFromChildren
                .Union(usersFromWorkLogs)
                .Distinct()
                .ToArray();
        }
    }
}
