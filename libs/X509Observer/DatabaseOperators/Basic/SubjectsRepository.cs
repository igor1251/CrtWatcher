using System.Collections.Generic;
using Dapper;
using System.Threading.Tasks;
using X509Observer.Primitives.Basic;
using X509Observer.Primitives.Database;
using X509Observer.MagicStrings.DatabaseQueries;

namespace X509Observer.DatabaseOperators.Basic
{
    public class SubjectsRepository : ISubjectsRepository
    {
        private IDbContext _dbContext;

        public SubjectsRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddDigitalFingerprintAsync(DigitalFingerprint digitalFingerprint)
        {
            await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.ADD_DIGITAL_FINGERPRINT, digitalFingerprint);
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.ADD_SUBJECT, subject);
        }

        public async Task AddSubjectAsync(IEnumerable<Subject> subjects)
        {
            foreach (var subject in subjects)
            {
                await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.ADD_SUBJECT, subject);
            }
        }

        public async Task<IEnumerable<DigitalFingerprint>> GetDigitalFingerprintsAsync()
        {
            var digitalFingerprints = await _dbContext.DbConnection.QueryAsync<DigitalFingerprint>(SubjectsRepositoryQueries.GET_DIGITAL_FINGERPRINTS);
            return digitalFingerprints;
        }

        public async Task<Subject> GetSubjectByIDAsync(int ID)
        {
            var subject = await _dbContext.DbConnection.QueryFirstOrDefaultAsync(SubjectsRepositoryQueries.GET_SUBJECT_BY_ID, ID);
            return subject;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync()
        {
            var subjects = await _dbContext.DbConnection.QueryAsync<Subject>(SubjectsRepositoryQueries.GET_SUBJECTS);
            return subjects;
        }

        public async Task RemoveDigitalFingerptintByIDAsync(int ID)
        {
            await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.REMOVE_DIGITAL_FINGERPRINT_BY_ID, ID);
        }

        public async Task RemoveSubjectByIDAsync(int ID)
        {
            await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.REMOVE_SUBJECT_BY_ID, ID);
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.UPDATE_SUBJECT, subject);
        }
    }
}
