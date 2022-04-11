using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using X509Observer.Primitives.Database;
using X509Observer.Reporters;
using X509Observer.MagicStrings.DatabaseQueries;
using X509Observer.Primitives.Network;

namespace X509Observer.DatabaseOperators.Network
{
    public class ApiUsersRepository : IApiUsersRepository
    {
        IDbContext _dbContext;

        public ApiUsersRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddApiUserAsync(ApiUser user)
        {
            try
            {
                var apiKey = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApiUser>("SELECT * FROM [ApiUsers] WHERE UserName=@UserName AND PasswordHash=@PasswordHash", new
                {
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash
                });
                if (apiKey == null)
                {
                    await _dbContext.DbConnection.ExecuteAsync(ApiUsersRepositoryQueries.ADD_API_USER, new
                    {
                        UserName = user.UserName,
                        PasswordHash = user.PasswordHash
                    });
                }
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("", ex);
            }
        }

        public async Task<ApiUser> GetApiUserByIDAsync(int userID)
        {
            try
            {
                var user = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApiUser>(ApiUsersRepositoryQueries.GET_API_USER_BY_ID, new
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

        public async Task<List<ApiUser>> GetApiUsersAsync()
        {
            try
            {
                var users = (await _dbContext.DbConnection.QueryAsync<ApiUser>(ApiUsersRepositoryQueries.GET_API_USERS)).ToList();
                return users;
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("", ex);
            }
            return null;
        }

        public async Task RemoveApiUserAsync(int userID)
        {
            try
            {
                await _dbContext.DbConnection.ExecuteAsync(ApiUsersRepositoryQueries.REMOVE_API_USER, new
                {
                    ID = userID
                });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("", ex);
            }
        }

        public async Task UpdateApiUserAsync(ApiUser user)
        {
            try
            {
                await _dbContext.DbConnection.ExecuteAsync(ApiUsersRepositoryQueries.UPDATE_API_USER, new
                {
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash
                });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("", ex);
            }
        }
    }
}
