using HostsRegistration.Models.Classes;
using HostsRegistration.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HostsRegistration.Services.Interfaces
{
    public interface IHostStore
    {
        Task<IEnumerable<IClientHost>> GetHosts();
        Task AddHost(ClientHost clientHost);
        Task DeleteHost(ClientHost clientHost);
        Task UpdateClientHost(ClientHost clientHost);
    }
}
