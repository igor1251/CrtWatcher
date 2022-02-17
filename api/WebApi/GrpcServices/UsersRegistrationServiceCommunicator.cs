using Grpc.Core;
using AutoMapper;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using ElectronicDigitalSignatire.Models.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.GrpcServices
{
    public class UsersRegistrationServiceCommunicator
    {
        GrpcChannel _channel;
        CertificateUsersRegistrationService.CertificateUsersRegistrationServiceClient _client;
        ILogger<UsersRegistrationServiceCommunicator> _logger;
        Mapper _mapper;
        MapperConfiguration _mapperConfiguration;

        public UsersRegistrationServiceCommunicator(ILogger<UsersRegistrationServiceCommunicator> logger)
        {
            _logger = logger;
            _channel = GrpcChannel.ForAddress("https://localhost:5049");
            _client = new CertificateUsersRegistrationService.CertificateUsersRegistrationServiceClient(_channel);
            _mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
            _mapper = new Mapper(_mapperConfiguration);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            _logger.LogInformation("Sending GET USERS request to localhost:5049");
            var response = await _client.GetRegisteredUsersAsync(new Google.Protobuf.WellKnownTypes.Empty());
            var users = _mapper.Map<List<User>>(response.Users);
            return users;
        }

        public async Task RegisterUser(User user)
        {
            var request = new UserRequest();
            request.User = _mapper.Map<UserDTO>(user);
            var response = await _client.RegisterUserAsync(request);
        }

        public async Task UnregisterUser(User user)
        {
            var request = new UserRequest();
            request.User = _mapper.Map<UserDTO>(user);
            var response = await _client.UnregisterUserAsync(request);
        }
    }
}
