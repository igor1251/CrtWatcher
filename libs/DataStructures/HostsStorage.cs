using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;

namespace DataStructures
{
    public class HostsStorage : IHostsStorage
    {
        IDbContext _dbContext;
        IHostsStorageQueries _queryStore;

        public HostsStorage(IDbContext dbContext, IHostsStorageQueries queryStore)
        {
            _queryStore = queryStore;
            _dbContext = dbContext;
            if (_dbContext == null) throw new Exception("Implementation of 'DbContext' not registered");
        }

        private async Task CheckDatabase()
        {
            if (!File.Exists(_dbContext.DbPath))
            {
                await _dbContext.DbConnection.ExecuteAsync(_queryStore.CreateTables);
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

            var alreadyRegisteredHost = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ClientHost>("SELECT * FROM [RegisteredHosts] WHERE IP = @IP", new { IP = host.IP });
            if (alreadyRegisteredHost == null)
            {
                await _dbContext.DbConnection.ExecuteAsync(_queryStore.AddClientHostQuery, host);
            }
            else if (alreadyRegisteredHost.HostName != host.HostName || alreadyRegisteredHost.ConnectionPort != host.ConnectionPort)
            {
                await _dbContext.DbConnection.ExecuteAsync("UPDATE [RegisteredHosts] SET HostName = @HostName, ConnectionPort = @ConnectionPort WHERE IP = @IP", host);   
            }
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
