using System.Collections.Generic;
using System.Threading.Tasks;
using ElectrnicDigitalSignatire.Models.Classes;

namespace ElectrnicDigitalSignatire.Services.Interfaces
{
    public interface IDbStore
    {
        Task<List<CertificateSubject>> GetSubjects();
        Task<CertificateSubject> GetSubjectByID(int id);
        Task<List<CertificateData>> GetCertificates(int subjectID);
        Task DeleteCertificate(int certificateID);
        Task DeleteSubject(int subjectID);
        Task InsertSubject(CertificateSubject subject);
        Task InsertSubject(List<CertificateSubject> subjects);
        Task InsertCertificate(CertificateData certificate, int subjectID);
        Task UpdateSubject(CertificateSubject subject);
    }
}
