﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.IO;
using ElectronicDigitalSignatire.Services.Interfaces;
using ElectronicDigitalSignatire.Models.Classes;
using ElectronicDigitalSignatire.Models.Interfaces;

namespace ElectronicDigitalSignatire.Services.Classes
{
    public class DbStore : IDbStore
    {
        IQueryStore _queryStore;
        IDbContext _dbContext;

        public DbStore(IQueryStore queryStore, IDbContext dbContext)
        {
            _queryStore = queryStore;
            _dbContext = dbContext;
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

        public async Task DeleteUser(int subjectID)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.QueryAsync(_queryStore.DeleteUser, new { ID = subjectID });
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
            var subjects = (await _dbContext.DbConnection.QueryAsync<User>(_queryStore.GetUsers)).AsList();
            return subjects;
        }

        public async Task<User> GetUserByID(int id)
        {
            await CheckDatabase();
            var subject = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE ID=@ID", new { ID = id });
            if (subject == null) return null;            
            subject.CertificateList = await GetCertificates(subject.ID);
            return subject;
        }

        public async Task InsertCertificate(Certificate certificate, int subjectID)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.QueryAsync(_queryStore.InsertCertificate, new 
            { 
                SubjectID = subjectID, 
                CertificateHash = certificate.CertificateHash, 
                Algorithm = certificate.Algorithm, 
                StartDate = certificate.StartDate, 
                EndDate = certificate.EndDate
            });
        }

        public async Task InsertUser(User subject)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.ExecuteAsync(_queryStore.InsertUser, subject);
            int subjectID = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<int>("SELECT ID FROM Subjects WHERE SubjectName=@Name", new { Name = subject.UserName });
            foreach (var item in subject.CertificateList)
            {
                await InsertCertificate(item, subjectID);
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

        public async Task UpdateUser(User subject)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.QueryAsync(_queryStore.UpdateUser, subject);
        }
    }
}