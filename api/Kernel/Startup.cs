using ElectronicDigitalSignatire.Services.Classes;
using ElectronicDigitalSignatire.Services.Interfaces;
using Kernel.gRPCServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel
{
    public class Startup
    {
        private const int HOST_REGISTRATION_SERVICE_PORT = 5003,
                          USER_REGISTRATION_SERVICE_PORT = 5004,
                          SETTINGS_EXCHANGE_SERVICE_PORT = 5005;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDbContext, DbContext>();
            services.AddSingleton<IDbStore, DbStore>();
            services.AddSingleton<ILocalStore, LocalStore>();
            services.AddSingleton<IQueryStore, QueryStore>();
            services.AddGrpc();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();

            app.MapWhen(context =>
            {
                return context.Connection.LocalPort == HOST_REGISTRATION_SERVICE_PORT;
            }, hostsRegApp =>
            {
                hostsRegApp.UseRouting();
                hostsRegApp.UseEndpoints(endpoints =>
                {
                    endpoints.MapGrpcService<HostsRegistrationService>();
                });
            });

            app.MapWhen(context =>
            {
                return context.Connection.LocalPort == USER_REGISTRATION_SERVICE_PORT;
            }, hostsRegApp =>
            {
                hostsRegApp.UseRouting();
                hostsRegApp.UseEndpoints(endpoints =>
                {
                    endpoints.MapGrpcService<UsersRegistrationService>();
                });
            });

            app.MapWhen(context =>
            {
                return context.Connection.LocalPort == SETTINGS_EXCHANGE_SERVICE_PORT;
            }, hostsRegApp =>
            {
                hostsRegApp.UseRouting();
                hostsRegApp.UseEndpoints(endpoints =>
                {
                    endpoints.MapGrpcService<SettingsExchangeService>();
                });
            });

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGrpcService<UsersRegistrationService>();
                endpoints.MapControllers();
            });
        }
    }
}
