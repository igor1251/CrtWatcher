using Dapper;
using System.Threading.Tasks;
using System.IO;

namespace DataStructures
{
    public class SettingsStorage : ISettingsStorage
    {
        IDbContext _dbContext;
        IBaseStorageQueries _queryStore;

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
            if (settings == null)
            {
                await UpdateSettings(settings);
                return new Settings();
            }
            return settings;
        }

        public async Task UpdateSettings(Settings settings)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.ExecuteAsync("DELETE FROM [Settings]");
            await _dbContext.DbConnection.ExecuteAsync("INSERT INTO [Settings] (VerificationFrequency, MainServerPort, MainServerIP) VALUES (@VerificationFrequency, @MainServerPort, @MainServerIP);", new
            {
                VerificationFrequency = settings.VerificationFrequency,
                MainServerPort = settings.MainServerPort,
                MainServerIP = settings.MainServerIP
            });
        }
    }
}
