using System;
using System.Threading.Tasks;

namespace JiraDataLayer.Cache
{
    class NonCache<T> : ICache<T>
    {
        public async Task<T> GetOrCompute(string key, Func<Task<T>> compute)
        {
            return await compute();
        }
    }
}
