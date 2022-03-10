using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using DataStructures;

namespace Kernel.gRPCServices
{
    public class DataExchangeService : ExchangeService.ExchangeServiceBase
    {
        ILogger<DataExchangeService> _logger;

        IHostsStorage _hostsStore;
        IUsersStorage _usersStore;
        ISettingsStorage _settingsStore;

        #region Data converters

        private Certificate ConvertCertificateFromDTO(CertificateDTO dto)
        {
            var certificate = new Certificate();
            certificate.Algorithm = dto.Algorithm;
            certificate.CertificateHash = dto.CertificateHash;
            certificate.ID = dto.Id;
            certificate.StartDate = dto.StartDate.ToDateTime();
            certificate.EndDate = dto.EndDate.ToDateTime();
            return certificate;
        }

        private CertificateDTO ConvertCertificateToDTO(Certificate certificate)
        {
            var dto = new CertificateDTO();
            dto.Id = certificate.ID;
            dto.Algorithm = certificate.Algorithm;
            dto.CertificateHash = certificate.CertificateHash;
            dto.StartDate = Timestamp.FromDateTime(certificate.StartDate.ToUniversalTime());
            dto.EndDate = Timestamp.FromDateTime(certificate.EndDate.ToUniversalTime());
            return dto;
        }

        private User ConvertUserFromDTO(UserDTO dto)
        {
            var user = new User();
            user.UserName = dto.UserName;
            user.UserPhone = dto.UserPhone;
            user.UserComment = dto.UserComment;
            user.ID = dto.Id;
            foreach (var cert in dto.CertificatesList)
            {
                user.CertificateList.Add(ConvertCertificateFromDTO(cert));
            }
            return user;
        }

        private UserDTO ConvertUserToDTO(User user)
        {
            var dto = new UserDTO();
            dto.UserName = user.UserName;
            dto.UserPhone = user.UserPhone;
            dto.UserComment = user.UserComment;
            dto.Id = user.ID;
            foreach (var cert in user.CertificateList)
            {
                dto.CertificatesList.Add(ConvertCertificateToDTO(cert));
            }
            return dto;
        }

        private ClientHost ConvertHostFromDTO(HostDTO dto)
        {
            var host = new ClientHost();
            host.IP = dto.Ip;
            host.HostName = dto.HostName;
            host.ConnectionPort = dto.ConnectionPort;
            return host;
        }

        private HostDTO ConvertHostToDTO(ClientHost host)
        {
            var dto = new HostDTO();
            dto.Ip = host.IP;
            dto.HostName = host.HostName;
            dto.ConnectionPort = host.ConnectionPort;
            return dto;
        }

        private Settings ConvertSettingsFromDTO(SettingsDTO dto)
        {
            var settings = new Settings();
            settings.VerificationFrequency = dto.VerificationFrequency;
            settings.MainServerIP = dto.MainServerIP;
            settings.MainServerPort = dto.MainServerPort;
            return settings;
        }

        private SettingsDTO ConvertSettingsToDTO(Settings settings)
        {
            var dto = new SettingsDTO();
            dto.VerificationFrequency = settings.VerificationFrequency;
            dto.MainServerIP = settings.MainServerIP;
            dto.MainServerPort = settings.MainServerPort;
            return dto;
        }

        #endregion

        public DataExchangeService(ILogger<DataExchangeService> logger,
                                   IHostsStorage hostsStore,
                                   IUsersStorage usersStore,
                                   ISettingsStorage settingsStore)
        {
            _logger = logger;
            _hostsStore = hostsStore;
            _usersStore = usersStore;
            _settingsStore = settingsStore;
        }

        public override async Task<SettingsResponse> GetSettings(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to load the settings....");
            var response = new SettingsResponse();
            try
            {
                response.Settings = ConvertSettingsToDTO(await _settingsStore.LoadSettings());
                _logger.LogInformation("The settings are loaded and ready to be sent.");
            }
            catch (Exception ex)
            {
                response.Settings = ConvertSettingsToDTO(new Settings());
                _logger.LogError(ex.Message);
            }

            return response;
        }

        public override async Task<Response> RegisterHost(HostRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to register a client host....\nhostname: {0}\nip: {1}\nport: {2}", request.Host.HostName, request.Host.Ip, request.Host.ConnectionPort);
            var response = new Response();
            response.Result = ExchangeResult.None;

            try
            {
                var host = ConvertHostFromDTO(request.Host);
                await _hostsStore.AddClientHost(host);
                response.Result = ExchangeResult.Success;
                _logger.LogInformation("The host has been successfully registered.");
            }
            catch (Exception ex)
            {
                response.Result = ExchangeResult.Failed;
                response.Comment = ex.Message;
                _logger.LogError(ex.Message);
            }
            
            return response;
        }

        public override async Task<Response> RegisterSingleUser(SingleUserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Trying to register new user....\nusername: {0}\nphone: {1}\ncomment: {2}", request.User.UserName, request.User.UserPhone, request.User.UserComment);
            var response = new Response();
            response.Result = ExchangeResult.None;

            try
            {
                var user = ConvertUserFromDTO(request.User);
                await _usersStore.InsertUser(user);
                response.Result = ExchangeResult.Success;
                _logger.LogInformation("The user has been successfully registered.");
            }
            catch (Exception ex)
            {
                response.Result = ExchangeResult.Failed;
                response.Comment = ex.Message;
                _logger.LogError(ex.Message);
            }

            return response;
        }

        public override async Task<Response> RegisterMultipleUsers(MultipleUsersRequest request, ServerCallContext context)
        {
            var response = new Response();
            response.Result = ExchangeResult.None;
            response.Comment = "Not implemented now.";
            return response;
        }
    }
}
