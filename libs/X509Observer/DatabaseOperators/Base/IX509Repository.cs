using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.Primitives.Base;

namespace X509Observer.DatabaseOperators.Base
{
    public interface IX509Repository
    {
        Task<bool> CheckDatabaseFileExistsAsync();
        Task<bool> ValidateDatabaseAsync();
        Task CreateDatabaseAsync();
        Task<IEnumerable<X509Subject>> GetX509SubjectsAsync();
        Task<X509Subject> GetX509SubjectByIDAsync(int ID);
        Task<IEnumerable<X509CertificateInfo>> GetX509CertificateInfosAsync();
        Task AddX509SubjectAsync(X509Subject subject);
        Task AddX509SubjectAsync(IEnumerable<X509Subject> subjects);
        Task AddX509CertificateInfoAsync(X509CertificateInfo certificateInfo);
        Task UpdateX509SubjectAsync(X509Subject subject);
        Task RemoveX509CertificateInfoAsync(int ID);
        Task RemoveX509SubjectAsync(int ID);
    }
}
