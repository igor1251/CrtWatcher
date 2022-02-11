using ElectronicDigitalSignatire.Models.Classes;
using ElectronicDigitalSignatire.Models.Interfaces;
using ElectronicDigitalSignatire.Services.Interfaces;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WA4D0GServer.Services.Interfaces;

namespace WA4D0GServer.Services.Classes
{
    public class GrpcWorkerCommunicationService : IWorkerCommunicationService
    {
        private readonly ILogger<GrpcWorkerCommunicationService> _logger;

        public GrpcWorkerCommunicationService(ILogger<GrpcWorkerCommunicationService> logger)
        {
            _logger = logger;
        }

        private CertificateData CertificateDataFromDTOConverter(CertificateDataDTO certificateDataDTO)
        {
            var certificateData = new CertificateData();
            certificateData.ID = certificateDataDTO.Id;
            certificateData.Algorithm = certificateDataDTO.Algorithm;
            certificateData.CertificateHash = certificateDataDTO.CertificateHash;
            certificateData.StartDate = certificateDataDTO.StartDate.ToDateTime();
            certificateData.EndDate = certificateDataDTO.EndDate.ToDateTime();
            return certificateData;
        }

        private CertificateSubject CertificateSubjectFromDTOConverter(CertificateSubjectDTO certificateSubjectDTO)
        {
            var certificateSubject = new CertificateSubject();
            certificateSubject.ID = certificateSubjectDTO.Id;
            certificateSubject.SubjectName = certificateSubjectDTO.SubjectName;
            certificateSubject.SubjectComment = certificateSubjectDTO.SubjectComment;
            foreach (var item in certificateSubjectDTO.Certificates)
            {
                certificateSubject.CertificateList.Add(CertificateDataFromDTOConverter(item));
            }
            return certificateSubject;
        }

        public async Task<IEnumerable<ICertificateSubject>> FetchAllCertificateSubjects(string uri)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(uri);
                var client = new X509Comm.X509CommClient(channel);
                var reply = await client.FetchCertificateSubjectsAsync(
                                  new CertificateSubjectRequest { StorageName = "local" });
                var subjects = new List<CertificateSubject>();

                foreach (var item in reply.Subjects)
                {
                    subjects.Add(CertificateSubjectFromDTOConverter(item));
                }

                return subjects;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public Task<ICertificateSubject> FetchCertificateSubject(string uri)
        {
            _logger.LogError("Method not implemented!!!");
            return null;
        }
    }
}
