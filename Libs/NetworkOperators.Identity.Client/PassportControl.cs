using NetworkOperators.Identity.DataTransferObjects;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Tools.Reporters;

namespace NetworkOperators.Identity.Client
{
    public class PassportControl
    {
        private HttpClient _httpClient;

        public PassportControl(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> RegisterClient(ConnectionParameters connectionParameters)
        {
            var registrationRequestDTO = new UserAuthorizationRequest()
            {
                UserName = connectionParameters.RemoteServiceLogin,
                Password = connectionParameters.RemoteServicePassword
            };

            var apiKey = string.Empty;

            using (var response = await _httpClient.PostAsJsonAsync(connectionParameters.RemoteRegistrationServiceAddress, registrationRequestDTO))
            {
                if (response.IsSuccessStatusCode)
                {
                    apiKey = JsonSerializer.Deserialize<UserAuthorizationResponse>(await response.Content.ReadAsStringAsync()).Token;
                }
                else
                {
                    await ErrorReporter.MakeReport("TryToRegisterService()", new Exception("Unable to register service. " + response.ReasonPhrase));
                }
            }

            return apiKey;
        }
    }
}
