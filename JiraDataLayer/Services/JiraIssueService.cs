﻿using Atlassian.Jira;
using JiraDataLayer.Models;
using JiraGraphThing.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraDataLayer.Services
{
    public class JiraIssueService
    {
        private readonly CustomFieldReader _customFieldReader;
        private readonly JiraRestClientProvider _jiraClientProvider;
        private const int _recordsPerBatch = 100;

        internal JiraIssueService(JiraRestClientProvider jiraClientProvider, CustomFieldReader customFieldReader)
        {
            _customFieldReader = customFieldReader;
            _jiraClientProvider = jiraClientProvider;
        }

        public async Task<JiraIssue> GetIssue(string key)
        {
            await foreach(var result in GetIssues(new SearchArgs(key:key,take:1)).ConfigureAwait(true))            
                return result;
            
            return null;
        }

        public async Task<JiraIssue[]> GetIssuesAsArray(SearchArgs searchArgs)
        {
            List<JiraIssue> results = new List<JiraIssue>();

            await foreach(var result in GetIssues(searchArgs))            
                results.Add(result);
            
            return results.ToArray();
        }

        public async IAsyncEnumerable<JiraIssue> GetIssues(SearchArgs searchArgs)
        {
            var client = _jiraClientProvider.CreateClient();
            int maxToTake = searchArgs.Take;
            int skip = 0;

            var jql = GetJql(searchArgs);
      
            while(true)
            {
                var chunk = (await client
                                    .Issues
                                    .GetIssuesFromJqlAsync(
                                        jql: jql,
                                        maxIssues: Math.Min(_recordsPerBatch, maxToTake),
                                        startAt: skip)).ToArray();

                maxToTake -= chunk.Length;

                foreach (var item in chunk)
                    yield return new JiraIssue(item, _customFieldReader);

                skip += chunk.Length;
                if (chunk.Length == 0)
                    break;
            }
        }

        private string GetJql(SearchArgs searchArgs)
        {
            List<string> terms = new List<string>();
            if (searchArgs.Project.NotNullOrEmpty())
                terms.Add($"Project='{searchArgs.Project}'");

            if (searchArgs.Key.NotNullOrEmpty())
                terms.Add($"key='{searchArgs.Key}'");

            if (searchArgs.ParentKey.NotNullOrEmpty())
                terms.Add($"(Parent = '{searchArgs.ParentKey}' OR \"Epic Link\"='{searchArgs.ParentKey}')");

            return string.Join(" AND ", terms.ToArray());
        }
    }

}
