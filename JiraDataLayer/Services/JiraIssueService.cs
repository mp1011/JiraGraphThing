using Atlassian.Jira;
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
            return (await GetIssues(new SearchArgs(key: key, take: 1))).FirstOrDefault();            
        }

        public async Task<JiraIssue[]> GetIssues(SearchArgs searchArgs)
        {
            List<JiraIssue> results = new List<JiraIssue>();
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
                    results.Add(new JiraIssue(item, _customFieldReader));

                skip += chunk.Length;
                if (chunk.Length == 0)
                    break;
            }

            return results.ToArray();
        }

        public async Task<WorkLog[]> GetWorkLogs(string issueKey)
        {
            var client = _jiraClientProvider.CreateClient();
            var logs = (await client.Issues.GetWorklogsAsync(issueKey)).ToArray();

            return logs.Select(log => new WorkLog(log))
                .ToArray();
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
            if (searchArgs.Sprint.NotNullOrEmpty())
                terms.Add($"Sprint='{searchArgs.Sprint}'");

            return string.Join(" AND ", terms.ToArray());
        }
    }

}
