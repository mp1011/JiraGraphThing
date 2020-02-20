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
            if (key == "DSDE-167")
                Console.WriteLine("!");

            var mappedType = _autoMapperService.GetMappedType<T>();
            _dao.Delete(mappedType, "Key=@key", new { key });

            foreach (var item in value)
            {
                IWithKey dto = _autoMapperService.Map(item) as IWithKey;
                dto.Key = key;
                _dao.Write(mappedType, dto, allowMultipleWithSameKey:true);
            }

        }
    }
}
