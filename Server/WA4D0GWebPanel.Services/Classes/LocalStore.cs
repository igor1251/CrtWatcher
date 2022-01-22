using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using WA4D0GWebPanel.Services.Interfaces;
using WA4D0GWebPanel.Models.Interfaces;
using WA4D0GWebPanel.Models.Classes;

namespace WA4D0GWebPanel.Services.Classes
{
    public class LocalStore : ILocalStore
    {
        public Task InsertCertificate(ICertificateData certificate)
        {
            throw new NotImplementedException();
        }

        private Task<int> FindSubject(List<CertificateSubject> subjects, string subjectName)
        {
            for (int i = 0; i < subjects.Count; i++)
            {
                if (subjects[i].SubjectName == subjectName)
                {
                    return Task.FromResult(i);
                }
            }
            return Task.FromResult(-1);
        }

        public async Task<List<CertificateSubject>> LoadCertificateSubjectsAndCertificates()
        {
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            List<CertificateSubject> subjects = new List<CertificateSubject>();
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificatesCollection = store.Certificates;
            foreach (X509Certificate x509Certificate in certificatesCollection)
            {
                using (X509Certificate2 x509 = new X509Certificate2(x509Certificate.GetRawCertData()))
                {
                    CertificateData certificateData = new CertificateData();
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
                        CertificateSubject subject = new CertificateSubject();
                        subject.ID = subjects.Count + 1;
                        subject.SubjectName = subjectName;
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
