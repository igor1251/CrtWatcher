using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetworkOperators.Identity.DataTransferObjects;
using Tools.Reporters;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using X509KeysVault.Entities;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Json;

namespace X509ObserverWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HttpClient _httpClient;
        private ServiceParameters _serviceParameters;
        public Worker(ILogger<Worker> logger, 
                      HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        private async Task<string> TryToRegisterService()
        {
            var registrationRequest = new UserAuthorizationRequest()
            {
                UserName = _serviceParameters.RemoteServiceLogin,
                Password = _serviceParameters.RemoteServicePassword
            };

            var apiKey = string.Empty;

            using (var requestContent = new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json"))
            {
                using (var response = await _httpClient.PostAsync(_serviceParameters.RemoteRegistrationServiceAddress, requestContent))
                {
                    _logger.LogInformation("\nотправка [POST] request => {0}\nrequestContent = {1}\n", _serviceParameters.RemoteRegistrationServiceAddress, requestContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        apiKey = JsonSerializer.Deserialize<UserAuthorizationResponse>(responseString).Token;
                        _logger.LogInformation("\nполучен ключ : {0}\n", apiKey);
                    }
                    else
                    {
                        await ErrorReporter.MakeReport("TryToRegisterService()", new Exception("Unable to register service. " + response.ReasonPhrase));
                        await StopAsync(new CancellationToken());
                    }
                }
            }

            return apiKey;
        }

        private async Task SendX509KeyToServer(Subject subject)
        {
            using (var response = await _httpClient.PostAsJsonAsync(_serviceParameters.RemoteX509VaultStoreService, subject))
            {
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("\nрезультат запроса: успех\n");
                }
                else
                {
                    _logger.LogWarning("\nрезультат запроса : ошибка \nстатус : {0}\nсообщение : {1}\n", response.StatusCode, response.ReasonPhrase);
                }
            }
        }

        private async Task SendX509KeysVaultReportToServer(List<X509KeysVault.Entities.Subject> subjects)
        {
            if (subjects == null) return;
            if (subjects.Count == 0) return;
            
            foreach (var subject in subjects)
            {
                await SendX509KeyToServer(subject);
            }
        }

        private Task<int> FindSubject(List<Subject> subjects, string subjectName)
        {
            for (int i = 0; i < subjects.Count; i++)
            {
                if (subjects[i].Name == subjectName)
                {
                    return Task.FromResult(i);
                }
            }
            return Task.FromResult(-1);
        }

        public async Task<List<Subject>> GetSubjectsFromSystemStorageAsync()
        {
            var subjects = new List<Subject>();
            try
            {
                var store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                var certificatesCollection = store.Certificates;
                foreach (var x509Certificate in certificatesCollection)
                {
                    using (var x509 = new X509Certificate2(x509Certificate.GetRawCertData()))
                    {
                        var digitalFingerprint = new DigitalFingerprint() { Hash = x509.GetCertHashString(), Start = x509.NotBefore, End = x509.NotAfter };
                        string subjectName = "";
                        foreach (var item in x509.Subject.Split(','))
                        {
                            if (item.IndexOf("CN") > -1)
                            {
                                subjectName = item.Remove(0, 3);                                    // немного волшебства
                                if (subjectName.IndexOf('=') > -1)                                  //
                                    subjectName = subjectName.Remove(subjectName.IndexOf('='), 1);  //
                            }
                        }

                        int subjectIndex = await FindSubject(subjects, subjectName);
                        if (subjectIndex > -1)
                        {
                            subjects[subjectIndex].Fingerprints.Add(digitalFingerprint);
                        }
                        else
                        {
                            var subject = new Subject() { Name = subjectName };
                            subject.Fingerprints.Add(digitalFingerprint);
                            subjects.Add(subject);
                        }
                    }
                }
                certificatesCollection.Clear();
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetSubjectsFromSystemStorageAsync()", ex);
            }
            return subjects;
        }



        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _serviceParameters = await ServiceParametersLoader.ReadServiceParameters();
            if (string.IsNullOrEmpty(_serviceParameters.ApiKey))
            {
                _logger.LogInformation("\nApiKey пуст. Требуется регистрация сервиса. Регистрируюсь...\n");
                _serviceParameters.ApiKey = await TryToRegisterService();
                _logger.LogInformation("\nРегистрация прошла успешно. Получен ApiKey = {0}\n", _serviceParameters.ApiKey);
            }
            else
            {
                _logger.LogInformation("\nДля доступа к Api будет использоваться ApiKey = {0}\n", _serviceParameters.ApiKey);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _serviceParameters.ApiKey);
            await base.StartAsync(cancellationToken);
        }
                
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("\nцикл запущен\n");
                var localX509KeyVaultSnapshot = await GetSubjectsFromSystemStorageAsync();
                if (localX509KeyVaultSnapshot.Count > 0)
                {
                    await SendX509KeysVaultReportToServer(localX509KeyVaultSnapshot);
                }
                Thread.Sleep(_serviceParameters.MonitoringInterval);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await ServiceParametersLoader.WriteServiceParameters(_serviceParameters);
            await base.StopAsync(cancellationToken);
        }
    }
}
