using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HostsRegistrationService.Services.Interfaces;
using HostsRegistrationService.Models.Classes;
using Microsoft.Extensions.Logging;

namespace HostsRegistrationService.GrpcServices
{
    public class RegistrationService : ClientHostsRegistrationService.ClientHostsRegistrationServiceBase
    {
        ILogger<RegistrationService> _logger;
        IHostStore _store;

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

        public RegistrationService(ILogger<RegistrationService> logger,
                                   IHostStore store)
        {
            _store = store;
            _logger = logger;
        }

        public override async Task<HostClientResponse> RegisterClientHost(HostClientRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to register a client host....\nhostname: {0}\nip: {1}\nport: {2}", request.Client.HostName, request.Client.IP, request.Client.ConnectionPort);
            var response = new HostClientResponse();
            try
            {
                var client = ConvertClientHostFromDTO(request.Client);
                await _store.AddClientHost(client);
                response.Result = ClientHostOperationResult.Accepted;
                _logger.LogInformation("The host has been successfully registered.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.Result = ClientHostOperationResult.Declined;
            }
            return response;
        }

        public override async Task<HostClientResponse> UnregisterClientHost(HostClientRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to unregister a client host....\nhostname: {0}\nip: {1}\nport: {2}", request.Client.HostName, request.Client.IP, request.Client.ConnectionPort);
            var response = new HostClientResponse();
            try
            {
                var client = ConvertClientHostFromDTO(request.Client);
                await _store.DeleteClientHost(client);
                response.Result = ClientHostOperationResult.Accepted;
                _logger.LogInformation("The host has been successfully unregistered.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.Result = ClientHostOperationResult.Declined;
            }
            return response;
        }

        public override async Task<RegisteredClientHostsResponse> GetRegisteredClientHosts(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to download a list of registered hosts....");
            var response = new RegisteredClientHostsResponse();
            try
            {
                var registeredClients = await _store.GetClientHosts();
                foreach (var client in registeredClients)
                {
                    response.Clients.Add(ConvertClientHostToDTO((ClientHost)client));
                }
                _logger.LogInformation("The list of registered hosts has been loaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return response;
        }
    }
}
