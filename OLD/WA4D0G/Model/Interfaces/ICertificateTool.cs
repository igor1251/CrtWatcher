using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WA4D0G.Model.Classes;

namespace WA4D0G.Model.Interfaces
{
    public interface ICertificateTool
    {
        Task<bool> SaveCertificateToDatabaseAsync(ICertificate certificate);
        Task<bool> SaveCertificateToDatabaseAsync(ObservableCollection<ICertificate> certificates);
        Task UpdateCertificateInDatabaseAsync(ICertificate certificate);
        Task DeleteCertificateFromDatabaseAsync(ICertificate certificate);
    }
}
