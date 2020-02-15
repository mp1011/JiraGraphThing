using JiraGraphThing.Core.Extensions;
using System;
using System.Threading.Tasks;

namespace JiraDataLayer.Cache
{
    public class BackgroundRefreshingCache<T> : ICache<T>
        where T:class
    {
        private readonly ICache<T> _underlyingCache;
        private readonly TimeSpan _maxAge;
        private DateTime _lastRefreshTime;
        
        public BackgroundRefreshingCache(TimeSpan maxAge, ICache<T> underlyingCache)
        {
            _maxAge = maxAge;
            _underlyingCache = underlyingCache;
        }

        public async Task<T> GetOrCompute(string key, Func<string, Task<T>> compute, bool forceCompute = false)
        {
            var cachedValue = _underlyingCache.GetValueOrDefault(key);
            if(cachedValue == null || forceCompute)
            {
                cachedValue = await _underlyingCache.GetOrCompute(key, compute, forceCompute);
                _lastRefreshTime = DateTime.Now;
                return cachedValue;
            }

            if (_lastRefreshTime.TimeSince() > _maxAge)
                ComputeValueInAnotherThread(key, compute);

            return cachedValue;
        }

        public T GetValueOrDefault(string key)
        {
            return _underlyingCache.GetValueOrDefault(key);
        }

        private void ComputeValueInAnotherThread(string key, Func<string, Task<T>> compute)
        {
            Task.Run(async () => await _underlyingCache.GetOrCompute(key, compute, forceCompute:true))
                .ContinueWith(t => _lastRefreshTime = DateTime.Now);
        }
    }
}
