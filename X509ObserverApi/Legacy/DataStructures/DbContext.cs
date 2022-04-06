using System;
using System.Data.SQLite;

namespace DataStructures
{
    public class DbContext : IDbContext
    {
        SQLiteConnection _dbConnection;
        string _dbPath = Environment.CurrentDirectory + "\\db.sqlite";

        public DbContext()
        {
            _dbConnection = new SQLiteConnection("Data Source=" + _dbPath);
        }

        public string DbPath => _dbPath;
        public SQLiteConnection DbConnection => _dbConnection;
    }
}
