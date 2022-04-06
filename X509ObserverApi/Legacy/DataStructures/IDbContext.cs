using System.Data.SQLite;

namespace DataStructures
{
    public interface IDbContext
    {
        string DbPath { get; }
        SQLiteConnection DbConnection { get; }
    }
}
