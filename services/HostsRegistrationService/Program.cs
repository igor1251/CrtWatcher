using HostsRegistrationService.GrpcServices;
using HostsRegistrationService.Services.Classes;
using HostsRegistrationService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostsRegistrationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IDbContext, DbContext>();
                    services.AddSingleton<IHostStore, HostStore>();
                    services.AddSingleton<RegistrationService>();
                });
    }
}