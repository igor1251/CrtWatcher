using System.Collections.Generic;
using Dapper;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using X509Observer.MagicStrings.DatabaseQueries;
using X509Observer.Reporters;
using X509Observer.Common.Contexts;
using X509Observer.Common.Entities;

namespace X509Observer.Server.Repositories
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
            try
            {
                var fingerprint = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<DigitalFingerprint>(SubjectsRepositoryQueries.GET_DIGITAL_FINGERPRINT_BY_HASH, new { Hash = hash });
                if (fingerprint == null) return false;
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("DigitalFingerprintEcists(string hash)", ex);
            }
            return true;
        }

        public async Task AddDigitalFingerprintAsync(DigitalFingerprint digitalFingerprint, int subjectID)
        {
            try
            {
                if (!await DigitalFingerprintExists(digitalFingerprint.Hash))
                    await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.ADD_DIGITAL_FINGERPRINT, new { SubjectID = subjectID, Hash = digitalFingerprint.Hash, Start = digitalFingerprint.Start, End = digitalFingerprint.End });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("AddDigitalFingerprintAsync(DigitalFingerprint digitalFingerprint, int subjectID)", ex);
            }
        }

        public async Task<List<DigitalFingerprint>> GetDigitalFingerprintsBySubjectIDAsync(int subjectID)
        {
            var digitalFingerprints = new List<DigitalFingerprint>();
            try
            {
                digitalFingerprints = (await _dbContext.DbConnection.QueryAsync<DigitalFingerprint>(SubjectsRepositoryQueries.GET_DIGITAL_FINGERPRINTS_BY_SUBJECT_ID, new { SubjectID = subjectID })).ToList();
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetDigitalFingerprintsBySubjectIDAsync(int subjectID)", ex);
            }
            return digitalFingerprints;
        }

        private async Task<int> IndexOfSubject(Subject subject)
        {
            int subjectIDInDatabase = 0;
            try
            {
                subjectIDInDatabase = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<int>(SubjectsRepositoryQueries.GET_SUBJECT_ID, new { Name = subject.Name });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("IndexOfSubject(Subject subject)", ex);
            }
            return subjectIDInDatabase;
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            try
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
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("AddSubjectAsync(Subject subject)", ex);
            }
        }

        public async Task AddSubjectAsync(List<Subject> subjects)
        {
            foreach (var subject in subjects)
            {
                await AddSubjectAsync(subject);
            }
        }

        public async Task<Subject> GetSubjectByIDAsync(int subjectID)
        {
            var subject = new Subject();
            try
            {
                subject = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<Subject>(SubjectsRepositoryQueries.GET_SUBJECT_BY_ID, new { ID = subjectID });
                subject.Fingerprints = await GetDigitalFingerprintsBySubjectIDAsync(subject.ID);
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetSubjectByIDAsync(int subjectID)", ex);
            }
            return subject;
        }

        public async Task<List<Subject>> GetSubjectsAsync()
        {
            var subjects = new List<Subject>();
            try
            {
                subjects = (await _dbContext.DbConnection.QueryAsync<Subject>(SubjectsRepositoryQueries.GET_SUBJECTS)).AsList();
                foreach (var item in subjects)
                {
                    item.Fingerprints = await GetDigitalFingerprintsBySubjectIDAsync(item.ID);
                }
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetSubjectsAsync()", ex);
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
            var subjects = new List<Subject>();
            try
            {
                var store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                var certificatesCollection = store.Certificates;
                foreach (var x509Certificate in certificatesCollection)
                {
                    using (var x509 = new X509Certificate2(x509Certificate.GetRawCertData()))
                    {
                        var digitalFingerprint = new DigitalFingerprint() { Hash = x509.GetCertHashString(), Start = x509.NotBefore, End = x509.NotAfter };
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
                            var subject = new Subject() { Name = subjectName };
                            subject.Fingerprints.Add(digitalFingerprint);
                            subjects.Add(subject);
                        }
                    }
                }
                certificatesCollection.Clear();
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetSubjectsFromSystemStorageAsync()", ex);
            }
            return subjects;
        }

        public async Task RemoveDigitalFingerptintByIDAsync(int fingerprintID)
        {
            try
            {
                await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.REMOVE_DIGITAL_FINGERPRINT_BY_ID, new { ID = fingerprintID });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("RemoveDigitalFingerptintByIDAsync(int fingerprintID)", ex);
            }
        }

        public async Task RemoveSubjectByIDAsync(int subjectID)
        {
            try
            {
                await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.REMOVE_SUBJECT_BY_ID, new { ID = subjectID });
                await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.REMOVE_DIGITAL_FINGERPRINTS_BY_SUBJECT_ID, new { ID = subjectID });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("RemoveSubjectByIDAsync(int subjectID)", ex);
            }
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            try
            {
                await _dbContext.DbConnection.ExecuteAsync(SubjectsRepositoryQueries.UPDATE_SUBJECT, new
                {
                    Name = subject.Name,
                    Phone = subject.Phone,
                    ID = subject.ID
                });
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("UpdateSubjectAsync(Subject subject)", ex);
            }
        }
    }
}
