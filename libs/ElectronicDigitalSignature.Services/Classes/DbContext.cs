using System;
using System.Data.SQLite;
using ElectronicDigitalSignatire.Services.Interfaces;

namespace ElectronicDigitalSignatire.Services.Classes
{
    public class DbContext : IDbContext
    {
        SQLiteConnection _dbConnection;
        string _dbPath = Environment.CurrentDirectory + "\\keysdb.sqlite";

        public DbContext()
        {
            _dbConnection = new SQLiteConnection("Data Source=" + _dbPath);
        }

        public string DbPath => _dbPath;
        public SQLiteConnection DbConnection => _dbConnection;
    }
}
