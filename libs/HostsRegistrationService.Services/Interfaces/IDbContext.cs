using System.Data.SQLite;

namespace HostsRegistration.Services.Interfaces
{
    public interface IDbContext
    {
        string DbPath { get; }
        SQLiteConnection DbConnection { get; }
    }
}
