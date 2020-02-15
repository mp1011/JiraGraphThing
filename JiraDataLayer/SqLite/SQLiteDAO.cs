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

            using (var conn = sqliteConnectionProvider.CreateConnection())
                _sqliteTableCreator.EnsureTablesCreated(conn);
        }

        public object ReadFirst(Type t, string whereClause, object args)
        {
            return this.InvokeGenericMethod(nameof(ReadFirst), t, whereClause, args);
        }

        public object[] Read(Type type, string whereClause, object args = null)
        {
            return (object[])this.InvokeGenericMethod(nameof(Read), type, whereClause, args);
        }

        public T[] Read<T>(string whereClause, object args = null)
        {
            using (var conn = _sqliteConnectionProvider.CreateConnection())
            {
                var tableName = _sqliteTableCreator.GetTableName<T>();
                return conn.Query<T>($"SELECT ROWID,* FROM {tableName} WHERE {whereClause}", args)
                    .ToArray();
            }
        }

        public int GetRowID<T>(string whereClause, object args = null)
        {
            using (var conn = _sqliteConnectionProvider.CreateConnection())
            {
                var tableName = _sqliteTableCreator.GetTableName<T>();
                var id = conn.ExecuteScalar<int>($"SELECT ROWID FROM {tableName} WHERE {whereClause}", args);
                return id;
            }
        }

        public T ReadFirst<T>(string whereClause, object args = null)
        {
            return Read<T>(whereClause, args).FirstOrDefault();
        }

        public void Write<T>(T value, bool allowMultipleWithSameKey=false)
            where T : IWithKey
        {
            lock (this)
            {
                if (string.IsNullOrEmpty(value.Key))
                    throw new Exception("Object must have a key before it can be saved");

                using (var conn = _sqliteConnectionProvider.CreateConnection())
                {
                    var tableName = _sqliteTableCreator.GetTableName<T>();

                    //dapper.contrib insert doesn't work
                    var properties = typeof(T)
                        .GetProperties()
                        .Where(p => p.Name != "ROWID")
                        .ToArray();

                    var fieldNames = properties
                        .Select(p => p.Name).ToArray();
                    var fieldValues = properties.Select(p => "@" + p.Name).ToArray();

                    var exists = conn.ExecuteScalar<int>(
                        $"select exists(select 0 from {tableName} where key = @Key)", new { value.Key });

                    if (exists == 0 || allowMultipleWithSameKey)
                    {
                        var insertScript = new StringBuilder()
                            .Append($"INSERT INTO {tableName} (")
                            .Append(string.Join(",", fieldNames))
                            .Append(") VALUES (")
                            .Append(string.Join(",", fieldValues))
                            .Append(");")
                            .ToString();

                        conn.ExecuteScalar(insertScript, value);
                    }
                    else
                    {
                        var fieldAssignments = fieldNames.Select(p => $"{p}=@{p}").ToArray();

                        var updateScript = new StringBuilder()
                            .Append($"UPDATE {tableName} SET ")
                            .Append(string.Join(", ", fieldAssignments))
                            .Append(" WHERE Key = @Key")
                            .ToString();

                        conn.ExecuteScalar(updateScript, value);
                    }
                }
            }
        }

        public void Write(Type objectType, object value, bool allowMultipleWithSameKey)
        {
            this.InvokeGenericMethod(nameof(Write), objectType, value, allowMultipleWithSameKey);
        }

        public void Delete<T>(string whereClause, object parameter)
        {
            var tableName = _sqliteTableCreator.GetTableName<T>();

            using (var conn = _sqliteConnectionProvider.CreateConnection())
            {
                conn.Execute($"DELETE FROM {tableName} WHERE {whereClause}", parameter);
            }
        }

        public void Delete(Type tableType, string whereClause, object parameter)
        {
            this.InvokeGenericMethod(nameof(Delete), tableType, whereClause, parameter);
        }
    }
}
