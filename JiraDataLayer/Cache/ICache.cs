using System;
using System.Threading.Tasks;

namespace JiraDataLayer.Cache
{
    public interface ICache<T> 
        where T : class
    {
        Task<T> GetOrCompute(string key, Func<string, Task<T>> compute, bool forceCompute=false);

        T GetValueOrDefault(string key);

    }
}
