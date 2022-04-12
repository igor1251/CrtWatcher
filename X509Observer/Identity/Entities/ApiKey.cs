using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace X509Observer.Identity.Entities
{
    public class ApiKey
    {
        [Required]
        [JsonPropertyName("value")]
        public string Value { get; init; }

        [Required]
        [JsonPropertyName("expiration-time")]
        public DateTime ExpirationTime { get; init; }
    }
}
