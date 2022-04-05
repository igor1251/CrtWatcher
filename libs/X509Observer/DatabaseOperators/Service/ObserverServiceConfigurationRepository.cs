using System;
using System.IO;
using System.Threading.Tasks;
using X509Observer.Primitives.Service;
using System.Text.Json;

namespace X509Observer.DatabaseOperators.Service
{
    public class ObserverServiceConfigurationRepository : IObserverServiceConfigurationRepository
    {
        private readonly string configurationFileName = Environment.CurrentDirectory + "\\settings.json";

        public async Task<ObserverServiceConfiguration> LoadObserverServiceConfiguration()
        {
            FileStream fs = new FileStream(configurationFileName, FileMode.OpenOrCreate, FileAccess.Read);
            var configuration = new ObserverServiceConfiguration();
            try
            {
                if (fs.Length == 0) return null;
                configuration = await JsonSerializer.DeserializeAsync<ObserverServiceConfiguration>(fs);
                
            }
            catch(Exception ex)
            {
                File.AppendAllText("error.log", DateTime.Today.ToString() + " " + ex.Message);
            }
            finally
            {
                fs.Close();
            }
            return configuration;
        }

        public async Task SaveObserverServiceConfiguration(ObserverServiceConfiguration configuration)
        {
            FileStream fs = new FileStream(configurationFileName, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                await JsonSerializer.SerializeAsync<ObserverServiceConfiguration>(fs, configuration);
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.log", DateTime.Today.ToString() + " " + ex.Message);
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
