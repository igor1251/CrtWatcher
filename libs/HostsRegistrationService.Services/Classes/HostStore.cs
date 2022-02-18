using HostsRegistrationService.Models.Classes;
using HostsRegistrationService.Models.Interfaces;
using HostsRegistrationService.Services.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace HostsRegistrationService.Services.Classes
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
