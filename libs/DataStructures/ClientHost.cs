using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataStructures
{
    public class ClientHost : IClientHost
    {
        private string _hostName, _ip;
        private int _connectionPort;

        public ClientHost()
        {
            _hostName = _ip = "";
            _connectionPort = 0;
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
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Invalid ip address: [" + value + "]");
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
