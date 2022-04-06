using System.Threading.Tasks;
using System.IO;
using Dapper;
using X509Observer.Primitives.Database;
using X509Observer.MagicStrings.DatabaseQueries;

namespace X509Observer.DatabaseOperators.MaintananceTools
{
    public class DatabaseVerificationTool : IDatabaseVerificationTool
    {
        private IDbContext _dbContext;

        public DatabaseVerificationTool(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CheckDatabaseFileExists()
        {
            return File.Exists(_dbContext.DbPath);
        }

        public async Task CreateDatabaseAsync()
        {
            await _dbContext.DbConnection.ExecuteAsync(CommonRepositoriesQueries.CREATE_DATABASE);
        }
    }
}
