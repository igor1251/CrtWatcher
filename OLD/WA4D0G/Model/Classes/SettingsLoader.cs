using System;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using WA4D0G.Model.Interfaces;
using WA4D0G.MaintenanceTools;

namespace WA4D0G.Model.Classes
{
    public class SettingsLoader : ISettingsLoader
    {
        private readonly string settingsPath = Environment.CurrentDirectory + "\\Config\\settings.json";

        public async Task<Settings> LoadSettingsAsync()
        {
            using (FileStream fs = new FileStream(settingsPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                if (fs.Length > 0)
                {
                    try
                    {
                        return await JsonSerializer.DeserializeAsync<Settings>(fs);
                    }
                    catch (Exception ex)
                    {
                        await Logger.WriteAsync("Error: Can't load settings. " + ex.Message);
                    }
                }
                return new Settings();
            }
        }

        public async Task SaveSettingsAsync(ISettings settings)
        {
            using (FileStream fs = new FileStream(settingsPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                try
                {
                    await JsonSerializer.SerializeAsync(fs, settings);
                }
                catch (Exception ex)
                {
                    await Logger.WriteAsync("Error: Can't save settings. " + ex.Message);
                }
            }
        }
    }
}
