using System.Threading.Tasks;

namespace DataStructures
{
    public interface ISettingsStorage
    {
        Task<Settings> LoadSettingsFromFile();
        Task SaveSettingsToFile(Settings settings);
    }
}
