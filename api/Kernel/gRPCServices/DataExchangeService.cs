using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using HostsRegistrationService.Services.Interfaces;
using ElectronicDigitalSignatire.Services.Interfaces;

namespace Kernel.gRPCServices
{
    public class DataExchangeService : ExchangeService.ExchangeServiceBase
    {
        ILogger<DataExchangeService> _logger;

        IHostStore _hostsStore;
        IUsersStore _usersStore;

        #region Data converters

        #endregion

        public DataExchangeService(ILogger<DataExchangeService> logger,
                                   IHostStore hostStore,
                                   IUsersStore usersStore)
        {
            _logger = logger;
            _hostsStore = hostStore;
            _usersStore = usersStore;
        }

        public override Task<Response> GetSettings(SettingsRequest request, ServerCallContext context)
        {
            return base.GetSettings(request, context);
        }

        public override Task<Response> RegisterHost(HostRequest request, ServerCallContext context)
        {
            return base.RegisterHost(request, context);
        }

        public override Task<Response> RegisterSingleUser(SingleUserRequest request, ServerCallContext context)
        {
            return base.RegisterSingleUser(request, context);
        }

        public override Task<Response> RegisterMultipleUsers(MultipleUsersRequest request, ServerCallContext context)
        {
            return base.RegisterMultipleUsers(request, context);
        }
    }
}
