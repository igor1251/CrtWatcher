using ElectronicDigitalSignatire.Models.Classes;
using ElectronicDigitalSignatire.Services.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WA4D0GServer
{
    public class X509CommunicationService : X509Communication.X509CommunicationBase
    {
        private readonly ILogger<X509CommunicationService> _logger;
        private readonly IDbStore _dbStore;

        public X509CommunicationService(ILogger<X509CommunicationService> logger,
                               IDbStore dbStore)
        {
            _logger = logger;
            _dbStore = dbStore;
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

        public override Task<ClientToServerSyncResponse> AppendCertificatesSubjectsToServerDatabase(ClientToServerSyncRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Request recieved. Type: " + request.RequestType.ToString());
            var response = new ClientToServerSyncResponse();

            if (request.RequestType == RequestType.Append)
            {
                foreach (var item in request.Subjects)
                {
                    _dbStore.InsertSubject(CertificateSubjectFromDTOConverter(item));
                }
                response.ResponseType = ResponseType.Accepted;

            }
            else
            {
                response.ResponseType = ResponseType.Declined;
            }
            _logger.LogInformation("Response created. Type: " + response.ResponseType.ToString());

            return Task.FromResult(response);
        }
    }
}
