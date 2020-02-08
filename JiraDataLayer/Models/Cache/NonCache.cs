using System;
using System.Threading.Tasks;

namespace JiraDataLayer.Models.Cache
{
    class NonCache<T> : ICache<T>
    {
        public async Task<T> GetOrCompute(string key, Func<Task<T>> compute)
        {
            return await compute();
        }
    }
}
