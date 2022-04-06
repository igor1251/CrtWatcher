using System.Threading.Tasks;

namespace DataStructures
{
    public interface ISettingsStorage
    {
        Task<Settings> LoadSettings();
        Task UpdateSettings(Settings settings);
    }
}
