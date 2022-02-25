using System;
using System.Data.SQLite;

namespace DataStructures
{
    public class UsersDbContext : IDbContext
    {
        SQLiteConnection _dbConnection;
        string _dbPath = Environment.CurrentDirectory + "\\usersdb.sqlite";

        public UsersDbContext()
        {
            _dbConnection = new SQLiteConnection("Data Source=" + _dbPath);
        }

        public string DbPath => _dbPath;
        public SQLiteConnection DbConnection => _dbConnection;
    }
}
