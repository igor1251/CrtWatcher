using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;

namespace WA4D0GService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
                using var channel = GrpcChannel.ForAddress("https://localhost:5003");
                var client = new X509Communication.X509CommunicationClient(channel);
                var request = new ClientToServerSyncRequest();
                request.RequestType = RequestType.Append;
                request.Subjects.Add(new CertificateSubjectDTO()
                {
                    SubjectName = "test",
                    SubjectComment = "test subject",
                    SubjectPhone = "89610037151",
                });
                var response = await client.AppendCertificatesSubjectsToServerDatabaseAsync(request);
            }
        }
        /*
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
        }
        */
    }
}
