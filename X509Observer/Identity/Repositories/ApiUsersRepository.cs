﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using X509Observer.Reporters;
using X509Observer.MagicStrings.DatabaseQueries;
using X509Observer.Common.Contexts;
using X509Observer.Identity.Entities;

namespace X509Observer.Identity.Repositories
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
                var isApiUserExists = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<bool>(ApiUsersRepositoryQueries.IS_API_USER_EXISTS, new
                {
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash
                });
                if (!isApiUserExists)
                {
                    await _dbContext.DbConnection.ExecuteAsync(ApiUsersRepositoryQueries.ADD_API_USER, new
                    {
                        UserName = user.UserName,
                        PasswordHash = user.PasswordHash,
                        Role = user.Role
                    });
                }
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("AddApiUserAsync(ApiUser user)", ex);
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

        public async Task<ApiUser> GetApiUserByUserNameAsync(string username)
        {
            try
            {
                var user = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApiUser>(ApiUsersRepositoryQueries.GET_API_USER_BY_USERNAME, new
                {
                    UserName = username
                });
                return user;
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetApiUserByUserNameAsync(string username)", ex);
            }
            return null;
        }

        public async Task<int> GetApiUserIDAsync(string username)
        {
            try
            {
                var userID = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<int>(ApiUsersRepositoryQueries.GET_API_USER_ID, new
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
                    PasswordHash = user.PasswordHash,
                    Role = user.Role
                });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("", ex);
            }
        }

        public async Task<bool> IsApiUserExistsAsync(ApiUser user)
        {
            try
            {
                var result = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<bool>(ApiUsersRepositoryQueries.IS_API_USER_EXISTS, new
                {
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash
                });
                return result;
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("IsApiUserExistsAsync(ApiUser user)", ex);
            }
            return false;
        }
    }
}