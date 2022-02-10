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
        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Fetcher.FetcherClient(channel);
            var reply = await client.FetchCertificateSubjectsAsync(
                              new CertificateSubjectRequest { StorageName = "local" });
            var subjects = new List<CertificateSubject>();
            subjects.AddRange(reply.Subjects);

            foreach (var item in subjects)
            {
                Console.WriteLine(item.SubjectName);
            }

            Console.ReadKey();
        }
    }
}