using ElectronicDigitalSignatire.Models.Classes;
using ElectronicDigitalSignatire.Services.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WA4D0GGrpcService
{
    public class X509CommService : X509Comm.X509CommBase
    {
        private readonly ILogger<X509CommService> _logger;
        private readonly ILocalStore _localStore;

        public X509CommService(ILogger<X509CommService> logger,
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

        public override async Task<CertificateSubjectReply> FetchCertificateSubjects(CertificateSubjectRequest request, ServerCallContext context)
        {
            var subjects = await _localStore.LoadCertificateSubjectsAndCertificates();
            var certificateSubjectReply = new CertificateSubjectReply();

            foreach (var item in subjects)
            {
                certificateSubjectReply.Subjects.Add(CertificateSubjectToDTOConverter(item));
            }

            return certificateSubjectReply;
        }
    }
}
