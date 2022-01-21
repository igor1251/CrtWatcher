using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WA4D0GWebPanel.Models;

namespace WA4D0GWebPanel.Services
{
    public class MockCertificateRepository : ICertificateRepository
    {
        List<Subject> _subjects;

        public MockCertificateRepository()
        {
            _subjects = new List<Subject>();
        }

        public Task DeleteSubject(int subjectID)
        {
            throw new NotImplementedException();
        }

        public Task EditSubject(Subject subject)
        {
            throw new NotImplementedException();
        }

        //песочница
        public Task<IEnumerable<Subject>> GetSubjectsList()
        {
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                Subject subject = new Subject()
                {
                    ID = i + 1,
                    FullName = "user" + i.ToString(),
                    Email = "user" + i.ToString() + "@gmail.com",
                    PhoneNumber = "8961003751"
                };

                subject.Certificates = new List<Certificate>();
                int certCount = random.Next(1, 7);
                for (int j = 1; j < certCount; j++)
                {
                    subject.Certificates.Add(new Certificate()
                    {
                        ID = j,
                        StartDate = DateTime.Now.AddDays(j),
                        EndDate = DateTime.Now.AddDays(j + 365),
                        Algorithm = "DES-256",
                        Hash = "a4f1c6d9e14b9"
                    });
                }

                _subjects.Add(subject);
            }

            return Task.FromResult<IEnumerable<Subject>>(_subjects);
        }
    }
}
