using ElectronicDigitalSignatire.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ElectronicDigitalSignatire.Models.Classes
{
    public class Certificate : ICertificate
    {
        int _id;
        string _certificateHash, _algorithm;
        DateTime _startDate, _endDate;

        [Required]
        [JsonPropertyName("id")]
        public int ID 
        {
            get => _id;
            set
            {
                if (value < 0) throw new ArgumentException("ID must be above or equal '0'");
                else _id = value;
            }
        }

        [Required]
        [JsonPropertyName("certificateHash")]
        public string CertificateHash 
        { 
            get => _certificateHash; 
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value)) 
                    throw new ArgumentException("Certificate hash cannot be an empty string, a space, or null");
                else _certificateHash = value;
            }
        }

        [Required]
        [JsonPropertyName("algorithm")]
        public string Algorithm 
        { 
            get => _algorithm; 
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Algorithm hash cannot be an empty string, a space, or null");
                else _algorithm = value;
            }
        }

        [Required]
        [JsonPropertyName("startDate")]
        public DateTime StartDate 
        { 
            get => _startDate; 
            set => _startDate = value; 
        }

        [Required]
        [JsonPropertyName("endDate")]
        public DateTime EndDate 
        { 
            get => _endDate; 
            set => _endDate = value; 
        }
    }
}
