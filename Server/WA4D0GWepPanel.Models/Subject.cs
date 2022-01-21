using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WA4D0GWebPanel.Models
{
    public class Subject
    {
        [Required]
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [Required]
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [Phone]
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("certificates")]
        public List<Certificate> Certificates { get; set; }
    }
}
