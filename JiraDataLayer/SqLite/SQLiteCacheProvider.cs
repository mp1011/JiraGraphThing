using JiraDataLayer.Cache;
using JiraDataLayer.Models;
using JiraDataLayer.Services;

namespace JiraDataLayer.SqLite
{
    public class SQLiteCacheProvider : ICacheProvider
    {
        private readonly SQLiteDAO _dao;
        private readonly AutoMapperService _autoMapper;

        internal SQLiteCacheProvider(SQLiteDAO dao, AutoMapperService autoMapper)
        {
            _dao = dao;
            _autoMapper = autoMapper;
        }

        public ICache<T> CreateCache<T>() where T : class
        {
            if (typeof(T) == typeof(SearchResults))
            {
                var cache = CreateCache<JiraIssue>();
                return (new SearchResultCache(_dao, _autoMapper, cache)) as ICache<T>;
            }
            else
                return new SQLiteCache<T>(_dao, _autoMapper);
        }

        public ICache<T[]> CreateArrayCache<T>() where T : class
        {
            return new SQLiteArrayCache<T>(_dao, _autoMapper);        
        }
    }
}
