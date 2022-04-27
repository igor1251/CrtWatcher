using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Tools.Reporters;

namespace NetworkOperators.Identity.Client
{
    public class ConnectionParametersLoader
    {
        private const string configFileName = "parameters.json";

        public static async Task<ConnectionParameters> ReadServiceParameters()
        {
            var serviceParameters = new ConnectionParameters();
            try
            {
                if (File.Exists(configFileName))
                    using (FileStream fs = new FileStream(configFileName, FileMode.Open, FileAccess.Read))
                    {
                        serviceParameters = await JsonSerializer.DeserializeAsync<ConnectionParameters>(fs);
                    }
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("ReadServiceParameters()", ex);
            }
            return serviceParameters;
        }

        public static async Task WriteServiceParameters(ConnectionParameters serviceParameters)
        {
            try
            {
                if (serviceParameters == null) serviceParameters = new ConnectionParameters();
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
