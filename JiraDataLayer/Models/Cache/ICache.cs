using System;
using System.Threading.Tasks;

namespace JiraDataLayer.Models.Cache
{
    public interface ICache<T>
    {
        Task<T> GetOrCompute(string key, Func<Task<T>> compute);
    }
}
