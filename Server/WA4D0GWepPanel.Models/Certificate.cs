using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WA4D0GWebPanel.Models
{
    public class Certificate
    {
        [Required]
        [JsonPropertyName("id")]
        public int ID { get; set; }
        
        [Required]
        [JsonPropertyName("algorithm")]
        public string Algorithm { get; set; }
        
        [Required]
        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [Required]
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [Required]
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
    }
}
