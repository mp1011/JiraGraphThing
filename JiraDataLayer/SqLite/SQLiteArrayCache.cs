using JiraDataLayer.Models.DTO;
using JiraDataLayer.Services;
using JiraDataLayer.SqLite;
using JiraGraphThing.Core.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JiraDataLayer.Cache
{

    public class SQLiteArrayCache<T> : SQLiteCache<T[]>
    {      
        internal SQLiteArrayCache(SQLiteDAO dao, AutoMapperService autoMapperService) : base(dao,autoMapperService)
        {
        }

        public async Task<T[]> GetOrCompute(string key, Func<Task<T[]>> compute)
        {
            throw new NotImplementedException();
            //var result = Read(key);
            //if(result == null)
            //{
            //    result = await compute();
            //    Write(key, result);
            //}

            //return result;
        }

        protected override T[] Read(string key)
        {
            var mappedType = _autoMapperService.GetMappedType<T>();
            var cached = _dao.Read(mappedType, "Key=@key", new { key });
            if (!cached.IsNullOrEmpty())
            {
                return cached
                    .Select(p => _autoMapperService.MapTo<T>(p))
                    .ToArray();
            }
            else
                return null;
        }

        protected override void Write(string key, T[] value)
        {
            foreach(var item in value)
            {
                IWithKey dto = _autoMapperService.Map(item) as IWithKey;
                dto.Key = key;
                _dao.InvokeGenericMethod(nameof(_dao.Write), dto.GetType(), dto);
            }

        }
    }
}
