using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStructures
{
    public interface IHostsStorage
    {
        Task<IEnumerable<IClientHost>> GetClientHosts();
        Task AddClientHost(ClientHost clientHost);
        Task DeleteClientHost(ClientHost clientHost);
        Task UpdateClientHost(ClientHost clientHost);
    }
}
