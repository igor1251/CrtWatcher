﻿using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using System.Linq;

namespace DataStructures
{
    public class HostsStorage : IHostsStorage
    {
        IDbContext _dbContext;
        IHostsStorageQueries _queryStore;

        public HostsStorage(IEnumerable<IDbContext> registeredDbContexts, IHostsStorageQueries queryStore)
        {
            _queryStore = queryStore;
            _dbContext = registeredDbContexts.FirstOrDefault(dbContext => dbContext.GetType() == typeof(HostsDbContext));
            if (_dbContext == null) throw new Exception("Implementation of 'HostsDbContext' not registered");
        }

        private async Task CheckDatabase()
        {
            if (!File.Exists(_dbContext.DbPath))
            {
                await _dbContext.DbConnection.ExecuteAsync(_queryStore.CreateTablesQuery);
            }
        }

        public async Task<IEnumerable<IClientHost>> GetClientHosts()
        {
            await CheckDatabase();
            var hosts = await _dbContext.DbConnection.QueryAsync<ClientHost>(_queryStore.GetClientHostsQuery);
            return hosts;
        }

        public async Task AddClientHost(ClientHost host)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.ExecuteAsync(_queryStore.AddClientHostQuery, host);
        }

        public async Task DeleteClientHost(ClientHost host)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.ExecuteAsync(_queryStore.DeleteClientHostQuery, new { IP = host.IP });
        }

        public async Task UpdateClientHost(ClientHost host)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.ExecuteAsync(_queryStore.UpdateClientHostQuery, host);
        }
    }
}