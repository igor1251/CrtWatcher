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

namespace Observer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ISettingsStorage _settingsStorage;
        private IUsersStorage _usersStorage;
        private ILocalUsersStorage _localUsersStorage;
        private ObserverConditionLoader _observerConditionLoader;
        private Settings settings;

        private GrpcChannel _channel;
        private ExchangeService.ExchangeServiceClient _exchangeServiceClient;

        public Worker(ILogger<Worker> logger,
                      ISettingsStorage settingsStorage,
                      IUsersStorage usersStorage,
                      ILocalUsersStorage localUsersStorage,
                      ObserverConditionLoader observerConditionLoader)
        {
            _logger = logger;
            _settingsStorage = settingsStorage;
            _usersStorage = usersStorage;
            _localUsersStorage = localUsersStorage;
            _observerConditionLoader = observerConditionLoader;
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
            host.ConnectionPort = 5001;
            host.IP = Dns.GetHostEntry(host.HostName).AddressList[0].ToString();
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
            var request = new HostRequest();
            request.Host = ConvertHostToDTO(GetHostInfo());
            await _exchangeServiceClient.RegisterHostAsync(request);
        }

        private async Task AskServerForSettings()
        {
            _logger.LogInformation("Trying to get the settings from the server....");
            try 
            {
                var response = await _exchangeServiceClient.GetSettingsAsync(new Empty());
                settings = ConvertSettingsFromDTO(response.Settings);
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
                    InitializeGrpcChannel(serverIP, serverPort);
                    await SendHostRegistrationRequestToServer();
                    await AskServerForSettings();
                    await _observerConditionLoader.SaveObserverConditionAsync(ObserverCondition.RegularLaunch);
                    break;
                case ObserverCondition.RegularLaunch:
                    _logger.LogInformation("The service is running normally. Loading the saved configuration....");
                    settings = await _settingsStorage.LoadSettingsFromFile();
                    _logger.LogInformation("Loaded configuration:\nServer IP = {0}\nServer port = {1}", settings.MainServerIP, settings.MainServerPort);
                    InitializeGrpcChannel(settings.MainServerIP, settings.MainServerPort.ToString());
                    break;
                case ObserverCondition.Error:
                    _logger.LogError("The service is started with errors. Work is impossible. See the log for more information.");
                    break;
            }
                        
            await base.StartAsync(cancellationToken);
        }

        private int IndexOfCertificate(List<Certificate> registeredCertificates, Certificate verifiedCertificate)
        {
            foreach (var certificate in registeredCertificates)
            {
                if (certificate.CertificateHash == verifiedCertificate.CertificateHash)
                {
                    return certificate.ID;
                }
            }
            return -1;
        }

        private int IndexOfUser(List<User> registeredUsers, User verifiedUser)
        {
            foreach (var user in registeredUsers)
            {
                if (user.UserName == verifiedUser.UserName)
                {
                    return user.ID;
                }
            }
            return -1;
        }

        private async Task SendUserInfoToServer(User user)
        {
            var request = new SingleUserRequest();
            request.User = ConvertUserToDTO(user);
            await _exchangeServiceClient.RegisterSingleUserAsync(request);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(settings.VerificationFrequency * 100, stoppingToken);

                _logger.LogInformation("Starting to check the certificate store....");
                var currentUsersList = await _localUsersStorage.LoadCertificateSubjectsAndCertificates();
                _logger.LogInformation("\tSystem storage: read. Count = {0}", currentUsersList.Count);
                var alreadyRegisteredUsersList = await _usersStorage.GetUsers();
                _logger.LogInformation("\tSystem storage: read. Count = {0}\n\tComparing....", alreadyRegisteredUsersList.Count);

                /*
                 * Если база данных с пользователями пуста, то мы заполняем ее 
                 * содержимым из системного хранилища и для каждого отпаравляем 
                 * запрос на сервер для регистрации
                 */ 
                if (alreadyRegisteredUsersList.Count == 0)
                {
                    _logger.LogInformation("The list of already registered users is empty. I save user information, send a save request to the server....");
                    await _usersStorage.InsertUser(currentUsersList);
                    foreach (var user in currentUsersList)
                    {
                        var request = new SingleUserRequest();
                        request.User = ConvertUserToDTO(user);
                        await _exchangeServiceClient.RegisterSingleUserAsync(request);
                    }
                }
                /*
                 * Если в базе данных по какой-то причине зарегистрировано пользоваателей 
                 * больше, чем их есть на самом деле, то она очищается и заполняется по новой
                 */ 
                else if (alreadyRegisteredUsersList.Count > currentUsersList.Count)
                {
                    _logger.LogInformation("The list of registered users is out of date. Re-filling....");
                    foreach (var user in alreadyRegisteredUsersList)
                    {
                        await _usersStorage.DeleteUser(user.ID);
                    }
                    await _usersStorage.InsertUser(currentUsersList);
                    _logger.LogInformation("The list of registered users has been updated.");
                }
                /*
                 * Если в базе данных столько же пользователей или меньше, чем зарегистрироваано 
                 * в системе, то мы просто проверяем, не поменялось ли что-то, если их столько же
                 * или регистрируем нового пользователя
                 */
                else
                {
                    foreach (var currentUser in currentUsersList)
                    {
                        int currentUserID = IndexOfUser(alreadyRegisteredUsersList, currentUser);
                        if (currentUserID < 0)
                        {
                            /*
                             * Пользователь еще не зарегистрирован
                             */
                            _logger.LogInformation("New user has been founded. Registering....");
                            await SendUserInfoToServer(currentUser);
                            await _usersStorage.InsertUser(currentUser);
                        }
                        else
                        {
                            var alreadyRegisteredUser = await _usersStorage.GetUserByID(currentUserID);
                            foreach (var certificate in currentUser.CertificateList)
                            {
                                if (IndexOfCertificate(alreadyRegisteredUser.CertificateList, certificate) < 0)
                                {
                                    /*
                                     * Пользователь уже зарегистрирован, но список его сертификатов обновился
                                     */
                                    _logger.LogInformation("New certificate has been founded. Registering....");
                                    await _usersStorage.InsertCertificate(certificate, currentUserID);
                                    await SendUserInfoToServer(currentUser);
                                }
                            }
                        }
                    }
                }

                await AskServerForSettings();
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _channel.ShutdownAsync();
            await _settingsStorage.SaveSettingsToFile(settings);
            await base.StopAsync(cancellationToken);
        }
    }
}
