using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.Primitives.Network;

namespace X509Observer.DatabaseOperators.Network
{
    public interface IClientDesktopRepository
    {
        Task<IEnumerable<ClientDesktop>> GetClientDesktopsAsync();
        Task<ClientDesktop> GetClientDesktopByIDAsync(int ID);
        Task<ClientDesktop> GetClientDesktopByNameAsync(string name);
        Task<ClientDesktop> GetClientDesktopByIPAsync(string IP);
        Task AddClientDesktopAsync(ClientDesktop clientDesktop);
        Task RemoveClientDesktopAsync(ClientDesktop clientDesktop);
        Task RemoveClientDesktopByIDAsync(int ID);
    }
}
