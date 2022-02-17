using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using HostsRegistrationService.GrpcServices;

namespace HostsRegistrationService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private const int _port = 5050;
        private const string _host = "localhost";

        Server _server;


        public Worker(ILogger<Worker> logger,
                      RegistrationService registrationService)
        {
            _logger = logger;
            _server = new Server
            {
                Services = { ClientHostsRegistrationService.BindService(registrationService) },
                Ports = { new ServerPort(_host, _port, ServerCredentials.Insecure) }
            };
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _server.Start();
                _logger.LogInformation("Registration service successfully started. Listening on {0}:{1}", "localhost", _port);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _server.ShutdownAsync();
                _logger.LogInformation("Registration service has been stopped.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
