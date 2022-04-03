using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace X509Observer.Primitives.Network
{
    public class ClientDesktop : IClientDesktop
    {
        private string _IP;
        private string _Name = string.Empty, _Comment = string.Empty;
        private DateTime _LastResponseTime = DateTime.Now;

        private readonly string IPv4_ADDR_REGEX = @"(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}";
        private readonly string IPv6_ADDR_REGEX = @"(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))";

        [Required]
        [JsonPropertyName("ip")]
        public string IP
        {
            get { return _IP; }
            set
            {
                if (!Regex.IsMatch(value, IPv6_ADDR_REGEX) && !Regex.IsMatch(value, IPv4_ADDR_REGEX) && value != "localhost")
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
