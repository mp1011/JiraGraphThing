using Atlassian.Jira;
using JiraDataLayer.Cache;
using JiraDataLayer.Models;
using JiraDataLayer.SqLite;
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
        private readonly SQLiteCache<SearchResults> _cache;
        private readonly SQLiteCache<WorkLog[]> _workLogCache;

        private const int _recordsPerBatch = 100;
        
        internal JiraIssueService(JiraRestClientProvider jiraClientProvider, CustomFieldReader customFieldReader,
            SQLiteCacheProvider cacheProvider)
        {
            _customFieldReader = customFieldReader;
            _jiraClientProvider = jiraClientProvider;
            _cache = cacheProvider.CreateCache<SearchResults>();
            _workLogCache = cacheProvider.CreateArrayCache<WorkLog>();
        }

        public async Task<JiraIssue> GetIssue(string key)
        {
            return (await GetIssues(new SearchArgs(key: key, take: 1))).FirstOrDefault();            
        }

        public async Task<JiraIssue[]> GetIssues(SearchArgs searchArgs)
        {     
            var jql = GetJql(searchArgs);
            var results = await _cache.GetOrCompute(jql, () => DoSearch(jql, searchArgs.Take));
            return results.Items;
        }

        private async Task<SearchResults> DoSearch(string jql, int maxToTake)
        {
            List<JiraIssue> results = new List<JiraIssue>();
            var client = _jiraClientProvider.CreateClient();
            int skip = 0;
            int originalMaxToTake = maxToTake;

            while (true)
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

            return new SearchResults(jql, originalMaxToTake, results.ToArray());
        }

        public async Task<WorkLog[]> GetWorkLogs(string issueKey, Sprint sprint = null)
        {
            var client = _jiraClientProvider.CreateClient();
            var logs = await _workLogCache.GetOrCompute(issueKey,
                async () => 
                (   await client.Issues
                    .GetWorklogsAsync(issueKey))
                    .Select(log=>new WorkLog(log))
                    .ToArray());

            if (sprint != null)
                logs = logs.Where(p => p.Start >= sprint.Start && p.Start <= sprint.End).ToArray();

            return logs;
        }

        public string GetJql(SearchArgs searchArgs)
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
