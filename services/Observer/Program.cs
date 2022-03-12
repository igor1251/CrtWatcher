using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using Polly.Extensions.Http;
using DataStructures;
using System.Net.Http;

namespace Observer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient("ApiHttpClient").AddPolicyHandler(GetRetryPolicy());
                    services.AddSingleton<IDbContext, DbContext>();
                    services.AddSingleton<IUsersStorageQueries, UsersStorageQueries>();
                    services.AddSingleton<ILocalUsersStorage, LocalUsersStorage>();
                    services.AddSingleton<IHostsStorageQueries, HostsStorageQueries>();
                    services.AddSingleton<IUsersStorage, UsersStorage>();
                    services.AddSingleton<IHostsStorage, HostsStorage>();
                    services.AddSingleton<IBaseStorageQueries, BaseStorageQueries>();
                    services.AddSingleton<ISettingsStorage, SettingsStorage>();
                    services.AddSingleton<ObserverConditionLoader>();
                    services.AddHostedService<Worker>();
                });
    }
}
