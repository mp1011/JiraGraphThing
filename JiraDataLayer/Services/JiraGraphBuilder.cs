using JiraDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiraDataLayer.Services
{
    public class JiraGraphBuilder
    {
        private readonly JiraIssueService _jiraIssueService;

        public JiraGraphBuilder(JiraIssueService jiraIssueService)
        {
            _jiraIssueService = jiraIssueService;
        }

        public async Task<JiraGraph> LoadItemGraph(string key)
        {
            var parent = await _jiraIssueService.GetIssue(key);
            return await ConstructNode(parent);
        }

        private async Task<JiraGraph> ConstructNode(JiraIssue issue)
        {
            switch(issue.TypeName)
            {
                case "Epic":
                    return new EpicNode(issue, await LoadChildren<IssueNode>(issue));
                default:
                    return new IssueNode(issue, await LoadChildren<IssueNode>(issue));
            }
        }


        private async Task<T[]> LoadChildren<T>(JiraIssue parent)
            where T:JiraGraph
        {

            var children = await _jiraIssueService.GetIssuesAsArray(new SearchArgs(parentKey: parent.Key));
            List<T> result = new List<T>();

            foreach(var child in children)
            {
                var childNode = await ConstructNode(child);
                if (childNode is T typedNode)
                    result.Add(typedNode);
                else 
                    throw new Exception($"Expected child of {parent} to be of type {typeof(T)}");
            }

            return result.ToArray();
        }

    }
}
