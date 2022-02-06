using System.Threading.Tasks;
using ElectronicDigitalSignature.Models.Interfaces;

namespace ElectronicDigitalSignature.Services.Interfaces
{
    public interface ISettingsStore
    {
        Task<ISettings> LoadSettings();
        Task SaveSettings();
    }
}
