using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStructures;

namespace Observer
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
                    services.AddSingleton<IDbContext, UsersDbContext>();
                    services.AddSingleton<IDbContext, HostsDbContext>();
                    services.AddSingleton<IUsersStorageQueries, UsersStorageQueries>();
                    services.AddSingleton<ILocalUsersStorage, LocalUsersStorage>();
                    services.AddSingleton<IHostsStorageQueries, HostsStorageQueries>();
                    services.AddSingleton<IUsersStorage, UsersStorage>();
                    services.AddSingleton<IHostsStorage, HostsStorage>();
                    services.AddSingleton<ISettingsStorage, SettingsStorage>();
                    services.AddHostedService<Worker>();
                });
    }
}
