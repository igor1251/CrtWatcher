using HostsRegistrationService.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HostsRegistrationService.Services.Interfaces
{
    public interface IHostStore
    {
        Task<IEnumerable<IClientHost>> GetClientHosts();
        Task AddClientHost(IClientHost clientHost);
        Task DeleteClientHost(IClientHost clientHost);
    }
}
