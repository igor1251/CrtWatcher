using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using UsersRegistrationService.GrpcServices;

namespace UsersRegistrationService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private const int _port = 5004;
        private const string _host = "localhost";

        Server _server;

        public Worker(ILogger<Worker> logger,
                      RegistrationService registrationService)
        {
            _logger = logger;
            _server = new Server
            {
                Services = { CertificateUsersRegistrationService.BindService(registrationService) },
                Ports = { new ServerPort(_host, _port, ServerCredentials.Insecure) }
            };
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _server.Start();
                _logger.LogInformation("Users registration service successfully started. Listening on {0}:{1}", "localhost", _port);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Task.FromResult(1);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _server.ShutdownAsync();
                _logger.LogInformation("Users registration service has been stopped.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
