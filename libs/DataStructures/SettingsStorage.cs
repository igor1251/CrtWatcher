using Dapper;
using System.Threading.Tasks;
using System.IO;

namespace DataStructures
{
    public class SettingsStorage : ISettingsStorage
    {
        IDbContext _dbContext;
        IBaseStorageQueries _queryStore;

        //private ClientHost GetHostInfo()
        //{
        //    var host = new ClientHost();
        //    host.HostName = Dns.GetHostName();
        //    host.ConnectionPort = 5000;

        //    foreach (IPAddress ip in Dns.GetHostAddresses(host.HostName))
        //    {
        //        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //        {
        //            host.IP = ip.ToString();
        //            break;
        //        }
        //    }

        //    return host;
        //}

        public SettingsStorage(IDbContext dbContext,
                               IBaseStorageQueries queryStore)
        {
            _dbContext = dbContext;
            _queryStore = queryStore;
        }

        private async Task CheckDatabase()
        {
            if (!File.Exists(_dbContext.DbPath))
            {
                await _dbContext.DbConnection.ExecuteAsync(_queryStore.CreateTables);
            }
        }

        public async Task<Settings> LoadSettings()
        {
            await CheckDatabase();
            var settings = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<Settings>("SELECT * FROM [Settings]");
            return settings;
        }

        public async Task UpdateSettings(Settings settings)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.ExecuteAsync("UPDATE [Settings] SET VerificationFrequency = @VerificationFrequency, MainServerPort = @MainServerPort, MainServerIP = @MainServerIP", settings);
        }
    }
}
