using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CrtLoader.Model.Interfaces
{
    public interface IDbContext
    {
        string DbPath { get; }
        SQLiteConnection DbConnection { get; }
    }
}
