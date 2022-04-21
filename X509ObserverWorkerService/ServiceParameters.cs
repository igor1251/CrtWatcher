using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using Tools.Reporters;

namespace X509ObserverWorkerService
{
    public class ServiceParameters
    {
        [Required]
        [JsonPropertyName("RemoteRegistrationServiceAddress")]
        public string RemoteRegistrationServiceAddress { get; set; } = "http://localhost:5000/passport/register/";
        [Required]
        [JsonPropertyName("RemoteAuthenticationServiceAddress")]
        public string RemoteAuthenticationServiceAddress { get; set; } = "http://localhost:5000/passport/login/";
        [Required]
        [JsonPropertyName("RemoteX509VaultStoreService")]
        public string RemoteX509VaultStoreService { get; set; } = "http://localhost:5000/api/subjects/";
        [Required]
        [JsonPropertyName("RemoteServiceLogin")]
        public string RemoteServiceLogin { get; set; } = "service";
        [Required]
        [JsonPropertyName("RemoteServicePassword")]
        public string RemoteServicePassword { get; set; } = "service";
        [Required]
        [JsonPropertyName("MonitoringInterval")]
        public int MonitoringInterval { get; set; } = 10000;
        [Required]
        [JsonPropertyName("ApiKey")]
        public string ApiKey { get; set; } = string.Empty;
    }
}
