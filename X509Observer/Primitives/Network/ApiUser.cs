using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace X509Observer.Primitives.Network
{
    public class ApiUser
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password-hash")]
        public string PasswordHash { get; set; }
    }
}
