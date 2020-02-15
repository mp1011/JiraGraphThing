using JiraDataLayer.SqLite;
using System;

namespace JiraDataLayer.Cache
{
    public class BackgroundCacheProvider : ICacheProvider
    {
        private readonly TimeSpan _cacheExpirationTime = TimeSpan.FromHours(1);
        private readonly SQLiteCacheProvider _internalCacheProvider;

        internal BackgroundCacheProvider(SQLiteCacheProvider internalCacheProvider)
        {
            _internalCacheProvider = internalCacheProvider;
        }

        public ICache<T[]> CreateArrayCache<T>() where T : class
        {
            return new BackgroundRefreshingCache<T[]>(_cacheExpirationTime, _internalCacheProvider.CreateArrayCache<T>());
        }

        public ICache<T> CreateCache<T>() where T : class
        {
            return new BackgroundRefreshingCache<T>(_cacheExpirationTime, _internalCacheProvider.CreateCache<T>());
        }
    }
}
