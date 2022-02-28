using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using System.Linq;
using System.IO;

namespace DataStructures
{
    public class UsersStorage : IUsersStorage
    {
        IUsersStorageQueries _queryStore;
        IDbContext _dbContext;

        public UsersStorage(IUsersStorageQueries queryStore, IEnumerable<IDbContext> registeredDbContexts)
        {
            _queryStore = queryStore;
            _dbContext = registeredDbContexts.FirstOrDefault(dbContext => dbContext.GetType() == typeof(UsersDbContext));
            if (_dbContext == null) throw new Exception("Implementation of 'UsersDbContext' not registered");
        }

        private async Task CheckDatabase()
        {
            if (!File.Exists(_dbContext.DbPath))
            {
                await _dbContext.DbConnection.QueryAsync(_queryStore.CreateTables);
            }
        }

        public async Task DeleteCertificate(int certificateID)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.QueryAsync(_queryStore.DeleteCertificate, new { ID = certificateID });
        }

        public async Task DeleteUser(int userID)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.QueryAsync(_queryStore.DeleteUser, new { ID = userID });
        }

        public async Task<List<Certificate>> GetCertificates(int subjectID)
        {
            await CheckDatabase();
            var certificates = (await _dbContext.DbConnection.QueryAsync<Certificate>(_queryStore.GetCertificates, new { ID = subjectID })).AsList();
            return certificates;
        }

        public async Task<List<User>> GetUsers()
        {
            await CheckDatabase();
            var users = (await _dbContext.DbConnection.QueryAsync<User>(_queryStore.GetUsers)).AsList();
            return users;
        }

        public async Task<User> GetUserByID(int id)
        {
            await CheckDatabase();
            var user = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE ID=@ID", new { ID = id });
            if (user == null) return null;            
            user.CertificateList = await GetCertificates(user.ID);
            return user;
        }

        public async Task InsertCertificate(Certificate certificate, int userID)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.QueryAsync(_queryStore.InsertCertificate, new 
            { 
                UserID = userID, 
                CertificateHash = certificate.CertificateHash, 
                Algorithm = certificate.Algorithm, 
                StartDate = certificate.StartDate, 
                EndDate = certificate.EndDate
            });
        }

        private bool CertificateExists(Certificate certificate, List<Certificate> alreadyRegisteredCertificates)
        {
            foreach (var item in alreadyRegisteredCertificates)
            {
                if (item.CertificateHash == certificate.CertificateHash)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task InsertUser(User user)
        {
            await CheckDatabase();

            var alreadyRegisteredUser = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<User>("SELECT ID FROM Users WHERE UserName = @Name", new { Name = user.UserName });
            if (alreadyRegisteredUser == null)
            {
                await _dbContext.DbConnection.ExecuteAsync(_queryStore.InsertUser, user);
                int userID = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<int>("SELECT ID FROM Users WHERE UserName=@Name", new { Name = user.UserName });
                foreach (var item in user.CertificateList)
                {
                    await InsertCertificate(item, userID);
                }
            }
            else
            {
                foreach (var certificate in user.CertificateList)
                {
                    if (!CertificateExists(certificate, alreadyRegisteredUser.CertificateList))
                    {
                        await InsertCertificate(certificate, alreadyRegisteredUser.ID);
                    }
                }
            }
        }

        public async Task InsertUser(List<User> subjects)
        {
            await _dbContext.DbConnection.OpenAsync();
            foreach (var item in subjects)
            {
                await InsertUser(item);
            }
            await _dbContext.DbConnection.CloseAsync();
        }

        public async Task UpdateUser(User user)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.QueryAsync(_queryStore.UpdateUser, user);
        }
    }
}
