using System.Data.SQLite;

namespace HostsRegistrationService.Services.Interfaces
{
    public interface IDbContext
    {
        SQLiteConnection DbConnection { get; }
    }
}
