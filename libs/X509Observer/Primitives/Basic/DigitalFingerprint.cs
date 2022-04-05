using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace X509Observer.Primitives.Basic
{
    public class DigitalFingerprint : IDigitalFingerprint
    {
        private int _ID = 0;
        private string _Hash = string.Empty;
        private DateTime _Start = DateTime.MinValue, _End = DateTime.MinValue;

        public DigitalFingerprint(int ID, string hash, DateTime start, DateTime end)
        {
            _ID = ID;
            _Hash = hash;
            _Start = start;
            _End = end;
        }

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
