using JiraGraphThing.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace JiraDataLayer.SqLite
{
    class SQLiteConnectionProvider
    {
        public IDbConnection CreateConnection()
        {
            string dbFilePath = "LocalDB.sqlite";
            if(DataConfig.DataPath.NotNullOrEmpty())
                dbFilePath = $@"{DataConfig.DataPath}\{dbFilePath}";

            var connectionString = $@"Data Source={dbFilePath};ProviderName=System.Data.SQLite;";
            if (!File.Exists("LocalDB.sqlite"))            
                connectionString += "New=True;";
            
            var connection = new SQLiteConnection(connectionString);       
            return connection;
        }
    }
}
