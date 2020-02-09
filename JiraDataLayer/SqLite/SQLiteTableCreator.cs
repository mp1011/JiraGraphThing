using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JiraDataLayer.SqLite
{
    class SQLiteTableCreator
    {
        public string EnsureTableCreated<T>(IDbConnection connection)
        {
            string tableName = typeof(T).Name.Replace("DTO", "");

            var exists = connection
                .Query<string>($"SELECT name from sqlite_master where name='{tableName}'")
                .Any();

            if(!exists)
                connection.Execute(GetEnsureTableCreatedScript(typeof(T), tableName));

            return tableName;
        }

        private string GetEnsureTableCreatedScript(Type type, string tableName)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE {tableName} (");

            bool first = true;
            foreach (var property in type.GetProperties().Where(p => p.Name != "ROWID"))
            {
                if(!first)
                    sb.Append(",");

                first = false;
                sb.AppendLine(CreatePropertyScript(property));
            }

            sb.AppendLine(")");

            return sb.ToString();
        }

        private string CreatePropertyScript(PropertyInfo property)
        {
            return $"{property.Name} {ToSqliteType(property.PropertyType)}";
        }

        private string ToSqliteType(Type dotNetType)
        {
            if (dotNetType == typeof(int))
                return "INTEGER";
            else if (dotNetType == typeof(decimal))
                return "REAL";
            else if (dotNetType == typeof(string))
                return "TEXT";
            else if(dotNetType.IsGenericType && dotNetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return ToSqliteType(dotNetType.GetGenericArguments()[0]);
            else 
                throw new NotSupportedException($"Type {dotNetType.Name} is not supported");
        }

    }
}
