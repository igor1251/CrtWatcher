using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using ClientHostCertificateService.Models;

namespace ClientHostCertificateService.Services
{
    public class ServiceConfigStore
    {
        private readonly string _configFilePath = Environment.CurrentDirectory + "\\clientHostServiceConfig.cfg";

        public async Task<ServiceConfig> LoadServiceConfigAsync()
        {
            var serviceConfig = new ServiceConfig();
            try
            {
                using (FileStream configFileStream = new FileStream(_configFilePath, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    if (configFileStream.Length == 0) return serviceConfig;
                    serviceConfig = await JsonSerializer.DeserializeAsync<ServiceConfig>(configFileStream);
                }
                
            }    
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(Environment.CurrentDirectory + "\\err.log", DateTime.Now.ToString() + " " + ex.ToString() + "\n");
            }
            return serviceConfig;
        }

        public async Task SaveServiceConfigAsync(ServiceConfig serviceConfig)
        {
            try
            {
                using (FileStream configFileStream = new FileStream(_configFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    await JsonSerializer.SerializeAsync<ServiceConfig>(configFileStream, serviceConfig);
                }
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(Environment.CurrentDirectory + "\\err.log", DateTime.Now.ToString() + " " + ex.ToString() + "\n");
            }
        }
    }
}
