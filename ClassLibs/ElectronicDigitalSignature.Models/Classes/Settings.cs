using ElectronicDigitalSignature.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ElectronicDigitalSignature.Models.Classes
{
    public class Settings : ISettings
    {
        private int _daysBeforeWarnings = 0;

        [Required]
        [Range(0, 366, ErrorMessage = "Wrong days count")]
        [JsonPropertyName("daysBeforeWarning")]
        public int DaysBeforeWarning 
        {
            get => _daysBeforeWarnings;
            set => _daysBeforeWarnings = value;
        }
    }
}
