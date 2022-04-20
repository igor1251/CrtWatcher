using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace X509ObserverWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _configuration["ApiKey"] = "test-api-key";
            _logger.LogInformation("Settings stored in appsettings.json:\n" +
                                   "\tRemoteRegistrationServiceAddress: " + _configuration["RemoteRegistrationServiceAddress"] + "\n" +
                                   "\tRemoteAuthenticationServiceAddress: " + _configuration["RemoteAuthenticationServiceAddress"] + "\n" +
                                   "\tRemoteX509VaultStoreService: " + _configuration["RemoteX509VaultStoreService"] + "\n" +
                                   "\tRemoteServiceLogin: " + _configuration["RemoteServiceLogin"] + "\n" +
                                   "\tRemoteServicePassword: " + _configuration["RemoteServicePassword"] + "\n" +
                                   "\tApiKey: " + _configuration["ApiKey"] + "\n" +
                                   "\tMonitoringInterval: " + _configuration["MonitoringInterval"] + "\n");
            return base.StartAsync(cancellationToken);
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
