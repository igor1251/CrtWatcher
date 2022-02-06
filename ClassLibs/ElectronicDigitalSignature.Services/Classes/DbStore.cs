using System.Collections.Generic;
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

        public async Task DeleteSubject(int subjectID)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.QueryAsync(_queryStore.DeleteSubject, new { ID = subjectID });
        }

        public async Task<List<CertificateData>> GetCertificates(int subjectID)
        {
            await CheckDatabase();
            var certificates = (await _dbContext.DbConnection.QueryAsync<CertificateData>(_queryStore.GetCertificates, new { ID = subjectID })).AsList();
            return certificates;
        }

        public async Task<List<CertificateSubject>> GetSubjects()
        {
            await CheckDatabase();
            var subjects = (await _dbContext.DbConnection.QueryAsync<CertificateSubject>(_queryStore.GetSubjects)).AsList();
            return subjects;
        }

        public async Task<CertificateSubject> GetSubjectByID(int id)
        {
            await CheckDatabase();
            var subject = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<CertificateSubject>("SELECT * FROM Subjects WHERE ID=@ID", new { ID = id });
            if (subject == null) return null;            
            subject.CertificateList = await GetCertificates(subject.ID);
            return subject;
        }

        public async Task InsertCertificate(CertificateData certificate, int subjectID)
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

        public async Task InsertSubject(CertificateSubject subject)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.ExecuteAsync(_queryStore.InsertSubject, subject);
            int subjectID = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<int>("SELECT ID FROM Subjects WHERE SubjectName=@Name", new { Name = subject.SubjectName });
            foreach (var item in subject.CertificateList)
            {
                await InsertCertificate(item, subjectID);
            }
        }

        public async Task InsertSubject(List<CertificateSubject> subjects)
        {
            await _dbContext.DbConnection.OpenAsync();
            foreach (var item in subjects)
            {
                await InsertSubject(item);
            }
            await _dbContext.DbConnection.CloseAsync();
        }

        public async Task UpdateSubject(CertificateSubject subject)
        {
            await CheckDatabase();
            await _dbContext.DbConnection.QueryAsync(_queryStore.UpdateSubject, subject);
        }
    }
}
