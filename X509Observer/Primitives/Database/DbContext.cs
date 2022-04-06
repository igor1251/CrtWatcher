using System;
using System.IO;
using Dapper;
using System.Data.SQLite;
using X509Observer.MagicStrings.DatabaseQueries;
using X509Observer.MagicStrings.MaintananceFilesNames;

namespace X509Observer.Primitives.Database
{
    public class DbContext : IDbContext
    {
        private string _DbPath = Environment.CurrentDirectory + "\\" + FileNames.MAIN_DATABASE_FILE_NAME;
        private SQLiteConnection _DbConnection;

        public DbContext()
        {
            _DbConnection = new SQLiteConnection("Data Source=" + _DbPath);

            if (!File.Exists(_DbPath))
            {
                _DbConnection.Execute(CommonRepositoriesQueries.CREATE_DATABASE);
            }            
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
