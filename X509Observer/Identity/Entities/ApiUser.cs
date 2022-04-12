using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace X509Observer.Identity.Entities
{
    public class ApiUser
    {
        [Required]
        [JsonPropertyName("id")]
        public int ID { get; init; }

        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password-hash")]
        public string PasswordHash { get; set; }

        [Required]
        [JsonPropertyName("role")]
        public ApiRole Role { get; set; }

        [JsonPropertyName("key")]
        public ApiKey Key { get; set; }
    }
}
