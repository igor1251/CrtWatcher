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
            var registrationRequestDTO = new UserAuthorizationRequest()
            {
                UserName = _serviceParameters.RemoteServiceLogin,
                Password = _serviceParameters.RemoteServicePassword
            };

            var apiKey = string.Empty;

            _logger.LogInformation("\nĞÅÃÈÑÒĞ. [POST] => {0}\n", _serviceParameters.RemoteRegistrationServiceAddress);
            using (var response = await _httpClient.PostAsJsonAsync(_serviceParameters.RemoteRegistrationServiceAddress, registrationRequestDTO))
            {
                if (response.IsSuccessStatusCode)
                {
                    apiKey = JsonSerializer.Deserialize<UserAuthorizationResponse>(await response.Content.ReadAsStringAsync()).Token;
                    _logger.LogInformation("\n[POST] ÓÑÏÅÕ\nÏÎË. ÊËŞ×\n[{0}]", apiKey);
                }
                else
                {
                    _logger.LogError("\n[POST] ÎØÈÁ.\nÊÎÄ: {0}\nÑÁÙ.: {1}", response.StatusCode, response.ReasonPhrase);
                    await ErrorReporter.MakeReport("TryToRegisterService()", new Exception("Unable to register service. " + response.ReasonPhrase));
                    await StopAsync(new CancellationToken(true));
                }
            }

            return apiKey;
        }

        private void ApplyNewApiKey(string apiKey)
        {
            _serviceParameters.ApiKey = apiKey;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _serviceParameters.ApiKey);
        }

        private async Task SendX509KeyToServer(Subject subject)
        {
            _logger.LogInformation("\nÎÒÏĞ. [POST] => {0}\n", _serviceParameters.RemoteX509VaultStoreService);
            using (var response = await _httpClient.PostAsJsonAsync(_serviceParameters.RemoteX509VaultStoreService, subject))
            {
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("\n[POST] ÓÑÏÅÕ.\n");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogInformation("\nÊËŞ× ÍÅ ÂÀË. ĞÅÃÈÑÒĞ.\n");
                    ApplyNewApiKey(await TryToRegisterService());
                    if (_serviceParameters.ApiKey != string.Empty)
                    {
                        _logger.LogInformation("\nÓÑÏÅÕ. ÍÎÂ. ÇÍ. APIKEY\n[{0}]\n", _serviceParameters.ApiKey);
                    }
                    else
                    {
                        _logger.LogError("\nÎØÈÁ. ÊËŞ× ÍÅ ÏÎËÓ×. ÇÀÂÅĞØ.\n");
                        await ErrorReporter.MakeReport("SendX509KeyToServer(Subject subject)", new Exception("Îòïğàâêà çíà÷åíèé íåâîçìîæíà. ApiKey íå ïîëó÷åí."));
                        await StopAsync(new CancellationToken(true));
                    }
                }
                else
                {
                    _logger.LogError("\n[POST] ÎØÈÁ.\nÊÎÄ: {0}\nÑÁÙ.: {1}", response.StatusCode, response.ReasonPhrase);
                    await ErrorReporter.MakeReport("TryToRegisterService()", new Exception("Ğåãèñòğàöèÿ íå ïğîéäåíà. " + response.ReasonPhrase));
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
                                subjectName = item.Remove(0, 3);                                    // íåìíîãî âîëøåáñòâà
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
                ApplyNewApiKey(await TryToRegisterService());
            }
            else
            {
                _logger.LogInformation("\nÈÑÏ. ÊËŞ×\n[{0}]\n", _serviceParameters.ApiKey);
            }
            await base.StartAsync(cancellationToken);
        }
                
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
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
