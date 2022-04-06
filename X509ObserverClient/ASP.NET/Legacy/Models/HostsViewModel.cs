using DataStructures;
using System.Collections.Generic;

namespace Site.Models
{
    public class HostsViewModel
    {
        public List<ClientHost> AvailableHosts { get; set; }

        public HostsViewModel(List<ClientHost> hosts)
        {
            AvailableHosts = hosts;
        }
    }
}
