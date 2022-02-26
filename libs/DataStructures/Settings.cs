using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataStructures
{
    public class Settings : ISettings
    {
        private int _verificationFrequency = 10, _mainServerPort = 5000;
        private string _mainServerIP = "localhost";

        private const string SERVER_IP_REGULAR_EXPRESSION = @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)"
;

        [Required]
        [JsonPropertyName("verificationFrequency")]
        public int VerificationFrequency
        {
            get => _verificationFrequency;
            set
            {
                if (value <= 0) throw new ArgumentException("The check frequency must be greater than 0");
                _verificationFrequency = value;
            }
        }

        [Required]
        [JsonPropertyName("mainServerPort")]
        [Range(1, 65535)]
        public int MainServerPort
        {
            get => _mainServerPort;
            set
            {
                if (value <= 0) throw new ArgumentException("The port number should be in the range from 1 to 65535");
                _mainServerPort = value;
            }
        }

        [Required]
        [JsonPropertyName("mainServerIP")]
        [RegularExpression(SERVER_IP_REGULAR_EXPRESSION, ErrorMessage = "Invalid server IP address")]
        public string MainServerIP
        {
            get => _mainServerIP;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("The server address cannot be empty");
            }
        }
    }
}
