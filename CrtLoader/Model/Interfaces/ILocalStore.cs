using System.Collections.Generic;
using System.Threading.Tasks;
using CrtLoader.Model.Classes;

namespace CrtLoader.Model.Interfaces
{
    public interface ILocalStore
    {
        Task<List<CertificateData>> LoadCertificates();
        Task InsertCertificate(ICertificateData certificate);
    }
}
