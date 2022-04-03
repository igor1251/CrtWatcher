using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace X509Observer.Primitives.Base
{
    public class X509CertificateInfo : IX509CertificateInfo
    {
        private int _ID;
        private string _Hash = string.Empty;
        private DateTime _Start, _End;

        [Required]
        [JsonPropertyName("id")]
        public int ID
        {
            get { return _ID; }
        }

        [Required]
        [JsonPropertyName("hash")]
        public string Hash
        {
            get { return _Hash; }
        }

        [Required]
        [JsonPropertyName("start")]
        public DateTime Start
        {
            get { return _Start; }
        }

        [Required]
        [JsonPropertyName("end")]
        public DateTime End
        {
            get { return _End; }
        }
    }
}
