using System.Collections.Generic;
using System.Threading.Tasks;
using WA4D0G.Model.Classes;

namespace WA4D0G.Model.Interfaces
{
    public interface ICertificateLoader
    {
        Task<List<Certificate>> ExtractCertificatesListAsync();
        Task<List<Certificate>> ExtractUnavailableCertificatesListAsync();
    }
}
