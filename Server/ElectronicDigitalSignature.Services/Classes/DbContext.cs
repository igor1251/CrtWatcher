using System;
using System.Data.SQLite;
using ElectrnicDigitalSignatire.Services.Interfaces;

namespace ElectrnicDigitalSignatire.Services.Classes
{
    public class DbContext : IDbContext
    {
        SQLiteConnection _dbConnection;
        string _dbPath = Environment.CurrentDirectory + "\\Data\\db\\keysdb.sqlite";

        public DbContext()
        {
            _dbConnection = new SQLiteConnection("Data Source=" + _dbPath);
        }

        public string DbPath => _dbPath;
        public SQLiteConnection DbConnection => _dbConnection;
    }
}
