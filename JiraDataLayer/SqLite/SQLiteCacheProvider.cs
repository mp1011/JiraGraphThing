using JiraDataLayer.Cache;
using JiraDataLayer.Models;
using JiraDataLayer.Services;

namespace JiraDataLayer.SqLite
{
    public class SQLiteCacheProvider
    {
        private readonly SQLiteDAO _dao;
        private readonly AutoMapperService _autoMapper;

        internal SQLiteCacheProvider(SQLiteDAO dao, AutoMapperService autoMapper)
        {
            _dao = dao;
            _autoMapper = autoMapper;
        }

        public SQLiteCache<T> CreateCache<T>()
        {
            if (typeof(T) == typeof(SearchResults))
                return (new SearchResultCache(_dao, _autoMapper, CreateCache<JiraIssue>())) as SQLiteCache<T>;
            else 
                return new SQLiteCache<T>(_dao, _autoMapper);
        }
    }
}
