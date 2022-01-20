using System;
using System.Collections.Generic;
using WA4D0GWebPanel.Models;

namespace WA4D0GWebPanel.Services
{
    public class MockCertificateRepository : ICertificateRepository
    {
        List<Certificate> _certificates;

        public MockCertificateRepository()
        {
            _certificates = new List<Certificate>();
        }

        public IEnumerable<Certificate> GetCertificatesList()
        {
            for (int i = 0; i < 10; i++)
            {
                Certificate cert = new Certificate();
                cert.ID = i;
                cert.Subject = new Subject();
                cert.Subject.FullName = "user" + i.ToString();
                cert.Subject.Email = cert.Subject.FullName + "@gmail.com";
                cert.Subject.PhoneNumber = "89610037151";
                cert.StartDate = DateTime.Now;
                cert.EndDate = cert.StartDate.AddDays(365);
                cert.Algorithm = "test_algorithm";
                cert.Hash = "4d4g4a44e4rrg4t4bt4btnbn44mny5646";
                _certificates.Add(cert);
            }

            return _certificates;
        }
    }
}
