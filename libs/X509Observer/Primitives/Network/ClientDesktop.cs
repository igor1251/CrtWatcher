using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using X509Observer.MagicStrings.RegularExpressions;

namespace X509Observer.Primitives.Network
{
    public class ClientDesktop : IClientDesktop
    {
        private string _IP;
        private string _Name = string.Empty, _Comment = string.Empty;
        private DateTime _LastResponseTime = DateTime.Now;

        [Required]
        [JsonPropertyName("ip")]
        public string IP
        {
            get { return _IP; }
            set
            {
                if (!Regex.IsMatch(value, NetworkPartsValidationStrings.IPv6_ADDR_REGEX) && !Regex.IsMatch(value, NetworkPartsValidationStrings.IPv4_ADDR_REGEX) && value != "localhost")
                {
                    throw new ArgumentException("The entered IP address does not meet the requirements.");
                }
                _IP = value;
            }
        }

        [Required]
        [JsonPropertyName("name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        
        [Required]
        [JsonPropertyName("comment")]
        public string Comment
        {
            get { return _Comment; }
            set { _Comment = value; }
        }
        
        [Required]
        [JsonPropertyName("last-response-time")]
        public DateTime LastResponseTime
        {
            get { return _LastResponseTime; }
            set { _LastResponseTime = value; }
        }
    }
}
