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
            var settings = new Settings();
            FileStream fs = new FileStream(_settingsPath, FileMode.OpenOrCreate, FileAccess.Read);
            if (fs.Length == 0)
            {
                fs.Close();
                SaveSettingsToFile(settings);
                return Task.FromResult(settings);
            }
            settings = serializer.Deserialize(fs) as Settings;
            fs.Close();
            return Task.FromResult(settings);
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
