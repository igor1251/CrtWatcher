using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using DataStructures;
using System.Net;
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

        private async Task SendHostRegistrationRequestToServer()
        {
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
                    await SendHostRegistrationRequestToServer();
                    await AskServerForSettings();
                    await _observerConditionLoader.SaveObserverConditionAsync(ObserverCondition.RegularLaunch);
                    break;
                case ObserverCondition.RegularLaunch:
                    _logger.LogInformation("The service is running normally. Loading the saved configuration....");
                    settings = await _settingsStorage.LoadSettings();
                    _logger.LogInformation("Loaded configuration:\nServer IP = {0}\nServer port = {1}", settings.MainServerIP, settings.MainServerPort);
                    break;
                case ObserverCondition.Error:
                    _logger.LogError("The service is started with errors. Work is impossible. See the log for more information.");
                    break;
            }
                        
            await base.StartAsync(cancellationToken);
        }

        private async Task SendUserInfoToServer(User user)
        {
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
            await _settingsStorage.UpdateSettings(settings);
            await base.StopAsync(cancellationToken);
        }
    }
}
