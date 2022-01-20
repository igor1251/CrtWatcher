using System.Data.SQLite;

namespace CrtAdminPanel.Models.Interfaces
{
    public interface ISQLiteDbContext : IDbContext
    {
        SQLiteConnection Connection { get; }
    }
}
