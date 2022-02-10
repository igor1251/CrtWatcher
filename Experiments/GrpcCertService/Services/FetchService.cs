using CrtLoader.Model.Classes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrpcCertService
{
    public class FetchService : Fetcher.FetcherBase
    {
        private readonly ILogger<FetchService> _logger;
        public FetchService(ILogger<FetchService> logger)
        {
            _logger = logger;
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
            certificateSubjectDTO.SubjectPhone= certificateSubject.SubjectPhone;
            certificateSubjectDTO.SubjectComment = certificateSubject.SubjectComment;
            
            foreach (var item in certificateSubject.CertificateList)
            {
                certificateSubjectDTO.Certificates.Add(CertificateDataToDTOConverter(item));
            }

            return certificateSubjectDTO;
        }

        public override async Task<CertificateSubjectReply> FetchCertificateSubjects(CertificateSubjectRequest request, ServerCallContext context)
        {
            var localStore = new LocalStore();
            var subjects = await localStore.LoadCertificateSubjectsAndCertificates();

            var certificateSubjectReply = new CertificateSubjectReply();

            foreach (var item in subjects)
            {
                certificateSubjectReply.Subjects.Add(CertificateSubjectToDTOConverter(item));
            }

            return certificateSubjectReply;
        }
    }
}
