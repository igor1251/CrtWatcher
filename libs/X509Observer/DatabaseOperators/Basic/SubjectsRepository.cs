using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using X509Observer.Primitives.Basic;
using X509Observer.Primitives.Database;
using System.Security.Cryptography.X509Certificates;
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

        private async Task<bool> DigitalFingerprintExists(string hash)
        {
            var fingerprint = await _dbContext.DbConnection.QueryFirstOrDefault(SubjectsRepositoryQueries.GET_DIGITAL_FINGERPRINT_BY_HASH, new { Hash = hash });
            if (fingerprint == null) return false;
            return true;
        }

        public async Task AddDigitalFingerprintAsync(DigitalFingerprint digitalFingerprint, int subjectID)
        {
            if (!await DigitalFingerprintExists(digitalFingerprint.Hash))
                await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.ADD_DIGITAL_FINGERPRINT, new { SubjectID = subjectID, Hash = digitalFingerprint.Hash, Start = digitalFingerprint.Start, End = digitalFingerprint.End });
        }

        public async Task<List<DigitalFingerprint>> GetDigitalFingerprintsBySubjectIDAsync(int subjectID)
        {
            var digitalFingerprints = (await _dbContext.DbConnection.QueryAsync<DigitalFingerprint>(SubjectsRepositoryQueries.GET_DIGITAL_FINGERPRINTS_BY_SUBJECT_ID, new { SubjectID = subjectID })).ToList();
            return digitalFingerprints;
        }

        private async Task<int> IndexOfSubject(Subject subject)
        {
            int subjectIDInDatabase = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<int>(SubjectsRepositoryQueries.GET_SUBJECT_ID, new { Name = subject.Name });
            return subjectIDInDatabase;
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            var subjectIDInDatabase = await IndexOfSubject(subject);
            if (subjectIDInDatabase <= 0)
            {
                await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.ADD_SUBJECT, new { Name = subject.Name, Phone = subject.Phone });
                subjectIDInDatabase = await IndexOfSubject(subject);
            }
            foreach (var fingerprint in subject.Fingerprints)
            {
                await AddDigitalFingerprintAsync(fingerprint, subjectIDInDatabase);
            }
        }

        public async Task AddSubjectAsync(List<Subject> subjects)
        {
            foreach (var subject in subjects)
            {
                await AddSubjectAsync(subject);
            }
        }

        public async Task<Subject> GetSubjectByIDAsync(int ID)
        {
            var subject = await _dbContext.DbConnection.QueryFirstOrDefaultAsync(SubjectsRepositoryQueries.GET_SUBJECT_BY_ID, ID);
            subject.Fingerprints = await GetDigitalFingerprintsBySubjectIDAsync(subject.ID);
            return subject;
        }

        public async Task<List<Subject>> GetSubjectsAsync()
        {
            var subjects = (await _dbContext.DbConnection.QueryAsync<Subject>(SubjectsRepositoryQueries.GET_SUBJECTS)).ToList();
            foreach (var item in subjects)
            {
                item.Fingerprints = await GetDigitalFingerprintsBySubjectIDAsync(item.ID);
            }
            return subjects;
        }

        private Task<int> FindSubject(List<Subject> subjects, string subjectName)
        {
            for (int i = 0; i < subjects.Count; i++)
            {
                if (subjects[i].Name == subjectName)
                {
                    return Task.FromResult(i);
                }
            }
            return Task.FromResult(-1);
        }

        public async Task<List<Subject>> GetSubjectsFromSystemStorageAsync()
        {
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            List<Subject> subjects = new List<Subject>();
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificatesCollection = store.Certificates;
            foreach (X509Certificate x509Certificate in certificatesCollection)
            {
                using (X509Certificate2 x509 = new X509Certificate2(x509Certificate.GetRawCertData()))
                {
                    DigitalFingerprint digitalFingerprint = new DigitalFingerprint(0, x509.GetCertHashString(), x509.NotBefore, x509.NotAfter);

                    string subjectName = "";
                    foreach (var item in x509.Subject.Split(','))
                    {
                        if (item.IndexOf("CN") > -1)
                        {
                            subjectName = item.Remove(0, 3);                                    // немного волшебства
                            if (subjectName.IndexOf('=') > -1)                                  //
                                subjectName = subjectName.Remove(subjectName.IndexOf('='), 1);  //
                        }
                    }

                    int subjectIndex = await FindSubject(subjects, subjectName);
                    if (subjectIndex > -1)
                    {
                        subjects[subjectIndex].Fingerprints.Add(digitalFingerprint);
                    }
                    else
                    {
                        Subject subject = new Subject(0, subjectName);
                        subject.Fingerprints.Add(digitalFingerprint);
                        subjects.Add(subject);
                    }
                }
            }
            certificatesCollection.Clear();
            return subjects;
        }

        public async Task RemoveDigitalFingerptintByIDAsync(int ID)
        {
            await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.REMOVE_DIGITAL_FINGERPRINT_BY_ID, ID);
        }

        public async Task RemoveSubjectByIDAsync(int ID)
        {
            await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.REMOVE_SUBJECT_BY_ID, ID);
            await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.REMOVE_DIGITAL_FINGERPRINTS_BY_SUBJECT_ID, ID);
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.UPDATE_SUBJECT, subject);
        }
    }
}
