using System.Data.SQLite;

namespace HostsRegistrationService.Services.Interfaces
{
    public interface IDbContext
    {
        string DbPath { get; }
        SQLiteConnection DbConnection { get; }
    }
}
