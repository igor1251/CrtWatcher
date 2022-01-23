using System.Collections.Generic;
using System.Threading.Tasks;
using ElectrnicDigitalSignatire.Models.Classes;
using ElectrnicDigitalSignatire.Models.Interfaces;

namespace ElectrnicDigitalSignatire.Services.Interfaces
{
    public interface IDbStore
    {
        Task<List<CertificateData>> GetCertificateData();
        Task<List<CertificateSubject>> GetCertificateSubjects();

        Task Delete(ICertificateData certificate);
        Task Insert(ICertificateData certificate);
        Task Insert(List<ICertificateData> certificate);
        Task Update(ICertificateSubject subject);
        Task Delete(ICertificateSubject subject);
        Task Insert(ICertificateSubject subject);
    }
}
