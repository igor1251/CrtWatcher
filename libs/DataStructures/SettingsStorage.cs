using System;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Net;

namespace DataStructures
{
    public class SettingsStorage : ISettingsStorage
    {
        private readonly string _settingsPath = Environment.CurrentDirectory + "\\settings.xml";

        private ClientHost GetHostInfo()
        {
            var host = new ClientHost();
            host.HostName = Dns.GetHostName();
            host.ConnectionPort = 5000;

            foreach (IPAddress ip in Dns.GetHostAddresses(host.HostName))
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    host.IP = ip.ToString();
                    break;
                }
            }

            return host;
        }

        public Task<Settings> LoadSettingsFromFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            var settings = new Settings();
            FileStream fs = new FileStream(_settingsPath, FileMode.OpenOrCreate, FileAccess.Read);
            if (fs.Length == 0)
            {
                fs.Close();
                settings.MainServerIP = GetHostInfo().IP;
                settings.MainServerPort = 5000;
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
