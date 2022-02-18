using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using HostsRegistrationService.Models.Classes;
using HostsRegistrationService.Models.Interfaces;

namespace WebApi.GrpcServices
{
    public class HostsRegistrationServiceCommunicator
    {
        Channel _channel;
        ClientHostsRegistrationService.ClientHostsRegistrationServiceClient _client;
        ILogger<HostsRegistrationServiceCommunicator> _logger;

        #region -= Converters =-

        private ClientHostDTO ConvertClientHostToDTO(ClientHost host)
        {
            var clientHostDTO = new ClientHostDTO();
            clientHostDTO.IP = host.IP;
            clientHostDTO.HostName = host.HostName;
            clientHostDTO.ConnectionPort = host.ConnectionPort;
            return clientHostDTO;
        }

        private ClientHost ConvertClientHostFromDTO(ClientHostDTO host)
        {
            var clientHost = new ClientHost();
            clientHost.IP = host.IP;
            clientHost.HostName = host.HostName;
            clientHost.ConnectionPort = host.ConnectionPort;
            return clientHost;
        }

        #endregion

        public HostsRegistrationServiceCommunicator(ILogger<HostsRegistrationServiceCommunicator> logger)
        {
            _logger = logger;
            _channel = new Channel("localhost:5005", ChannelCredentials.Insecure);
            _client = new ClientHostsRegistrationService.ClientHostsRegistrationServiceClient(_channel);
        }

        public async Task<IEnumerable<IClientHost>> GetRegisteredClientHosts()
        {
            var response = await _client.GetRegisteredClientHostsAsync(new Google.Protobuf.WellKnownTypes.Empty());
            var clientHosts = new List<ClientHost>();
            foreach (var host in response.Clients)
            {
                clientHosts.Add(ConvertClientHostFromDTO(host));
            }
            return clientHosts;
        }


    }
}
