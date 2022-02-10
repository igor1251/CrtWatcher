using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CrtLoader.Model.Classes;
using Grpc.Net.Client;

namespace GrpcGreeterClient
{
    class Program
    {
        static CertificateData CertificateDataFromDTOConverter(CertificateDataDTO certificateDataDTO)
        {
            var certificateData = new CertificateData();
            certificateData.ID = certificateDataDTO.Id;
            certificateData.Algorithm = certificateDataDTO.Algorithm;
            certificateData.CertificateHash = certificateDataDTO.CertificateHash;
            certificateData.StartDate = certificateDataDTO.StartDate.ToDateTime();
            certificateData.EndDate = certificateDataDTO.EndDate.ToDateTime();
            return certificateData;
        }

        static CertificateSubject CertificateSubjectFromDTOConverter(CertificateSubjectDTO certificateSubjectDTO)
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

        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Fetcher.FetcherClient(channel);
            var reply = await client.FetchCertificateSubjectsAsync(
                              new CertificateSubjectRequest { StorageName = "local" });
            var subjects = new List<CertificateSubject>();
            
            foreach (var item in reply.Subjects)
            {
                subjects.Add(CertificateSubjectFromDTOConverter(item));
            }

            foreach (var subject in subjects)
            {
                Console.WriteLine("{0}\t{1}\t{2}", subject.SubjectName, subject.SubjectPhone, subject.SubjectComment);
                foreach (var certificate in subject.CertificateList)
                {
                    Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", certificate.Algorithm, certificate.CertificateHash, certificate.StartDate.ToString(), certificate.EndDate.ToString());
                }
            }

            Console.ReadKey();
        }
    }
}