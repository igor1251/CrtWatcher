using ClientHostCertificateService.Models;
using ClientHostCertificateService.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClientHostCertificateService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ServiceConfig _serviceConfig;
        private ServiceConfigStore _serviceConfigStore;


        public Worker(ILogger<Worker> logger, ServiceConfigStore serviceConfigStore)
        {
            _logger = logger;
            _serviceConfigStore = serviceConfigStore;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _serviceConfig = await _serviceConfigStore.LoadServiceConfigAsync();
            _logger.LogInformation("Loaded configuration:\nService condition: {0}\nFrequency of verification: {1} hours.", _serviceConfig.Condition.ToString(), _serviceConfig.FrequencyOfVerificateonInHours);
            //как-то правильно вызвать ExecuteAsync
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _serviceConfigStore.SaveServiceConfigAsync(_serviceConfig);
        }
    }
}
