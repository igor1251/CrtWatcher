using Microsoft.Extensions.Logging;
using NetworkOperators.Identity.Client;
using System.Net.Http;
using System.Threading.Tasks;

namespace X509ObserverAdmin.Helpers
{
    public class Gatekeeper
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<Gatekeeper> _logger;

        public Gatekeeper(HttpClient httpClient,
                          ILogger<Gatekeeper> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task PrepareHttpClientAsync(ConnectionParameters connectionParameters)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", connectionParameters.ApiKey);
        }
    }
}
