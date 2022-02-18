using System.Threading.Tasks;
using ElectronicDigitalSignatire.Services.Interfaces;
using Grpc.Core;
using ElectronicDigitalSignatire.Models.Classes;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;

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
            certificateDTO.StartDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(certificate.StartDate.ToUniversalTime());
            certificateDTO.EndDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(certificate.EndDate.ToUniversalTime());
            return certificateDTO;
        }

        private UserDTO ConvertUserToDTO(User user)
        {
            var userDTO = new UserDTO();
            userDTO.UserName = user.UserName;
            userDTO.Id = user.ID;
            userDTO.UserPhone = user.UserPhone == null ? "" : user.UserPhone;
            userDTO.UserComment = user.UserComment == null ? "" : user.UserComment;
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
            convertedUser.UserComment = user.UserComment == null ? "" : user.UserComment;
            convertedUser.UserPhone = user.UserPhone == null ? "" : user.UserPhone;
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
            _logger.LogInformation("Trying to register new user....\nusername: {0}\nphone: {1}\ncomment: {2}", request.User.UserName, request.User.UserPhone, request.User.UserComment);
            var response = new UserResponse();
            try
            {
                var user = ConvertUserFromDTO(request.User);
                await _store.InsertUser(user);
                response.Result = UserOperationResult.Accepted;
                _logger.LogInformation("The user has been successfully registered.");
            }
            catch (Exception ex)
            {
                response.Result = UserOperationResult.Declined;
                _logger.LogError(ex.Message);
            }
            return response;
        }

        public override async Task<UserResponse> UnregisterUser(UserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to unregister user....\nusername: {0}\nphone: {1}\ncomment: {2}", request.User.UserName, request.User.UserPhone, request.User.UserComment);
            var response = new UserResponse();
            try
            {
                var user = ConvertUserFromDTO(request.User);
                await _store.DeleteUser(user.ID);
                response.Result = UserOperationResult.Accepted;
                _logger.LogInformation("The user has been successfully unregistered.");
            }
            catch (Exception ex)
            {
                response.Result = UserOperationResult.Declined;
                _logger.LogError(ex.Message);
            }
            return response;
        }

        public override async Task<UserResponse> UpdateUser(UserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to update user....\nusername: {0}\nphone: {1}\ncomment: {2}", request.User.UserName, request.User.UserPhone, request.User.UserComment);
            var response = new UserResponse();
            try
            {
                var user = ConvertUserFromDTO(request.User);
                await _store.UpdateUser(user);
                response.Result = UserOperationResult.Accepted;
                _logger.LogInformation("The user has been successfully updated.");
            }
            catch (Exception ex)
            {
                response.Result = UserOperationResult.Declined;
                _logger.LogError(ex.Message);
            }
            return response;
        }

        public override async Task<RegisteredUsersResponse> GetRegisteredUsers(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to get a list of registered users....");
            var response = new RegisteredUsersResponse();
            try
            {
                var registeredUsers = await _store.GetUsers();
                foreach (var user in registeredUsers)
                {
                    response.Users.Add(ConvertUserToDTO(user));
                }
                _logger.LogInformation("The list of users has been uploaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return response;
        }

        public override async Task<UserByIDResponse> GetRegisteredUserByID(UserByIDRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to get a user under ID = {0}....", request.Id);
            var response = new UserByIDResponse();
            try
            {
                var user = await _store.GetUserByID(request.Id);
                if (user == null) response.User = null;
                else response.User = ConvertUserToDTO(user);
                _logger.LogInformation("User found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return response;
        }
    }
}
