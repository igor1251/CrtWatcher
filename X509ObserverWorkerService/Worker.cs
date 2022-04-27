using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tools.Reporters;
using System.Net.Http;
using X509KeysVault.Entities;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Json;
using NetworkOperators.Identity.Client;

namespace X509ObserverWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HttpClient _httpClient;
        private ConnectionParameters _connectionParameters;
        private PassportControl _passportControl;

        public Worker(ILogger<Worker> logger, 
                      HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _passportControl = new PassportControl(httpClient);
        }

        private async Task SendX509KeyToServer(Subject subject)
        {
            _logger.LogInformation("\n����. [POST] => {0}\n", _connectionParameters.RemoteX509VaultStoreService);
            using (var response = await _httpClient.PostAsJsonAsync(_connectionParameters.RemoteX509VaultStoreService, subject))
            {
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("\n[POST] �����.\n");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogInformation("\n���� �� ���. �������.\n");
                    ApplyNewApiKey(await _passportControl.RegisterClient(_connectionParameters));
                    if (_connectionParameters.ApiKey != string.Empty)
                    {
                        _logger.LogInformation("\n�����. ���. ��. APIKEY\n[{0}]\n", _connectionParameters.ApiKey);
                    }
                    else
                    {
                        _logger.LogError("\n����. ���� �� �����. ������.\n");
                        await ErrorReporter.MakeReport("SendX509KeyToServer(Subject subject)", new Exception("�������� �������� ����������. ApiKey �� �������."));
                        await StopAsync(new CancellationToken(true));
                    }
                }
                else
                {
                    _logger.LogError("\n[POST] ����.\n���: {0}\n���.: {1}", response.StatusCode, response.ReasonPhrase);
                    await ErrorReporter.MakeReport("TryToRegisterService()", new Exception("����������� �� ��������. " + response.ReasonPhrase));
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
                                subjectName = item.Remove(0, 3);                                    // ������� ����������
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

        private void ApplyNewApiKey(string apiKey)
        {
            _connectionParameters.ApiKey = apiKey;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _connectionParameters.ApiKey);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _connectionParameters = await ConnectionParametersLoader.ReadServiceParameters();
            if (string.IsNullOrEmpty(_connectionParameters.ApiKey))
            {
                ApplyNewApiKey(await _passportControl.RegisterClient(_connectionParameters));
            }
            else
            {
                _logger.LogInformation("\n���. ����\n[{0}]\n", _connectionParameters.ApiKey);
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
                Thread.Sleep(_connectionParameters.MonitoringInterval);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await ConnectionParametersLoader.WriteServiceParameters(_connectionParameters);
            await base.StopAsync(cancellationToken);
        }
    }
}
