using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace DataStructures
{
    public class LocalUsersStore : ILocalUsersStore
    {
        public Task InsertCertificate(ICertificate certificate)
        {
            throw new NotImplementedException();
        }

        private Task<int> FindSubject(List<User> subjects, string subjectName)
        {
            for (int i = 0; i < subjects.Count; i++)
            {
                if (subjects[i].UserName == subjectName)
                {
                    return Task.FromResult(i);
                }
            }
            return Task.FromResult(-1);
        }

        public async Task<List<User>> LoadCertificateSubjectsAndCertificates()
        {
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            List<User> subjects = new List<User>();
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificatesCollection = store.Certificates;
            foreach (X509Certificate x509Certificate in certificatesCollection)
            {
                using (X509Certificate2 x509 = new X509Certificate2(x509Certificate.GetRawCertData()))
                {
                    Certificate certificateData = new Certificate();
                    certificateData.CertificateHash = x509.GetCertHashString();
                    certificateData.Algorithm = x509.GetKeyAlgorithm();
                    certificateData.StartDate = x509.NotBefore;
                    certificateData.EndDate = x509.NotAfter;

                    string subjectName = x509.Subject.Split(',')[0].Remove(0, 3);
                    int subjectIndex = await FindSubject(subjects, subjectName);
                    if (subjectIndex > -1)
                    {
                        certificateData.ID = subjects[subjectIndex].CertificateList.Count + 1;
                        subjects[subjectIndex].CertificateList.Add(certificateData);
                    }
                    else
                    {
                        User subject = new User();
                        subject.ID = subjects.Count + 1;
                        subject.UserName = subjectName;
                        certificateData.ID = 1;
                        subject.CertificateList.Add(certificateData);
                        subjects.Add(subject);
                    }
                }
            }
            certificatesCollection.Clear();
            return subjects;
        }
    }
}
