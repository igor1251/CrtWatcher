using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using System;
using DataStructures;
using Site.Models;

namespace Site.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private HttpClient _httpClient;

        public SettingsController(ILogger<SettingsController> logger,
                                  IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("ApiHttpClient");
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

        public async Task<IActionResult> Index()
        {
            var settings = await LoadInfo<Settings>(RequestLinks.GetSettings);
            _logger.LogInformation("Loaded settings is:\nServerIP = {0}\nServerPort = {1}\nWarnSecondsCount = {2}", settings.MainServerIP, settings.MainServerPort, settings.VerificationFrequency);
            return View(new SettingsViewModel(settings));
        }

        public async Task<IActionResult> SettingsEdit(Settings settings)
        {
            _logger.LogInformation("New settings is:\nServerIP = {0}\nServerPort = {1}\nWarnSecondsCount = {2}", settings.MainServerIP, settings.MainServerPort, settings.VerificationFrequency);

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var updatedSettings = JsonSerializer.Serialize<Settings>(settings);
            using (var requestContent = new StringContent(updatedSettings, Encoding.UTF8, "application/json"))
            {
                using (var response = await _httpClient.PutAsync(RequestLinks.SettingsResponseLink, requestContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
