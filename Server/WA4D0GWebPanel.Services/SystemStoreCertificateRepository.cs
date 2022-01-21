using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using WA4D0GWebPanel.Models;

namespace WA4D0GWebPanel.Services
{
    public class SystemStoreCertificateRepository : ICertificateRepository
    {
        private Task<int> FindSubject(List<Subject> subjects, string subjectName)
        {
            for (int i = 0; i < subjects.Count; i++)
            {
                if (subjects[i].FullName == subjectName)
                {
                    return Task.FromResult(i);
                }
            }
            return Task.FromResult(-1);
        }

        public async Task<IEnumerable<Subject>> GetSubjectsList()
        {
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            List<Subject> subjects = new List<Subject>();
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificatesCollection = store.Certificates;
            foreach (X509Certificate x509Certificate in certificatesCollection)
            {
                using (X509Certificate2 x509 = new X509Certificate2(x509Certificate.GetRawCertData()))
                {
                    Certificate certificateData = new Certificate();
                    certificateData.Hash = x509.GetCertHashString();
                    certificateData.Algorithm = x509.GetKeyAlgorithm();
                    certificateData.StartDate = x509.NotBefore;
                    certificateData.EndDate = x509.NotAfter;

                    string subjectName = x509.Subject.Split(',')[0].Remove(0, 3);
                    int subjectIndex = await FindSubject(subjects, subjectName);
                    if (subjectIndex > -1)
                    {
                        //убрать в будущем, ID должны братья из БД
                        certificateData.ID = subjects[subjectIndex].Certificates.Count + 1;
                        //
                        subjects[subjectIndex].Certificates.Add(certificateData);
                    }
                    else
                    {
                        Subject subject = new Subject();
                        //убрать в будущем, ID должны братья из БД
                        subject.ID = subjects.Count + 1;
                        //
                        subject.Certificates = new List<Certificate>();
                        subject.FullName = subjectName;
                        //убрать в будущем, ID должны братья из БД
                        certificateData.ID = 1;
                        //
                        subject.Certificates.Add(certificateData);
                        subjects.Add(subject);
                    }
                }
            }
            certificatesCollection.Clear();
            return subjects;
        }

        public Task DeleteSubject(int subjectID)
        {
            throw new System.NotImplementedException();
        }

        public Task EditSubject(Subject subject)
        {
            throw new System.NotImplementedException();
        }
    }
}
