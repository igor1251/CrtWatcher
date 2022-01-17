using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.IO;
using CrtLoader.Model.Interfaces;

namespace CrtLoader.Model.Classes
{
    internal class DbStore : IDbStore
    {
        IQueryStore _queryStore;
        IDbContext _dbContext;

        public DbStore(IQueryStore queryStore, IDbContext dbContext)
        {
            _queryStore = queryStore;
            _dbContext = dbContext;
        }

        public async Task<List<CertificateData>> GetCertificateData()
        {
            /*
            if (!File.Exists(_dbContext.DbPath)) throw new FileNotFoundException("Database file not found.");
            var certificates = (await _dbContext.DbConnection.QueryAsync<CertificateData, CertificateSubject, CertificateData>(_queryStore.GetCertificates, (cert, subj) =>
            {
                cert.Subject = subj;
                return cert;
            })).AsList();
            return certificates;
            */
            return null;
        }

        public async Task<List<CertificateSubject>> GetCertificateSubjects()
        {
            if (!File.Exists(_dbContext.DbPath)) throw new FileNotFoundException("Database file not found.");
            var subjects = (await _dbContext.DbConnection.QueryAsync<CertificateSubject>(_queryStore.GetSubjects)).AsList();
            return subjects;
        }

        public async Task Delete(ICertificateData certificate)
        {
            if (!File.Exists(_dbContext.DbPath)) throw new FileNotFoundException("Database file not found.");
            await _dbContext.DbConnection.QueryAsync(_queryStore.DeleteCertificate, certificate);
        }

        public async Task Delete(ICertificateSubject subject)
        {
            if (!File.Exists(_dbContext.DbPath)) throw new FileNotFoundException("Database file not found.");
            await _dbContext.DbConnection.QueryAsync(_queryStore.DeleteCertificateSubject, subject);
        }

        public async Task Insert(ICertificateData certificate)
        {
            //нормально не заработает, обработать случаи добавления субъекта, если его нет и добавления ID, если есть
            if (!File.Exists(_dbContext.DbPath)) throw new FileNotFoundException("Database file not found.");
            await _dbContext.DbConnection.QueryAsync(_queryStore.InsertCertificate, certificate);
        }

        public async Task Insert(List<ICertificateData> certificates)
        {
            //нормально не заработает, обработать случаи добавления субъекта, если его нет и добавления ID, если есть
            if (!File.Exists(_dbContext.DbPath)) throw new FileNotFoundException("Database file not found.");
            await _dbContext.DbConnection.OpenAsync();
            foreach (var item in certificates)
            {
                await _dbContext.DbConnection.QueryAsync(_queryStore.InsertCertificate, item);
            }
            await _dbContext.DbConnection.CloseAsync();
        }

        public async Task Insert(ICertificateSubject subject)
        {
            if (!File.Exists(_dbContext.DbPath)) throw new FileNotFoundException("Database file not found.");
            await _dbContext.DbConnection.QueryAsync(_queryStore.InsertCertificateSubject, subject);
        }

        public async Task Update(ICertificateSubject subject)
        {
            if (!File.Exists(_dbContext.DbPath)) throw new FileNotFoundException("Database file not found.");
            await _dbContext.DbConnection.QueryAsync(_queryStore.UpdateCertificateSubject, subject);
        }
    }
}
