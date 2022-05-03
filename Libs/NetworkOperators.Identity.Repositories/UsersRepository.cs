using NetworkOperators.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Reporters;
using System.Data.SQLite;
using Dapper;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace NetworkOperators.Identity.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private SQLiteConnection connection;

        public UsersRepository(IConfiguration configuration)
        {
            var databaseFile = configuration["UsersDatabaseFile"];
            connection = new SQLiteConnection("Data Source=" + databaseFile);
            if (!File.Exists(databaseFile))
            {
                connection.Execute(Queries.CREATE_TABLES);
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                var isUserExists = await connection.QueryFirstOrDefaultAsync<bool>(Queries.IS_USER_EXISTS, new
                {
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash
                });
                if (!isUserExists)
                {
                    await connection.ExecuteAsync(Queries.ADD_USER, new
                    {
                        UserName = user.UserName,
                        PasswordHash = user.PasswordHash,
                        Permissions = user.Permissions
                    });
                }
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("AddApiUserAsync(ApiUser user)", ex);
            }
        }

        public async Task<User> GetUserByIDAsync(int userID)
        {
            try
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(Queries.GET_USER_BY_ID, new
                {
                    ID = userID
                });
                return user;
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("", ex);
            }
            return null;
        }

        public async Task<User> GetUserByAuthenticationDataAsync(string username, string passwordHash)
        {
            try
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(Queries.GET_USER_BY_AUTHENTICATION_DATA, new
                {
                    UserName = username,
                    PasswordHash = passwordHash
                });
                return user;
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetUserByAuthenticationDataAsync(string username)", ex);
            }
            return null;
        }

        public async Task<int> GetUserIDAsync(string username)
        {
            try
            {
                var userID = await connection.QueryFirstOrDefaultAsync<int>(Queries.GET_USER_ID, new
                {
                    UserName = username
                });
                return userID;
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetApiUserIDAsync(string username)", ex);
            }
            return -1;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var users = (await connection.QueryAsync<User>(Queries.GET_USERS)).ToList();
                return users;
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("", ex);
            }
            return null;
        }

        public async Task RemoveUserAsync(int userID)
        {
            try
            {
                await connection.ExecuteAsync(Queries.REMOVE_USER, new
                {
                    ID = userID
                });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("", ex);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                await connection.ExecuteAsync(Queries.UPDATE_USER, new
                {
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash,
                    Permissions = user.Permissions
                });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("", ex);
            }
        }

        public async Task<bool> IsUserExistsAsync(string username)
        {
            try
            {
                var result = await connection.QueryFirstOrDefaultAsync<bool>(Queries.IS_USER_EXISTS, new
                {
                    UserName = username,
                });
                return result;
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("IsApiUserExistsAsync(ApiUser user)", ex);
            }
            return false;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(Queries.GET_USER_BY_USERNAME, new
                {
                    UserName = username,
                });
                return user;
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetUserByUsernameAsync(string username)", ex);
            }
            return null;
        }
    }
}
