using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace X509KeysVault.Entities
{
    public class Subject
    {
        private int _ID = 0;
        private string _Name = string.Empty, _Phone = string.Empty;
        private List<DigitalFingerprint> _Fingerprints = new List<DigitalFingerprint>();

        private const string PHONE_TEMPLATE_REGULAR_EXPRESSION = @"^(\s*)?(\+)?([- _():=+]?\d[- _():=+]?){10,14}(\s*)?$";

        [Required]
        [JsonPropertyName("id")]
        public int ID
        {
            get { return _ID; }
            init { _ID = value; }
        }

        [Required]
        [JsonPropertyName("name")]
        public string Name
        {
            get { return _Name; }
            init { _Name = value; }
        }

        [JsonPropertyName("phone")]
        public string Phone
        {
            get { return _Phone; }
            set
            {
                //if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                //{
                //    _Phone = string.Empty;
                //}
                //else if (!Regex.IsMatch(value, PHONE_TEMPLATE_REGULAR_EXPRESSION))
                //{
                //    throw new ArgumentException("The entered phone number does not meet the requirements.");
                //}
                //else _Phone = value;
                _Phone = value;
            }
        }

        [Required]
        [JsonPropertyName("fingerprints")]
        public List<DigitalFingerprint> Fingerprints
        {
            get { return _Fingerprints; }
            set
            {
                _Fingerprints = value;
            }
        }
    }
}
