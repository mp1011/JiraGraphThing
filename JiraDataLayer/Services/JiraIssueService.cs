using JiraDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraDataLayer.Services
{
    public class JiraIssueService
    {
        private readonly JiraRestClientProvider _jiraClientProvider;
        private const int _recordsPerBatch = 100;

        internal JiraIssueService(JiraRestClientProvider jiraClientProvider)
        {
            _jiraClientProvider = jiraClientProvider;
        }

        public async IAsyncEnumerable<JiraIssue> GetIssues(SearchArgs searchArgs)
        {
            var client = _jiraClientProvider.CreateClient();
            int maxToTake = searchArgs.Take;
            int skip = 0;

            while(true)
            {
                var chunk = await Task.Run(() => client
                                                    .Issues
                                                    .Queryable
                                                    .Where(i => i.Project == searchArgs.Project)
                                                    .Skip(skip)
                                                    .Take(Math.Min(_recordsPerBatch, maxToTake))
                                                    .ToArray());

                maxToTake -= chunk.Length;

                foreach (var item in chunk)
                    yield return new JiraIssue(item);

                skip += chunk.Length;
                if (chunk.Length == 0)
                    break;
            }
        }
    }
}
