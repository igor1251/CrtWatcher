using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NetworkOperators.Identity.Entities
{
    public class User
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
        public string Role { get; set; }
    }
}
