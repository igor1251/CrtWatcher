using System.Data.SQLite;

namespace WA4D0G.Model.Interfaces
{
    public interface ISQLiteDbContext : IDbContext
    {
        SQLiteConnection Connection { get; }
    }
}
