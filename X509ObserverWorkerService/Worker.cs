using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using NetworkOperators.Identity.DataTransferObjects;
using Tools.Reporters;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace X509ObserverWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public Worker(ILogger<Worker> logger, 
                      IConfiguration configuration, 
                      HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        private async Task<string> TryToRegisterService()
        {
            var registrationRequest = new UserAuthorizationRequest()
            {
                UserName = _configuration["RemoteServiceLogin"],
                Password = _configuration["RemoteServicePassword"]
            };

            var apiKey = string.Empty;

            using (var requestContent = new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json"))
            {
                using (var response = await _httpClient.PostAsync(_configuration["RemoteRegistrationServiceAddress"], requestContent))
                {
                    _logger.LogInformation("registration [POST] request => " + _configuration["RemoteRegistrationServiceAddress"] + "\nrequestContent = " + requestContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        apiKey = JsonSerializer.Deserialize<UserAuthorizationResponse>(responseString).Token;
                        _logger.LogInformation("recieved token : {0}", apiKey);
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

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_configuration["ApiKey"]))
            {
                if (!string.IsNullOrEmpty(_configuration["RemoteServiceLogin"]) && 
                    !string.IsNullOrEmpty(_configuration["RemoteServicePassword"]))
                {
                    var apiKey = await TryToRegisterService();
                    if (!string.IsNullOrEmpty(apiKey))
                    {
                        _configuration["ApiKey"] = apiKey;
                    }
                    else
                    {
                        _logger.LogInformation("ApiKey is empty");
                        await StopAsync(cancellationToken);
                    }
                }
                else
                {
                    await ErrorReporter.MakeReport("StartAsync(CancellationToken cancellationToken)", new Exception("Credentials is not specified"));
                    await StopAsync(cancellationToken);
                }
            }

            _logger.LogInformation("\n\nSettings stored in appsettings.json:\n" +
                                   "RemoteRegistrationServiceAddress: " + _configuration["RemoteRegistrationServiceAddress"] + "\n" +
                                   "RemoteAuthenticationServiceAddress: " + _configuration["RemoteAuthenticationServiceAddress"] + "\n" +
                                   "RemoteX509VaultStoreService: " + _configuration["RemoteX509VaultStoreService"] + "\n" +
                                   "RemoteServiceLogin: " + _configuration["RemoteServiceLogin"] + "\n" +
                                   "RemoteServicePassword: " + _configuration["RemoteServicePassword"] + "\n" +
                                   "ApiKey: " + _configuration["ApiKey"] + "\n" +
                                   "MonitoringInterval: " + _configuration["MonitoringInterval"] + "\n\n");

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
