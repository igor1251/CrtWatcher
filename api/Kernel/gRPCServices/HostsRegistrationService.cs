using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HostsRegistration.Models.Classes;
using HostsRegistration.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Kernel.gRPCServices
{
    public class HostsRegistrationService : HostsService.HostsServiceBase
    {
        ILogger<HostsRegistrationService> _logger;
        IHostStore _store;

        #region -= Converters =-

        private HostDTO ConvertHostToDTO(ClientHost host)
        {
            var hostDTO = new HostDTO();
            hostDTO.IP = host.IP;
            hostDTO.HostName = host.HostName;
            hostDTO.ConnectionPort = host.ConnectionPort;
            return hostDTO;
        }

        private ClientHost ConvertHostFromDTO(HostDTO hostDTO)
        {
            var host = new ClientHost();
            host.IP = hostDTO.IP;
            host.HostName = hostDTO.HostName;
            host.ConnectionPort = hostDTO.ConnectionPort;
            return host;
        }

        #endregion

        public HostsRegistrationService(ILogger<HostsRegistrationService> logger,
                                        IHostStore store)
        {
            _store = store;
            _logger = logger;
        }

        public override async Task<HostResponse> RegisterHost(HostRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to register a client host....\nhostname: {0}\nip: {1}\nport: {2}", request.Client.HostName, request.Client.IP, request.Client.ConnectionPort);
            var response = new HostResponse();
            try
            {
                var client = ConvertHostFromDTO(request.Client);
                await _store.AddHost(client);
                response.Result = HostOperationResult.Accepted;
                _logger.LogInformation("The host has been successfully registered.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.Result = HostOperationResult.Declined;
            }
            return response;
        }

        public override async Task<HostResponse> UnregisterHost(HostRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to unregister a client host....\nhostname: {0}\nip: {1}\nport: {2}", request.Client.HostName, request.Client.IP, request.Client.ConnectionPort);
            var response = new HostResponse();
            try
            {
                var client = ConvertHostFromDTO(request.Client);
                await _store.DeleteHost(client);
                response.Result = HostOperationResult.Accepted;
                _logger.LogInformation("The host has been successfully unregistered.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.Result = HostOperationResult.Declined;
            }
            return response;
        }

        public override async Task<RegisteredHostsResponse> GetRegisteredHosts(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to download a list of registered hosts....");
            var response = new RegisteredHostsResponse();
            try
            {
                var registeredClients = await _store.GetHosts();
                foreach (var client in registeredClients)
                {
                    response.Clients.Add(ConvertHostToDTO((ClientHost)client));
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
