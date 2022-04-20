using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NetworkOperators.Identity.DataTransferObjects
{
    public class UserAuthorizationResponse
    {
        [Required]
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
