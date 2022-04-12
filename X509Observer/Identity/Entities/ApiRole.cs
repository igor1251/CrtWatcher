using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace X509Observer.Identity.Entities
{
    public class ApiRole
    {
        [Required]
        [JsonPropertyName("id")]
        public int ID { get; init; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; init; }
    }
}
