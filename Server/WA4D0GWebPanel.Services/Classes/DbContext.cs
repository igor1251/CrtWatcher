using System;
using System.Data.SQLite;
using WA4D0GWebPanel.Services.Interfaces;

namespace WA4D0GWebPanel.Services.Classes
{
    public class DbContext : IDbContext
    {
        SQLiteConnection _dbConnection;
        string _dbPath = Environment.CurrentDirectory + "\\db\\keysdb.sqlite";

        public DbContext()
        {
            _dbConnection = new SQLiteConnection(_dbPath);
        }

        public string DbPath => _dbPath;
        public SQLiteConnection DbConnection => _dbConnection;
    }
}
