using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace JiraDataLayer.Models.Cache
{
    public class ConcurrentInMemoryCache<T> : ICache<T>
    {
        private ConcurrentDictionary<string, T> _items = new ConcurrentDictionary<string, T>();

        public async Task<T> GetOrCompute(string key, Func<Task<T>> compute)
        {
            return await Task.Run(async () =>
            {
                T result;
                if (!_items.TryGetValue(key, out result))
                {
                    result = await compute();                          
                    _items.TryAdd(key, result);
                }

                return result;
            });
        }
    }
}
