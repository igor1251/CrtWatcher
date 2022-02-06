using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using WA4D0G.Model.Interfaces;

namespace WA4D0G.Model.Classes
{
    public class SystemStoreCertificateLoader : ICertificateLoader
    {
        private ISettingsLoader _settingsExtractor;

        public SystemStoreCertificateLoader(ISettingsLoader settingsExtractor)
        {
            _settingsExtractor = settingsExtractor;
        }

        private Task<List<Certificate>> ExtractCertificatesListFromSystemStore(bool onlyUnavailable, uint warnDaysCount)
        {
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            List<Certificate> certificates = new List<Certificate>();
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificatesCollection = store.Certificates;
            foreach (X509Certificate x509Certificate in certificatesCollection)
            {
                using (X509Certificate2 x509 = new X509Certificate2(x509Certificate.GetRawCertData()))
                {
                    if (onlyUnavailable && (x509.NotAfter - DateTime.Now).Days > warnDaysCount)
                        continue;
                    Certificate certificate = new Certificate();
                    certificate.HolderFIO = x509.Subject.Split(',')[0].Remove(0, 3);
                    certificate.CertStartDateTime = x509.NotBefore;
                    certificate.CertEndDateTime = x509.NotAfter;
                    certificates.Add(certificate);
                }
            }
            certificatesCollection.Clear();
            return Task.FromResult(certificates);
        }

        public async Task<List<Certificate>> ExtractCertificatesListAsync()
        {
            ISettings settings = await _settingsExtractor.LoadSettingsAsync();
            return await ExtractCertificatesListFromSystemStore(false, settings.WarnDaysCount);
        }

        public async Task<List<Certificate>> ExtractUnavailableCertificatesListAsync()
        {
            ISettings settings = await _settingsExtractor.LoadSettingsAsync();
            return await ExtractCertificatesListFromSystemStore(true, settings.WarnDaysCount);
        }
    }
}
