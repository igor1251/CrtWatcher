using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace X509Observer.Primitives.Basic
{
    public class DigitalFingerprint
    {
        private int _ID = 0;
        private string _Hash = string.Empty;
        private DateTime _Start = DateTime.MinValue, _End = DateTime.MinValue;

        [Required]
        [JsonPropertyName("id")]
        public int ID
        {
            get { return _ID; }
            init { _ID = value; }
        }

        [Required]
        [JsonPropertyName("hash")]
        public string Hash
        {
            get { return _Hash; }
            init { _Hash = value; }
        }

        [Required]
        [JsonPropertyName("start")]
        public DateTime Start
        {
            get { return _Start; }
            init { _Start = value; }
        }

        [Required]
        [JsonPropertyName("end")]
        public DateTime End
        {
            get { return _End; }
            init { _End = value; }
        }
    }
}
