using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X509Observer.Primitives.Network;

namespace X509Observer.DatabaseOperators.Network
{
    public class ClientDesktopsRepository : IClientDesktopsRepository
    {
        public Task AddClientDesktopAsync(ClientDesktop clientDesktop)
        {
            throw new NotImplementedException();
        }

        public Task<ClientDesktop> GetClientDesktopByIDAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<ClientDesktop> GetClientDesktopByIPAsync(string IP)
        {
            throw new NotImplementedException();
        }

        public Task<ClientDesktop> GetClientDesktopByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientDesktop>> GetClientDesktopsAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveClientDesktopAsync(ClientDesktop clientDesktop)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClientDesktopByIDAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClientDesktopByIPAsync(string IP)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClientDesktopByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
