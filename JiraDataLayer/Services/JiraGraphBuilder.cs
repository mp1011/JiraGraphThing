﻿using JiraDataLayer.Models;
using JiraDataLayer.Models.Cache;
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

        public JiraGraphBuilder(JiraIssueService jiraIssueService)
        {
            _jiraIssueService = jiraIssueService;
        }

        public async Task<IssueNode> LoadItemGraph(string key)
        {
            var parent = await _jiraIssueService.GetIssue(key);
            return await ConstructNode(parent, new NonCache<IssueNode>());
        }

        public async Task<SprintNode> LoadSprintGraph(string sprint)
        {
            var cache = new ConcurrentInMemoryCache<IssueNode>();
            List<IssueNode> sprintNodes = new List<IssueNode>();
            await foreach(var issue in _jiraIssueService.GetIssues(new SearchArgs(sprint: sprint)))
            {
                sprintNodes.Add(await ConstructNode(issue, cache));
            }

            var unique = sprintNodes.Where(s => !sprintNodes
                .Any(k => k != s && k.Contains(s)))
                .ToArray();

            return new SprintNode(sprint, unique);
        }

        private async Task<IssueNode> ConstructNode(JiraIssue issue, ICache<IssueNode> cache)
        {
            return await (cache.GetOrCompute(issue.Key, async () =>
            {
                var logs = await _jiraIssueService.GetWorkLogs(issue.Key).AttachErrorHandler();
                return new IssueNode(issue, logs, await LoadChildren<IssueNode>(issue,cache).AttachErrorHandler());
            })).AttachErrorHandler();
        }

        private async Task<T[]> LoadChildren<T>(JiraIssue parent, ICache<IssueNode> cache)
            where T:JiraGraph
        {

            var children = await _jiraIssueService.GetIssuesAsArray(new SearchArgs(parentKey: parent.Key)).AttachErrorHandler();
            List<T> result = new List<T>();

            foreach(var child in children)
            {
                var childNode = await ConstructNode(child,cache).AttachErrorHandler();
                if (childNode is T typedNode)
                    result.Add(typedNode);
                else 
                    throw new Exception($"Expected child of {parent} to be of type {typeof(T)}");
            }

            return result.ToArray();
        }

    }
}