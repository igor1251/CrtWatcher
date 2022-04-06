using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DataStructures
{
    public class ClientHost : IClientHost
    {
        private string _hostName, _ip;
        private int _connectionPort;

        private readonly string IPv4_ADDR_REGEX = @"(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}";
        private readonly string IPv6_ADDR_REGEX = @"(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))";

        public ClientHost()
        {
            _hostName = _ip = "";
            _connectionPort = 1;
        }

        [Required]
        [JsonPropertyName("hostName")]
        public string HostName
        {
            get => string.IsNullOrEmpty(_hostName) ? "" : _hostName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Invalid hostname: [" + value + "]");
                }
                _hostName = value;
            }
        }

        [Required]
        [JsonPropertyName("ip")]
        public string IP
        {
            get => string.IsNullOrEmpty(_ip) ? "" : _ip;
            set
            {
                if (string.IsNullOrEmpty(value) || 
                    (!Regex.IsMatch(value, IPv4_ADDR_REGEX) &&
                     !Regex.IsMatch(value, IPv6_ADDR_REGEX)) && 
                     value != "localhost")
                {
                    throw new ArgumentException("Invalid ip address: [" + value + "]");
                }
                _ip = value;
            }
        }

        [Required]
        [JsonPropertyName("connectionPort")]
        public int ConnectionPort
        {
            get => _connectionPort;
            set
            {
                if (value < 0 || value > 65535)
                {
                    throw new ArgumentException("Invalid port: [" + value + "]. It must be in range between 1 and 65535");
                }
                _connectionPort = value;
            }
        }
    }
}
