using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.Primitives.Base;

namespace X509Observer.DatabaseOperators.Base
{
    public interface ISubjectsRepository
    {
        Task<IEnumerable<Subject>> GetSubjectsAsync();
        Task<Subject> GetSubjectByIDAsync(int ID);
        Task<IEnumerable<DigitalFingerprint>> GetDigitalFingerprintsAsync();
        Task AddSubjectAsync(Subject subject);
        Task AddSubjectAsync(IEnumerable<Subject> subjects);
        Task AddDigitalFingerprintAsync(DigitalFingerprint certificateInfo);
        Task UpdateSubjectAsync(Subject subject);
        Task RemoveDigitalFingerptintByIDAsync(int ID);
        Task RemoveSubjectByIDAsync(int ID);
    }
}
