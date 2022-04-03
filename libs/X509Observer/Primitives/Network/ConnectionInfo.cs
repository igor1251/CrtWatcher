using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace X509Observer.Primitives.Network
{
    public class ConnectionInfo : IConnectionInfo
    {
        string _ServerIP = string.Empty, _ServerPort = string.Empty, _Protocol = string.Empty;

        private readonly string IPv4_ADDR_REGEX = @"(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}";
        private readonly string IPv6_ADDR_REGEX = @"(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))";
        private readonly string PORT_REGEX = @"^((6553[0-5])|(655[0-2][0-9])|(65[0-4][0-9]{2})|(6[0-4][0-9]{3})|([1-5][0-9]{4})|([0-5]{0,5})|([0-9]{1,4}))$";

        [Required]
        [JsonPropertyName("server-ip")]
        public string ServerIP
        {
            get { return _ServerIP; }
            set
            {
                if (!Regex.IsMatch(value, IPv4_ADDR_REGEX) && !Regex.IsMatch(value, IPv6_ADDR_REGEX) && value != "localhost")
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
                if (!Regex.IsMatch(value, PORT_REGEX))
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
