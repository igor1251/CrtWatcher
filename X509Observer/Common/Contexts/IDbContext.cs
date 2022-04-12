using System.Data.SQLite;

namespace X509Observer.Common.Contexts
{
    public interface IDbContext
    {
        string DbPath { get; }
        SQLiteConnection DbConnection { get; }
    }
}
