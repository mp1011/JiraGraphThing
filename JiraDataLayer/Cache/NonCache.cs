using System;
using System.Threading.Tasks;

namespace JiraDataLayer.Cache
{
    public class NonCache<T> : ICache<T>
         where T : class
    {
        public async Task<T> GetOrCompute(string key, Func<string, Task<T>> compute, bool forceCompute = false)
        {
            return await compute(key);
        }

        public T GetValueOrDefault(string key)
        {
            return default(T);
        }
    }
}
