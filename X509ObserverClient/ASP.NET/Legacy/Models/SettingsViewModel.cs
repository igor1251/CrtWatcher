using DataStructures;

namespace Site.Models
{
    public class SettingsViewModel
    {
        public Settings Settings { get; set; }

        public SettingsViewModel(Settings settings)
        {
            Settings = settings;
        }
    }
}
