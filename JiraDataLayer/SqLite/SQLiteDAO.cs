using Dapper;
using JiraDataLayer.Models.DTO;
using JiraGraphThing.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiraDataLayer.SqLite
{
    public class SQLiteDAO
    {
        private readonly SQLiteConnectionProvider _sqliteConnectionProvider;
        private readonly SQLiteTableCreator _sqliteTableCreator;
    
        internal SQLiteDAO(SQLiteConnectionProvider sqliteConnectionProvider, SQLiteTableCreator sqliteTableCreator)
        {
            _sqliteConnectionProvider = sqliteConnectionProvider;
            _sqliteTableCreator = sqliteTableCreator;
        }

        public object ReadFirst(Type t, string whereClause, object args)
        {
            return this.InvokeGenericMethod(nameof(ReadFirst), t, whereClause,args);
        }

        public object[] Read(Type type, string whereClause, object args = null)
        {
            return (object[])this.InvokeGenericMethod(nameof(Read), type, whereClause, args);
        }

        public T[] Read<T>(string whereClause, object args=null)
        {
            using (var conn = _sqliteConnectionProvider.CreateConnection())
            {
                var tableName = _sqliteTableCreator.EnsureTableCreated<T>(conn);
                return conn.Query<T>($"SELECT ROWID,* FROM {tableName} WHERE {whereClause}", args)
                    .ToArray();             
            }
        }

        public int GetRowID<T>(string whereClause, object args = null)
        {
            using (var conn = _sqliteConnectionProvider.CreateConnection())
            {
                var tableName = _sqliteTableCreator.EnsureTableCreated<T>(conn);
                var id= conn.ExecuteScalar<int>($"SELECT ROWID FROM {tableName} WHERE {whereClause}", args);
                return id;
            }
        }

        public T ReadFirst<T>(string whereClause, object args=null)
        {
            return Read<T>(whereClause, args).FirstOrDefault();
        }

        public void Write<T>(T value)
        {
            using (var conn = _sqliteConnectionProvider.CreateConnection())
            {
                var tableName = _sqliteTableCreator.EnsureTableCreated<T>(conn);

                //dapper.contrib insert doesn't work
                var properties = typeof(T)
                    .GetProperties()
                    .Where(p => p.Name != "ROWID")
                    .ToArray();

                var fieldNames = properties
                    .Select(p => p.Name).ToArray();
                var fieldValues = properties.Select(p =>
                {
                    var propertyValue = p.GetValue(value);
                    if (propertyValue == null)
                        return "null";
                    else if (propertyValue is string s)
                        return $"'{s}'";
                    else if (propertyValue is DateTime dt)
                        return $"'{dt.ToString("yyyy-MM-dd hh:mm:ss")}'";
                    else
                        return propertyValue.ToString();
                }).ToArray();

                var insertScript = new StringBuilder()
                    .Append($"INSERT INTO {tableName} (")
                    .Append(string.Join(",", fieldNames))
                    .Append(") VALUES (")
                    .Append(string.Join(",", fieldValues))
                    .Append(");")
                    .ToString();

                conn.ExecuteScalar(insertScript);
            }
        }
    }

}
