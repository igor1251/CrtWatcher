using ClientHostCertificateService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientHostCertificateService
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
                    services.AddSingleton<ServiceConfigStore>();
                    //services.AddSingleton<>();
                    //services.AddSingleton<>();
                    //services.AddSingleton<>();
                    //services.AddSingleton<>();
                    //services.AddSingleton<>();
                });
    }
}
