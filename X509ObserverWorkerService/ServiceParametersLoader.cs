using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using Tools.Reporters;

namespace X509ObserverWorkerService
{
    internal static class ServiceParametersLoader
    {
        private const string configFileName = "parameters.json";

        internal static async Task<ServiceParameters> ReadServiceParameters()
        {
            var serviceParameters = new ServiceParameters();
            try
            {
                if (File.Exists(configFileName))
                    using (FileStream fs = new FileStream(configFileName, FileMode.Open, FileAccess.Read))
                    {
                        serviceParameters = await JsonSerializer.DeserializeAsync<ServiceParameters>(fs);
                    }
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("ReadServiceParameters()", ex);
            }
            return serviceParameters;
        }

        internal static async Task WriteServiceParameters(ServiceParameters serviceParameters)
        {
            try
            {
                if (serviceParameters == null) serviceParameters = new ServiceParameters();
                using (FileStream fs = new FileStream(configFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    await JsonSerializer.SerializeAsync(fs, serviceParameters);
                }
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("WriteServiceParameters(ServiceParameters serviceParameters)", ex);
            }
        }
    }
}
