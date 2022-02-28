using DataStructures;

namespace Site.Models
{
    public class SettingsViewModel
    {
        public Settings ServiceSettings { get; set; }

        public SettingsViewModel(Settings settings)
        {
            ServiceSettings = settings;
        }
    }
}
