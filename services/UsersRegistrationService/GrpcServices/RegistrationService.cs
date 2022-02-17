using System.Threading.Tasks;
using ElectronicDigitalSignatire.Services.Interfaces;
using Grpc.Core;
using AutoMapper;
using ElectronicDigitalSignatire.Models.Classes;
using Google.Protobuf.WellKnownTypes;

namespace UsersRegistrationService.GrpcServices
{
    public class RegistrationService : CertificateUsersRegistrationService.CertificateUsersRegistrationServiceBase
    {
        IDbStore _store;
        MapperConfiguration mapperConfiguration;
        Mapper mapper;

        public RegistrationService(IDbStore store)
        {
            _store = store;
            mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
            mapper = new Mapper(mapperConfiguration);
        }

        public override async Task<UserResponse> RegisterUser(UserRequest request, ServerCallContext context)
        {
            var response = new UserResponse();
            var user = mapper.Map<User>(request.User);
            await _store.InsertUser(user);
            response.Result = UserOperationResult.Accepted;
            return response;
        }

        public override async Task<UserResponse> UnregisterUser(UserRequest request, ServerCallContext context)
        {
            var response = new UserResponse();
            var user = mapper.Map<User>(request.User);
            await _store.DeleteUser(user.ID);
            response.Result = UserOperationResult.Accepted;
            return response;
        }

        public override async Task<UserResponse> UpdateUser(UserRequest request, ServerCallContext context)
        {
            var response = new UserResponse();
            var user = mapper.Map<User>(request.User);
            await _store.UpdateUser(user);
            response.Result = UserOperationResult.Accepted;
            return response;
        }

        public override async Task<RegisteredUsersResponse> GetRegisteredUsers(Empty request, ServerCallContext context)
        {
            var response = new RegisteredUsersResponse();
            var registeredUsers = await _store.GetUsers();
            foreach (var user in registeredUsers)
            {
                response.Users.Add(mapper.Map<UserDTO>(user));
            }
            return response;
        }
    }
}
