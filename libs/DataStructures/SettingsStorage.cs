using System;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace DataStructures
{
    public class SettingsStorage : ISettingsStorage
    {
        private readonly string _settingsPath = Environment.CurrentDirectory + "\\settings.xml";

        public Task<Settings> LoadSettingsFromFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(_settingsPath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                if (fs.Length == 0) return Task.FromResult(new Settings());
                var settings = serializer.Deserialize(fs) as Settings;
                return Task.FromResult(settings);
            }
        }

        public Task SaveSettingsToFile(Settings settings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(_settingsPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                serializer.Serialize(fs, settings);
                return Task.FromResult(0);
            }
        }
    }
}
