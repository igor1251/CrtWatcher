using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using ElectronicDigitalSignatire.Services.Interfaces;
using ElectronicDigitalSignatire.Models.Classes;

namespace WA4D0GService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ILocalStore _localStore;

        private readonly int DELAY = 20000;

        public Worker(ILogger<Worker> logger,
                      ILocalStore localStore)
        {
            _logger = logger;
            _localStore = localStore;
        }

        private CertificateDataDTO CertificateDataToDTOConverter(CertificateData certificateData)
        {
            var certificateDataDTO = new CertificateDataDTO();
            certificateDataDTO.Id = certificateData.ID;
            certificateDataDTO.Algorithm = certificateData.Algorithm;
            certificateDataDTO.CertificateHash = certificateData.CertificateHash;
            certificateDataDTO.StartDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(certificateData.StartDate.ToUniversalTime());
            certificateDataDTO.EndDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(certificateData.EndDate.ToUniversalTime());
            return certificateDataDTO;
        }

        private CertificateSubjectDTO CertificateSubjectToDTOConverter(CertificateSubject certificateSubject)
        {
            var certificateSubjectDTO = new CertificateSubjectDTO();
            certificateSubjectDTO.SubjectName = certificateSubject.SubjectName;
            certificateSubjectDTO.SubjectPhone = certificateSubject.SubjectPhone;
            certificateSubjectDTO.SubjectComment = certificateSubject.SubjectComment;

            foreach (var item in certificateSubject.CertificateList)
            {
                certificateSubjectDTO.Certificates.Add(CertificateDataToDTOConverter(item));
            }

            return certificateSubjectDTO;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Sending request to server: {time}", DateTimeOffset.Now);
                using (var channel = GrpcChannel.ForAddress("https://localhost:5003"))
                {
                    var client = new X509Communication.X509CommunicationClient(channel);
                    var request = new ClientToServerSyncRequest();
                    request.RequestType = RequestType.Append;

                    var subjects = await _localStore.LoadCertificateSubjectsAndCertificates();
                    foreach (var item in subjects)
                    {
                        request.Subjects.Add(CertificateSubjectToDTOConverter(item));
                    }
                    var response = await client.AppendCertificatesSubjectsToServerDatabaseAsync(request);
                    _logger.LogInformation("Server response: " + response.ResponseType.ToString());
                }
                await Task.Delay(DELAY, stoppingToken);
            }
        }
    }
}
