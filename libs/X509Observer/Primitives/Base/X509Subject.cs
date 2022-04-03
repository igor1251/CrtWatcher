using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace X509Observer.Primitives.Base
{
    public class X509Subject : IX509Subject
    {
        private int _ID;
        private string _Name = string.Empty, _Phone = string.Empty;
        private IEnumerable<IX509CertificateInfo> _Certificates;

        const string PHONE_TEMPLATE_REGULAR_EXPRESSION = @"^(\s*)?(\+)?([- _():=+]?\d[- _():=+]?){10,14}(\s*)?$";

        [Required]
        [JsonPropertyName("id")]
        public int ID
        {
            get { return _ID; }
        }

        [Required]
        [JsonPropertyName("name")]
        public string Name
        {
            get { return _Name; }
        }

        [Required]
        [JsonPropertyName("phone")]
        public string Phone
        {
            get { return _Phone; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    _Phone = string.Empty;
                }
                else if (!Regex.IsMatch(value, PHONE_TEMPLATE_REGULAR_EXPRESSION))
                {
                    throw new ArgumentException("The entered phone number does not meet the requirements.");
                }
                else _Phone = value;
            }
        }

        [Required]
        [JsonPropertyName("certificates")]
        public IEnumerable<IX509CertificateInfo> Certificates
        {
            get { return _Certificates; }
            set
            {
                _Certificates = value;
            }
        }
    }
}
