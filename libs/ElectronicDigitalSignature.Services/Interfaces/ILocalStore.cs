using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicDigitalSignatire.Models.Classes;
using ElectronicDigitalSignatire.Models.Interfaces;

namespace ElectronicDigitalSignatire.Services.Interfaces
{
    public interface ILocalStore
    {
        Task<List<CertificateSubject>> LoadCertificateSubjectsAndCertificates();
        Task InsertCertificate(ICertificateData certificate);
    }
}
