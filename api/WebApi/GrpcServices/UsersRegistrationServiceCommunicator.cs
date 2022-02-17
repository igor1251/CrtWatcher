﻿using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using ElectronicDigitalSignatire.Models.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.GrpcServices
{
    public class UsersRegistrationServiceCommunicator
    {
        Channel _channel;
        CertificateUsersRegistrationService.CertificateUsersRegistrationServiceClient _client;
        ILogger<UsersRegistrationServiceCommunicator> _logger;

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

        public UsersRegistrationServiceCommunicator(ILogger<UsersRegistrationServiceCommunicator> logger)
        {
            _logger = logger;
            //_channel = GrpcChannel.ForAddress("https://localhost:5004");
            _channel = new Channel("localhost:5004", ChannelCredentials.Insecure);
            _client = new CertificateUsersRegistrationService.CertificateUsersRegistrationServiceClient(_channel);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            _logger.LogInformation("Sending GET USERS request to localhost:5004");
            var response = await _client.GetRegisteredUsersAsync(new Google.Protobuf.WellKnownTypes.Empty());
            var users = new List<User>();
            foreach (var user in response.Users)
            {
                users.Add(ConvertUserFromDTO(user));
            }
            return users;
        }

        public async Task RegisterUser(User user)
        {
            var request = new UserRequest();
            request.User = ConvertUserToDTO(user);
            var response = await _client.RegisterUserAsync(request);
        }

        public async Task UnregisterUser(User user)
        {
            var request = new UserRequest();
            request.User = ConvertUserToDTO(user);
            var response = await _client.UnregisterUserAsync(request);
        }
    }
}
