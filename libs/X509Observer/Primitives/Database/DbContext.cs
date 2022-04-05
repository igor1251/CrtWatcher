using System;
using System.IO;
using Dapper;
using System.Data.SQLite;
using X509Observer.MagicStrings.DatabaseQueries;

namespace X509Observer.Primitives.Database
{
    public class DbContext : IDbContext
    {
        private string _DbPath = Environment.CurrentDirectory + "\\db.sqlite";
        private SQLiteConnection _DbConnection;

        public DbContext()
        {
            if (!File.Exists(_DbPath))
            {
                _DbConnection.Execute(CommonRepositoriesQueries.CREATE_DATABASE);
            }

            _DbConnection = new SQLiteConnection("Data Source=" + _DbPath);
        }

        public string DbPath
        {
            get { return _DbPath; }
        }

        public SQLiteConnection DbConnection
        {
            get { return _DbConnection; }
        }
    }
}
