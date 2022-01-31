using System.Collections.Generic;
using System.Threading.Tasks;
using ElectrnicDigitalSignatire.Models.Classes;
using ElectrnicDigitalSignatire.Models.Interfaces;

namespace ElectrnicDigitalSignatire.Services.Interfaces
{
    public interface ILocalStore
    {
        Task<List<CertificateSubject>> LoadCertificateSubjectsAndCertificates();
        Task InsertCertificate(ICertificateData certificate);
    }
}
