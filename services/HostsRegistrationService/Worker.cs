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

        private const int _port = 5005;
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

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _server.Start();
                _logger.LogInformation("Hosts registration service successfully started. Listening on {0}:{1}", _host, _port);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Task.FromResult(1);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) { }

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
