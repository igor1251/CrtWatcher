using System.Data.SQLite;

namespace HostsRegistrationService.Services.Interfaces
{
    public interface IHostsDbContext
    {
        string DbPath { get; }
        SQLiteConnection DbConnection { get; }
    }
}
