using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using System.IO;
using DataStructures;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;

namespace Observer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ISettingsStorage _settingsStorage;
        private IUsersStorage _usersStorage;
        private ILocalUsersStorage _localUsersStorage;

        private GrpcChannel _channel;
        private ExchangeService.ExchangeServiceClient _exchangeServiceClient;

        public Worker(ILogger<Worker> logger,
                      ISettingsStorage settingsStorage,
                      IUsersStorage usersStorage,
                      ILocalUsersStorage localUsersStorage)
        {
            _logger = logger;
            _settingsStorage = settingsStorage;
            _usersStorage = usersStorage;
            _localUsersStorage = localUsersStorage;
        }

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

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = GrpcChannel.ForAddress("https://localhost:5000");
            _exchangeServiceClient = new ExchangeService.ExchangeServiceClient(_channel);
            var host = new ClientHost();
            host.IP = "localhost";
            host.HostName = "test";
            host.ConnectionPort = 1;
            var request = new HostRequest();
            request.Host = ConvertHostToDTO(host);
            await _exchangeServiceClient.RegisterHostAsync(request);
            var response = await _exchangeServiceClient.GetSettingsAsync(new Empty());
            var settings = ConvertSettingsFromDTO(response.Settings);
            await _settingsStorage.SaveSettingsToFile(settings);
            await ExecuteAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var settings = await _settingsStorage.LoadSettingsFromFile();
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(settings.VerificationFrequency * 100, stoppingToken);
                var registeredUsers = await _localUsersStorage.LoadCertificateSubjectsAndCertificates();
                //await _usersStorage.InsertUser(registeredUsers);
                foreach (var user in registeredUsers)
                {
                    var request = new SingleUserRequest();
                    request.User = ConvertUserToDTO(user);
                    await _exchangeServiceClient.RegisterSingleUserAsync(request);
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _channel.ShutdownAsync();
        }
    }
}
