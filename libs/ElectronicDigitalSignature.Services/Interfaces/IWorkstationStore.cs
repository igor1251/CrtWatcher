using ElectronicDigitalSignature.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicDigitalSignature.Services.Interfaces
{
    public interface IWorkstationStore
    {
        Task AddWorkstation();
        Task RemoveWorkstation();
        Task<IEnumerable<IWorkstation>> GetRegisteredWorkstations();
    }
}
