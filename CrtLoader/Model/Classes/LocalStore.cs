using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using CrtLoader.Model.Interfaces;

namespace CrtLoader.Model.Classes
{
    public class LocalStore : ILocalStore
    {
        public Task InsertCertificate(ICertificateData certificate)
        {
            throw new NotImplementedException();
        }

        public Task<List<CertificateData>> LoadCertificates()
        {
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            List<CertificateData> certificates = new List<CertificateData>();
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificatesCollection = store.Certificates;
            foreach (X509Certificate x509Certificate in certificatesCollection)
            {
                using (X509Certificate2 x509 = new X509Certificate2(x509Certificate.GetRawCertData()))
                {
                    CertificateSubject subject = new CertificateSubject();
                    subject.SubjectName = x509.Subject.Split(',')[0].Remove(0, 3);
                    CertificateData certificateData = new CertificateData();
                    certificateData.Subject = subject;
                    certificateData.CertificateHash = x509.GetCertHashString();
                    certificateData.Algorithm = x509.GetKeyAlgorithm();
                    certificateData.StartDate = x509.NotBefore;
                    certificateData.EndDate = x509.NotAfter;
                    certificates.Add(certificateData);
                }
            }
            certificatesCollection.Clear();
            return Task.FromResult(certificates);
        }
    }
}
