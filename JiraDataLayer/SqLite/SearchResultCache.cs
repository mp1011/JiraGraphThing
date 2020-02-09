using JiraDataLayer.Cache;
using JiraDataLayer.Models;
using JiraDataLayer.Models.DTO;
using JiraDataLayer.Services;
using JiraGraphThing.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraDataLayer.SqLite
{
    public class SearchResultCache : SQLiteCache<SearchResults>
    {
        private SQLiteCache<JiraIssue> _issueCache;

        internal SearchResultCache(SQLiteDAO dao, AutoMapperService autoMapperService, SQLiteCache<JiraIssue> issueCache) 
            : base(dao, autoMapperService)
        {
            _issueCache = issueCache;
        }

        protected override SearchResults Read(string key)
        {
            var cached = _dao.ReadFirst<JQLSearchDTO>("JQL=@jql", new { jql = key.AlphaNumericOnly() });
            if (cached != null)
            {
                var results = _dao.Read<JQLSearchItemDTO>("SearchID=@id", new { id = cached.ROWID });
                var issues = results.Select(p => _issueCache.GetCached(p.IssueKey)).ToArray();
                return new SearchResults(cached.JQL, cached.Take, issues);
            }

            return null;
        }

        protected override void Write(string key, SearchResults value)
        {
            _dao.Write(new JQLSearchDTO
            {
                JQL = value.JQL.AlphaNumericOnly(),
                Take = value.MaxRecords
            });

            //last_insert_rowid() isnt working, don't know why
            var id = _dao.GetRowID<JQLSearchDTO>($"JQL='{value.JQL.AlphaNumericOnly()}'");

            foreach(var item in value.Items)
            {
                Task.Run(()=>_issueCache.GetOrCompute(item.Key, async () => item));
                _dao.Write(new JQLSearchItemDTO { SearchID = id, IssueKey = item.Key });
            }

        }
    }
}
