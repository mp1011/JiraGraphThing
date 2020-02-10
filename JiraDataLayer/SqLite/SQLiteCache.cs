using JiraDataLayer.Services;
using JiraDataLayer.SqLite;
using JiraGraphThing.Core.Extensions;
using System;
using System.Threading.Tasks;

namespace JiraDataLayer.Cache
{
    public class SQLiteCache<T> : ICache<T>
    {
        protected readonly SQLiteDAO _dao;
        protected readonly AutoMapperService _autoMapperService;

        internal SQLiteCache(SQLiteDAO dao, AutoMapperService autoMapperService)
        {
            _dao = dao;
            _autoMapperService = autoMapperService;
        }

        public T GetCached(string key)
        {
            return Read(key);
        }
           
        public async Task<T> GetOrCompute(string key, Func<Task<T>> compute)
        {
            var result = Read(key);
            if(result == null)
            {
                result = await compute();
                Write(key, result);
            }

            return result;
        }

        protected virtual T Read(string key)
        {
            var mappedType = _autoMapperService.GetMappedType<T>();
            var cached = _dao.ReadFirst(mappedType, "Key=@key",new { key });
            if (cached != null)
                return _autoMapperService.MapTo<T>(cached);
            else
                return default(T);
        }

        protected virtual void Write(string key, T value)
        {
            var dto = _autoMapperService.Map(value);
            _dao.InvokeGenericMethod(nameof(_dao.Write), value.GetType(), value);
        }
    }
}
