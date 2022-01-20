using System.Collections.Generic;
using System.Threading.Tasks;
using CrtAdminPanel.Models.Classes;

namespace CrtAdminPanel.Models.Interfaces
{
    public interface ICertificateLoader
    {
        Task<List<Certificate>> ExtractCertificatesListAsync();
        Task<List<Certificate>> ExtractUnavailableCertificatesListAsync();
    }
}
