using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace JiraDataLayer.Cache
{
    public class ConcurrentInMemoryCache<T> : ICache<T>
        where T : class
    {
        private ConcurrentDictionary<string, T> _items = new ConcurrentDictionary<string, T>();

        public async Task<T> GetOrCompute(string key, Func<string, Task<T>> compute, bool forceCompute = false)
        {
            return await Task.Run(async () =>
            {
                T result;
                if (forceCompute || !_items.TryGetValue(key, out result))
                {
                    result = await compute(key);
                    if (_items.ContainsKey(key))
                    {
                        T oldValue;
                        _items.TryGetValue(key, out oldValue);
                        _items.TryUpdate(key, result, oldValue);
                    }
                    else 
                        _items.TryAdd(key, result);
                }

                return result;
            });
        }

        public T GetValueOrDefault(string key)
        {
            T result;
            if (!_items.TryGetValue(key, out result))
                return default;

            return result;
        }
    }
}
