using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.Common.Entities;

namespace X509Observer.Server.Repositories
{
    public interface ISubjectsRepository
    {
        Task<List<Subject>> GetSubjectsFromSystemStorageAsync();
        Task<List<Subject>> GetSubjectsAsync();
        Task<Subject> GetSubjectByIDAsync(int ID);
        Task<List<DigitalFingerprint>> GetDigitalFingerprintsBySubjectIDAsync(int subjectID);
        Task AddSubjectAsync(Subject subject);
        Task AddSubjectAsync(List<Subject> subjects);
        Task AddDigitalFingerprintAsync(DigitalFingerprint certificateInfo, int subjectID);
        Task UpdateSubjectAsync(Subject subject);
        Task RemoveDigitalFingerptintByIDAsync(int ID);
        Task RemoveSubjectByIDAsync(int ID);
    }
}
