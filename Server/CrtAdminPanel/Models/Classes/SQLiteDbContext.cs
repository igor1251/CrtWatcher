using System;
using System.Data.SQLite;
using CrtAdminPanel.Models.Interfaces;

namespace CrtAdminPanel.Models.Classes
{
    public class SQLiteDbContext : ISQLiteDbContext
    {
        string _databaseFile;
        SQLiteConnection _connection;

        public SQLiteDbContext()
        {
            _databaseFile = Environment.CurrentDirectory + "\\Db\\keys.sqlite";
            _connection = new SQLiteConnection("Data Source=" + _databaseFile);
        }

        public string DatabaseFile
        {
            get => _databaseFile;
        }

        public SQLiteConnection Connection
        {
            get => _connection;
        }
    }
}
