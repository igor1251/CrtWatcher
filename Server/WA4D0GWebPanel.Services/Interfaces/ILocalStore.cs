using System.Collections.Generic;
using System.Threading.Tasks;
using WA4D0GWebPanel.Models.Classes;
using WA4D0GWebPanel.Models.Interfaces;

namespace WA4D0GWebPanel.Services.Interfaces
{
    public interface ILocalStore
    {
        Task<List<CertificateSubject>> LoadCertificateSubjectsAndCertificates();
        Task InsertCertificate(ICertificateData certificate);
    }
}
