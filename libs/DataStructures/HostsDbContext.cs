﻿using System;
using System.Data.SQLite;

namespace DataStructures
{
    public class HostsDbContext : IDbContext
    {
        private readonly static string _dbPath = Environment.CurrentDirectory + "\\hostsdb.sqlite", _connectionString = "Data Source=" + _dbPath;
        private SQLiteConnection _connection;

        public string DbPath
        {
            get => _dbPath;
        }

        public SQLiteConnection DbConnection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SQLiteConnection(_connectionString);
                }
                return _connection;
            }
        }
    }
}