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
using System.Net;
using Google.Protobuf.WellKnownTypes;
using System.Net.Http;
using System.Text.Json;
using Site;
using System.Text;

namespace Observer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ISettingsStorage _settingsStorage;
        private IUsersStorage _usersStorage;
        private ILocalUsersStorage _localUsersStorage;
        private HttpClient _httpClient;
        private ObserverConditionLoader _observerConditionLoader;
        private Settings settings;

        private GrpcChannel _channel;
        private ExchangeService.ExchangeServiceClient _exchangeServiceClient;

        public Worker(ILogger<Worker> logger,
                      ISettingsStorage settingsStorage,
                      IUsersStorage usersStorage,
                      ILocalUsersStorage localUsersStorage,
                      ObserverConditionLoader observerConditionLoader,
                      IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _settingsStorage = settingsStorage;
            _usersStorage = usersStorage;
            _localUsersStorage = localUsersStorage;
            _observerConditionLoader = observerConditionLoader;
            _httpClient = httpClientFactory.CreateClient("ApiHttpClient");
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

        private ClientHost GetHostInfo()
        {
            var host = new ClientHost();
            host.HostName = Dns.GetHostName();
            host.ConnectionPort = 322;

            foreach (IPAddress ip in Dns.GetHostAddresses(host.HostName))
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    host.IP = ip.ToString();
                    break;
                }
            }

            return host;
        }

        private void InitializeGrpcChannel(string serverIP, string serverPort)
        {
            _logger.LogInformation("Trying to open a gRPC connection to the server....\n\tServer IP = {0}\n\tServer port = {1}", serverIP, serverPort);
            try
            {
                _channel = GrpcChannel.ForAddress("https://" + serverIP + ":" + serverPort);
                _exchangeServiceClient = new ExchangeService.ExchangeServiceClient(_channel);
                _logger.LogInformation("The connection has been successfully established.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task SendHostRegistrationRequestToServer()
        {
            //try
            //{
            //    _logger.LogInformation("Trying to register this host on server....");
            //    var request = new HostRequest();
            //    request.Host = ConvertHostToDTO(GetHostInfo());
            //    await _exchangeServiceClient.RegisterHostAsync(request);
            //    _logger.LogInformation("Request has been successfully delivered.");
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message);
            //}
            var hostInfo = JsonSerializer.Serialize<ClientHost>(GetHostInfo());
            using (var requestContent = new StringContent(hostInfo, Encoding.UTF8, "application/json"))
            {
                using (var response = await _httpClient.PostAsync(RequestLinks.HostResponseLink, requestContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                    }
                }
            }
        }

        private async Task<T> LoadInfo<T>(string request) where T : new()
        {
            var result = new T();

            using (var response = await _httpClient.GetAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                    return result;
                }

                try
                {
                    var contentJsonString = await response.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize<T>(contentJsonString);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return result;
        }

        private async Task AskServerForSettings()
        {
            //_logger.LogInformation("Trying to get the settings from the server....");
            //try 
            //{
            //    var response = await _exchangeServiceClient.GetSettingsAsync(new Empty());
            //    settings = ConvertSettingsFromDTO(response.Settings);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message);
            //    settings = new Settings();
            //}
            _logger.LogInformation("Trying to get the settings from the server....");
            try
            {
                settings = await LoadInfo<Settings>(RequestLinks.GetSettings);
                _logger.LogInformation("Settings has been succesfully loaded from server.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                settings = new Settings();
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var observerCondition = await _observerConditionLoader.LoadObserverConditionAsync();
            switch (observerCondition)
            {
                case ObserverCondition.FirstLaunch:
                    Console.Write("The observer will be launched for the first time. Enter the address of the control server > ");
                    var serverIP = Console.ReadLine();
                    Console.Write("Enter the port number > ");
                    var serverPort = Console.ReadLine();
                    //InitializeGrpcChannel(serverIP, serverPort);
                    await SendHostRegistrationRequestToServer();
                    await AskServerForSettings();
                    await _observerConditionLoader.SaveObserverConditionAsync(ObserverCondition.RegularLaunch);
                    break;
                case ObserverCondition.RegularLaunch:
                    _logger.LogInformation("The service is running normally. Loading the saved configuration....");
                    settings = await _settingsStorage.LoadSettingsFromFile();
                    _logger.LogInformation("Loaded configuration:\nServer IP = {0}\nServer port = {1}", settings.MainServerIP, settings.MainServerPort);
                    //InitializeGrpcChannel(settings.MainServerIP, settings.MainServerPort.ToString());
                    break;
                case ObserverCondition.Error:
                    _logger.LogError("The service is started with errors. Work is impossible. See the log for more information.");
                    break;
            }
                        
            await base.StartAsync(cancellationToken);
        }

        private async Task SendUserInfoToServer(User user)
        {
            //try
            //{
            //    _logger.LogInformation("Sending information about registered users to the server....");
            //    var request = new SingleUserRequest();
            //    request.User = ConvertUserToDTO(user);
            //    await _exchangeServiceClient.RegisterSingleUserAsync(request);
            //    _logger.LogInformation("Infoormation has been successfully delivered.");
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message);
            //}
            var users = JsonSerializer.Serialize<User>(user);
            using (var requestContent = new StringContent(users, Encoding.UTF8, "application/json"))
            {
                using (var response = await _httpClient.PostAsync(RequestLinks.UsersResponseLink, requestContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                    }
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(settings.VerificationFrequency * 1000, stoppingToken);

                _logger.LogInformation("Sending information about registered users to the server....");
                var registeredUsers = await _localUsersStorage.LoadCertificateSubjectsAndCertificates();
                foreach (var registeredUser in registeredUsers)
                {
                    await SendUserInfoToServer(registeredUser);
                }
                _logger.LogInformation("Information has been successfully delivered.");
                await AskServerForSettings();
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            //await _channel.ShutdownAsync();
            await _settingsStorage.SaveSettingsToFile(settings);
            await base.StopAsync(cancellationToken);
        }
    }
}
