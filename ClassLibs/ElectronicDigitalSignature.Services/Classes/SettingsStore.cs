using ElectronicDigitalSignature.Models.Interfaces;
using ElectronicDigitalSignature.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ElectronicDigitalSignature.Services.Classes
{
    public class SettingsStore : ISettingsStore
    {
        public Task<ISettings> LoadSettings()
        {
            throw new NotImplementedException();
        }

        public Task SaveSettings()
        {
            throw new NotImplementedException();
        }
    }
}
