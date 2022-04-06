using System.Data.SQLite;

namespace X509Observer.Primitives.Database
{
    public interface IDbContext
    {
        string DbPath { get; }
        SQLiteConnection DbConnection { get; }
    }
}
