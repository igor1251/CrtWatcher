using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using System;

namespace Site.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private HttpClient _httpClient;

        public UsersController(ILogger<UsersController> logger,
                               IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("UsersHttpClient");
        }

        private async Task<T> LoadInfo<T>(string request) where T : new()
        {
            var result = new T();

            using (var response = await _httpClient.GetAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                    return result;
                }

                try
                {
                    var contentJsonString = await response.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize<T>(contentJsonString);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return result;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
