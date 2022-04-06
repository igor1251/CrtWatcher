using System.Collections.Generic;
using Dapper;
using System;
using System.Threading.Tasks;
using X509Observer.Primitives.Database;
using X509Observer.Primitives.Network;
using X509Observer.MagicStrings.DatabaseQueries;
using X509Observer.Reporters;

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
            try
            {
                await _dbContext.DbConnection.ExecuteAsync(ClientDesktopsRepositoryQueries.ADD_CLIENT_DESKTOP, clientDesktop);
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("AddClientDesktopAsync(ClientDesktop clientDesktop)", ex);
            }
        }

        public async Task<ClientDesktop> GetClientDesktopByIDAsync(int ID)
        {
            var clientDesktop = new ClientDesktop();
            try
            {
                clientDesktop = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ClientDesktop>(ClientDesktopsRepositoryQueries.GET_CLIENT_DESKTOP_BY_ID, ID);
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetClientDesktopByIDAsync(int ID)", ex);
            }
            return clientDesktop;
        }

        public async Task<ClientDesktop> GetClientDesktopByIPAsync(string IP)
        {
            var clientDesktop = new ClientDesktop();
            try
            {
                clientDesktop = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ClientDesktop>(ClientDesktopsRepositoryQueries.GET_CLIENT_DESKTOP_BY_IP, IP);
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetClientDesktopByIPAsync(string IP)", ex);
            }
            return clientDesktop;
        }

        public async Task<ClientDesktop> GetClientDesktopByNameAsync(string name)
        {
            var clientDesktop = new ClientDesktop();
            try
            {
                clientDesktop = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ClientDesktop>(ClientDesktopsRepositoryQueries.GET_CLIENT_DESKTOP_BY_NAME, new { NAME = name });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetClientDesktopByNameAsync(string name)", ex);
            }
            return clientDesktop;
        }

        public async Task<List<ClientDesktop>> GetClientDesktopsAsync()
        {
            var clientDesktops = new List<ClientDesktop>();
            try
            {
                clientDesktops = (await _dbContext.DbConnection.QueryAsync<ClientDesktop>(ClientDesktopsRepositoryQueries.GET_CLIENT_DESKTOPS)).AsList();
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetClientDesktopsAsync()", ex);
            }
            return clientDesktops;
        }

        public async Task RemoveClientDesktopAsync(ClientDesktop clientDesktop)
        {
            try
            {
                await _dbContext.DbConnection.ExecuteAsync(ClientDesktopsRepositoryQueries.REMOVE_CLIENT_DESKTOP_BY_IP, new { IP = clientDesktop.IP });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("RemoveClientDesktopAsync(ClientDesktop clientDesktop)", ex);
            }
        }
    }
}
