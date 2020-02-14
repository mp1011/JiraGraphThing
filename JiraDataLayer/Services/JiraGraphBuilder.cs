using JiraDataLayer.Cache;
using JiraDataLayer.Models;
using JiraDataLayer.Models.GraphModels;
using JiraGraphThing.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraDataLayer.Services
{
    public class JiraGraphBuilder
    {
        private readonly JiraIssueService _jiraIssueService;

        public delegate void NotifyProgress(string message, decimal progress);

        public NotifyProgress OnProgressChanged;

        public JiraGraphBuilder(JiraIssueService jiraIssueService)
        {
            _jiraIssueService = jiraIssueService;
        }

        public async Task<IssueNode> LoadItemGraph(string key, Sprint sprint=null)
        {
            var parent = await _jiraIssueService.GetIssue(key);
            return await ConstructNode(parent, new NonCache<IssueNode>(), sprint);
        }

        public async Task<UserSprintNode> LoadUserSprintGraph(Sprint sprint)
        {
            var sprintNode = await LoadSprintGraph(sprint);

            var users = sprintNode.GetAssociatedUsers();

            var userNodes = users
                .Select(u => ExtractUserNode(sprintNode, u))
                .ToArray();

            return new UserSprintNode(sprint, userNodes);
        }

        private UserNode ExtractUserNode(SprintNode sprint, string user)
        {
            return new UserNode(user, ExtractForUser(user, sprint.Children));
        }

        private IEnumerable<IssueNode> ExtractForUser(string user, IEnumerable<IssueNode> nodes)
        {
            List<IssueNode> filtered = new List<IssueNode>();
            foreach(var node in nodes)
            {
                var extracted = ExtractForUser(user, node);
                if (extracted.GetWorkLogs().Any() || extracted.IsAnyAssignedTo(user))
                    filtered.Add(extracted);
            }

            return filtered;
        }

        private IssueNode ExtractForUser(string user, IssueNode node)
        {
            var userWorkLogs = node.WorkLogs
                .Where(p => p.Author == user)
                .ToArray();

            if(node.Issue.Assignee == user)
                return new IssueNode(node.Issue, userWorkLogs, ExtractForUser(user, node.Children));
            else
                return new IssueNode(node.Issue.RemoveStoryPoints(), userWorkLogs, ExtractForUser(user, node.Children));

        }

        public async Task<SprintNode> LoadSprintGraph(Sprint sprint)
        {
            OnProgressChanged?.Invoke($"Loading issues for {sprint}",0);

            var cache = new ConcurrentInMemoryCache<IssueNode>();
            List<IssueNode> sprintNodes = new List<IssueNode>();

            var issues = await _jiraIssueService.GetIssues(new SearchArgs(sprint: sprint.Name));
            int processed = 0;
            foreach (var issue in issues)
            {
                OnProgressChanged?.Invoke($"Loading {issue.Key}", (decimal)processed / (decimal)issues.Length);
                sprintNodes.Add(await ConstructNode(issue, cache,sprint));
                processed++;
            }

            var unique = sprintNodes.Where(s => !sprintNodes
                .Any(k => k != s && k.Contains(s)))
                .ToArray();

            return new SprintNode(sprint, unique);
        }

        private async Task<IssueNode> ConstructNode(JiraIssue issue, ICache<IssueNode> cache, Sprint sprint)
        {
            return await (cache.GetOrCompute(issue.Key, async () =>
            {
                var logs = await _jiraIssueService.GetWorkLogs(issue.Key,sprint).AttachErrorHandler();
                return new IssueNode(issue, logs, await LoadChildren<IssueNode>(issue,cache,sprint).AttachErrorHandler());
            })).AttachErrorHandler();
        }

        private async Task<T[]> LoadChildren<T>(JiraIssue parent, ICache<IssueNode> cache, Sprint sprint)
            where T:JiraGraph
        {

            var children = await _jiraIssueService.GetIssues(new SearchArgs(parentKey: parent.Key)).AttachErrorHandler();
            List<T> result = new List<T>();

            foreach(var child in children)
            {
                var childNode = await ConstructNode(child,cache,sprint).AttachErrorHandler();
                if (childNode is T typedNode)
                    result.Add(typedNode);
                else 
                    throw new Exception($"Expected child of {parent} to be of type {typeof(T)}");
            }

            return result.ToArray();
        }

    }
}
