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
            var connectionString = $@"Data Source=LocalDB.sqlite;ProviderName=System.Data.SQLite";
            if (!File.Exists("LocalDB.sqlite"))            
                connectionString += "New=True;";
            
            var connection = new SQLiteConnection(connectionString);       
            return connection;
        }
    }
}
