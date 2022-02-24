using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ElectronicDigitalSignatire.Services.Interfaces
{
    public interface IUsersDbContext
    {
        string DbPath { get; }
        SQLiteConnection DbConnection { get; }
    }
}
