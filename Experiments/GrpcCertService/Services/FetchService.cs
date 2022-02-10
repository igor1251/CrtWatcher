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

        public override async Task<CertificateSubjectReply> FetchCertificateSubjects(CertificateSubjectRequest request, ServerCallContext context)
        {
            var localStore = new LocalStore();
            var subjects = await localStore.LoadCertificateSubjectsAndCertificates();

            var certificateSubjectReply = new CertificateSubjectReply();
            certificateSubjectReply.Subjects.AddRange((IEnumerable<CertificateSubject>)subjects);
            return certificateSubjectReply;
        }
    }
}
