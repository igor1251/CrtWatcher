using ElectronicDigitalSignatire.Services.Classes;
using ElectronicDigitalSignatire.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UsersRegistrationService.GrpcServices;

namespace UsersRegistrationService
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
                    services.AddSingleton<IDbContext, DbContext>();
                    services.AddSingleton<ILocalStore, LocalStore>();
                    services.AddSingleton<IDbStore, DbStore>();
                    services.AddSingleton<IQueryStore, QueryStore>();
                    services.AddSingleton<RegistrationService>();
                    services.AddHostedService<Worker>();
                });
    }
}
