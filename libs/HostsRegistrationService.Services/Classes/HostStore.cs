using HostsRegistration.Models.Classes;
using HostsRegistration.Models.Interfaces;
using HostsRegistration.Services.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace HostsRegistration.Services.Classes
{
    public class HostStore : IHostStore
    {
        IDbContext _dbContext;
        IQueryStore _queryStore;

        public HostStore(IDbContext dbContext, IQueryStore queryStore)
        {
            _queryStore = queryStore;
            _dbContext = dbContext;
        }

        private async Task CheckDatabase()
        {
            if (!File.Exists(_dbContext.DbPath))
            {
                await _dbContext.DbConnection.ExecuteAsync(_queryStore.CreateTablesQuery);
            }
        }

        public async Task<IEnumerable<IClientHost>> GetHosts()
        {
            await CheckDatabase();
            var hosts = await _dbContext.DbConnection.QueryAsync<ClientHost>(_queryStore.GetClientHostsQuery);
            return hosts;
        }

        public async Task AddHost(ClientHost host)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.ExecuteAsync(_queryStore.AddClientHostQuery, host);
        }

        public async Task DeleteHost(ClientHost host)
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
