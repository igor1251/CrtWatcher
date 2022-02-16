using HostsRegistrationService.Models.Classes;
using HostsRegistrationService.Models.Interfaces;
using HostsRegistrationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace HostsRegistrationService.Services.Classes
{
    public class HostStore : IHostStore
    {
        IDbContext _dbContext;

        public HostStore(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<IClientHost>> GetClientHosts()
        {
            var hosts = await _dbContext.DbConnection.QueryAsync<ClientHost>("SELECT * FROM [RegisteredHosts]");
            return hosts;
        }

        public async Task AddClientHost(IClientHost host)
        {
            await _dbContext.DbConnection.ExecuteAsync("INSERT INTO [RegisteredHosts] (HostName, IP, ConnectionPort) VALUES (@HostName, @IP, @ConnectionPort)", host);
        }

        public async Task DeleteClientHost(IClientHost host)
        {
            await _dbContext.DbConnection.ExecuteAsync("DELETE FROM [RegisteredHosts] WHERE IP = @IP", new { IP = host.IP });
        }
    }
}
