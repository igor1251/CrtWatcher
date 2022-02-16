using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using AutoMapper;
using HostsRegistrationService.Services.Interfaces;
using HostsRegistrationService.Models.Classes;

namespace HostsRegistrationService.GrpcServices
{
    public class RegistrationService : ClientHostsRegistrationService.ClientHostsRegistrationServiceBase
    {
        IHostStore _store;
        MapperConfiguration configuration;
        Mapper mapper;

        public RegistrationService(IHostStore store)
        {
            _store = store;
            configuration = new MapperConfiguration(cfg => cfg.CreateMap<ClientHost, ClientHostDTO>());
            mapper = new Mapper(configuration);
        }

        public override async Task<HostClientResponse> RegisterClientHost(HostClientRequest request, ServerCallContext context)
        {
            var response = new HostClientResponse();
            var client = mapper.Map<ClientHost>(request.Client);
            await _store.AddClientHost(client);
            response.Result = ClientHostOperationResult.Accepted;
            return response;
        }

        public override async Task<HostClientResponse> UnregisterClientHost(HostClientRequest request, ServerCallContext context)
        {
            var response = new HostClientResponse();
            var client = mapper.Map<ClientHost>(request.Client);
            await _store.DeleteClientHost(client);
            response.Result = ClientHostOperationResult.Accepted;
            return response;
        }

        public override async Task<RegisteredClientHostsResponse> GetRegisteredClientHosts(Empty request, ServerCallContext context)
        {
            var response = new RegisteredClientHostsResponse();
            var registeredClients = await _store.GetClientHosts();
            foreach (var client in registeredClients)
            {
                response.Clients.Add(mapper.Map<ClientHostDTO>(client));
            }
            return response;
        }
    }
}
