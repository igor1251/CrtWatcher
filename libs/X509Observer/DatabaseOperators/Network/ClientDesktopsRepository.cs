using System.Collections.Generic;
using Dapper;
using System.Threading.Tasks;
using X509Observer.Primitives.Database;
using X509Observer.Primitives.Network;
using X509Observer.MagicStrings.DatabaseQueries;

namespace X509Observer.DatabaseOperators.Network
{
    public class ClientDesktopsRepository : IClientDesktopsRepository
    {
        private IDbContext _dbContext;

        public ClientDesktopsRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddClientDesktopAsync(ClientDesktop clientDesktop)
        {
            await _dbContext.DbConnection.ExecuteAsync(ClientDesktopsRepositoryQueries.ADD_CLIENT_DESKTOP, clientDesktop);
        }

        public async Task<ClientDesktop> GetClientDesktopByIDAsync(int ID)
        {
            var clientDesktop = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ClientDesktop>(ClientDesktopsRepositoryQueries.GET_CLIENT_DESKTOP_BY_ID, ID);
            return clientDesktop;
        }

        public async Task<ClientDesktop> GetClientDesktopByIPAsync(string IP)
        {
            var clientDesktop = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ClientDesktop>(ClientDesktopsRepositoryQueries.GET_CLIENT_DESKTOP_BY_IP, IP);
            return clientDesktop;
        }

        public async Task<ClientDesktop> GetClientDesktopByNameAsync(string name)
        {
            var clientDesktop = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ClientDesktop>(ClientDesktopsRepositoryQueries.GET_CLIENT_DESKTOP_BY_NAME, new { NAME = name });
            return clientDesktop;
        }

        public async Task<IEnumerable<ClientDesktop>> GetClientDesktopsAsync()
        {
            var clientDesktops = await _dbContext.DbConnection.QueryAsync<ClientDesktop>(ClientDesktopsRepositoryQueries.GET_CLIENT_DESKTOPS);
            return clientDesktops;
        }

        public async Task RemoveClientDesktopAsync(ClientDesktop clientDesktop)
        {
            await _dbContext.DbConnection.ExecuteAsync(ClientDesktopsRepositoryQueries.REMOVE_CLIENT_DESKTOP_BY_IP, new { IP = clientDesktop.IP });
        }
    }
}
