using JiraGraphThing.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiraDataLayer.Cache
{
    public class BackgroundRefreshingCache<T> : ICache<T>
        where T:class
    {
        private readonly ICache<T> _underlyingCache;
        private readonly TimeSpan _maxAge;
        private Dictionary<string, DateTime> _lastRefreshTime = new Dictionary<string, DateTime>();
        
        public BackgroundRefreshingCache(TimeSpan maxAge, ICache<T> underlyingCache)
        {
            _maxAge = maxAge;
            _underlyingCache = underlyingCache;
        }

        public async Task<T> GetOrCompute(string key, Func<string, Task<T>> compute, bool forceCompute = false)
        {
            if (!_lastRefreshTime.ContainsKey(key))
                _lastRefreshTime[key] = DateTime.MinValue;

            var cachedValue = _underlyingCache.GetValueOrDefault(key);
            if(cachedValue == null || forceCompute)
            {
                cachedValue = await _underlyingCache.GetOrCompute(key, compute, forceCompute);
                _lastRefreshTime[key] = DateTime.Now;
            }

            if (_lastRefreshTime[key].TimeSince() > _maxAge)
                ComputeValueInAnotherThread(key, compute);

            return cachedValue;
        }

        public T GetValueOrDefault(string key)
        {
            return _underlyingCache.GetValueOrDefault(key);
        }

        private void ComputeValueInAnotherThread(string key, Func<string, Task<T>> compute)
        {
            if (key.ToUpper().Contains("DSDE-167"))
                Console.WriteLine("!");

            Task.Run(async () => await _underlyingCache.GetOrCompute(key, compute, forceCompute:true))
                .ContinueWith(t => _lastRefreshTime[key] = DateTime.Now);
        }
    }
}
