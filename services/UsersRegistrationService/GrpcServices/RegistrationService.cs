using System.Threading.Tasks;
using ElectronicDigitalSignatire.Services.Interfaces;
using Grpc.Core;
using ElectronicDigitalSignatire.Models.Classes;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;

namespace UsersRegistrationService.GrpcServices
{
    public class RegistrationService : CertificateUsersRegistrationService.CertificateUsersRegistrationServiceBase
    {
        IDbStore _store;
        ILogger<RegistrationService> _logger;

        #region -= Converters =-

        private CertificateDTO ConvertCertificateToDTO(Certificate certificate)
        {
            var certificateDTO = new CertificateDTO();
            certificateDTO.Algorithm = certificate.Algorithm;
            certificateDTO.CertificateHash = certificate.CertificateHash;
            certificateDTO.Id = certificate.ID;
            certificateDTO.StartDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(certificate.StartDate);
            certificateDTO.EndDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(certificate.EndDate);
            return certificateDTO;
        }

        private UserDTO ConvertUserToDTO(User user)
        {
            var userDTO = new UserDTO();
            userDTO.UserName = user.UserName;
            userDTO.Id = user.ID;
            userDTO.UserPhone = user.UserPhone;
            userDTO.UserComment = user.UserComment;
            foreach (var item in user.CertificateList)
            {
                userDTO.CertificatesList.Add(ConvertCertificateToDTO(item));
            }
            return userDTO;
        }

        private Certificate ConvertCertificateFromDTO(CertificateDTO certificate)
        {
            var convertedCertificate = new Certificate();
            convertedCertificate.StartDate = certificate.StartDate.ToDateTime();
            convertedCertificate.EndDate = certificate.EndDate.ToDateTime();
            convertedCertificate.CertificateHash = certificate.CertificateHash;
            convertedCertificate.Algorithm = certificate.Algorithm;
            convertedCertificate.ID = certificate.Id;
            return convertedCertificate;
        }

        private User ConvertUserFromDTO(UserDTO user)
        {
            var convertedUser = new User();
            convertedUser.UserName = user.UserName;
            convertedUser.UserComment = user.UserComment;
            convertedUser.UserPhone = user.UserPhone;
            convertedUser.ID = user.Id;
            foreach (var item in user.CertificatesList)
            {
                convertedUser.CertificateList.Add(ConvertCertificateFromDTO(item));
            }
            return convertedUser;

        }

        #endregion

        public RegistrationService(IDbStore store,
                                   ILogger<RegistrationService> logger)
        {
            _store = store;
            _logger = logger;
        }

        public override async Task<UserResponse> RegisterUser(UserRequest request, ServerCallContext context)
        {
            var response = new UserResponse();
            var user = ConvertUserFromDTO(request.User);
            await _store.InsertUser(user);
            response.Result = UserOperationResult.Accepted;
            return response;
        }

        public override async Task<UserResponse> UnregisterUser(UserRequest request, ServerCallContext context)
        {
            var response = new UserResponse();
            var user = ConvertUserFromDTO(request.User);
            await _store.DeleteUser(user.ID);
            response.Result = UserOperationResult.Accepted;
            return response;
        }

        public override async Task<UserResponse> UpdateUser(UserRequest request, ServerCallContext context)
        {
            var response = new UserResponse();
            var user = ConvertUserFromDTO(request.User);
            await _store.UpdateUser(user);
            response.Result = UserOperationResult.Accepted;
            return response;
        }

        public override async Task<RegisteredUsersResponse> GetRegisteredUsers(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Time to send users liiiiiist!");
            var response = new RegisteredUsersResponse();
            var registeredUsers = await _store.GetUsers();
            foreach (var user in registeredUsers)
            {
                response.Users.Add(ConvertUserToDTO(user));
            }
            return response;
        }
    }
}
