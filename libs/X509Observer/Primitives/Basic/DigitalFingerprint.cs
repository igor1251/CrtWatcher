using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace X509Observer.Primitives.Basic
{
    public class DigitalFingerprint : IDigitalFingerprint
    {
        private int _ID;
        private string _Hash = string.Empty;
        private DateTime _Start, _End;

        public DigitalFingerprint()
        {
            _ID = 0;
            _Start = DateTime.MinValue;
            _End = DateTime.MinValue;
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
