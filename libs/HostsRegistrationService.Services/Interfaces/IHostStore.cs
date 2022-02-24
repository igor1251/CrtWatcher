using HostsRegistrationService.Models.Classes;
using HostsRegistrationService.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HostsRegistrationService.Services.Interfaces
{
    public interface IHostStore
    {
        Task<IEnumerable<IClientHost>> GetClientHosts();
        Task AddClientHost(ClientHost clientHost);
        Task DeleteClientHost(ClientHost clientHost);
        Task UpdateClientHost(ClientHost clientHost);
    }
}
