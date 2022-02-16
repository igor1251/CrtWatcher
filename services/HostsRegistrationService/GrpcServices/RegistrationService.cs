using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using HostsRegistrationService.Services.Interfaces;

namespace HostsRegistrationService.GrpcServices
{
    public class RegistrationService : ClientHostsRegistrationService.ClientHostsRegistrationServiceBase
    {
        IHostStore _store;

        public RegistrationService(IHostStore store)
        {
            _store = store;
        }

        public override async Task<ClientHostRegistrationResponse> ProceedRequest(ClientHostRegistrationRequest request, ServerCallContext context)
        {
            var response = new ClientHostRegistrationResponse();
            if (request.ClientHostOperation == ClientHostOperation.Add)
            {

            }
            else if (request.ClientHostOperation == ClientHostOperation.Delete)
            {

            }
            return response;
        }
    }
}
