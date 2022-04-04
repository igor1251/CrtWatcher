using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X509Observer.Primitives.Base;

namespace X509Observer.DatabaseOperators.Base
{
    public class SubjectsRepository : ISubjectsRepository
    {
        public Task AddDigitalFingerprintAsync(DigitalFingerprint certificateInfo)
        {
            throw new NotImplementedException();
        }

        public Task AddSubjectAsync(Subject subject)
        {
            throw new NotImplementedException();
        }

        public Task AddSubjectAsync(IEnumerable<Subject> subjects)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DigitalFingerprint>> GetDigitalFingerprintsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Subject> GetSubjectByIDAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Subject>> GetSubjectsAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveDigitalFingerptintByIDAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public Task RemoveSubjectByIDAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSubjectAsync(Subject subject)
        {
            throw new NotImplementedException();
        }
    }
}
