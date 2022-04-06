using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using X509Observer.MagicStrings.RegularExpressions;

namespace X509Observer.Primitives.Network
{
    public class ConnectionInfo
    {
        string _ServerIP = string.Empty, _ServerPort = string.Empty, _Protocol = string.Empty;

        [Required]
        [JsonPropertyName("server-ip")]
        public string ServerIP
        {
            get { return _ServerIP; }
            set
            {
                if (!Regex.IsMatch(value, NetworkPartsValidationStrings.IPv4_ADDR_REGEX) && !Regex.IsMatch(value, NetworkPartsValidationStrings.IPv6_ADDR_REGEX) && value != "localhost")
                {
                    throw new ArgumentException("The entered IP address does not meet the requirements.");
                }
                _ServerIP = value;
            }
        }
        
        [Required]
        [JsonPropertyName("server-port")]
        public string ServerPort
        {
            get { return _ServerPort; }
            set 
            {
                if (!Regex.IsMatch(value, NetworkPartsValidationStrings.PORT_NUMBER_REGEX))
                {
                    throw new ArgumentException("The entered port number does not meet the requirements.");
                }
                _ServerPort = value;
            }
        }
        
        [Required]
        [JsonPropertyName("protocol")]
        public string Protocol
        {
            get { return _Protocol; }
            set { _Protocol = value; }
        }
    }
}
