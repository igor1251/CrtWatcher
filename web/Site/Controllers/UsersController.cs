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
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private HttpClient _httpClient;

        public UsersController(ILogger<UsersController> logger,
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

        private async Task<User> LoadUserInfo(int id)
        {
            return await LoadInfo<User>(RequestLinks.UsersResponseLink + id); ;
        }

        public async Task<IActionResult> Index()
        {
            var users = await LoadInfo<List<User>>(RequestLinks.GetUsersFromDbLink);
            return View(new UsersViewModel(users));
        }

        public async Task<IActionResult> Details(int id)
        {
            var userDetailsViewModel = new UserDetailsViewModel(await LoadUserInfo(id));
            return View(userDetailsViewModel);
        }

        public async Task<IActionResult> UserEdit(User user)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", user.ID);
            }

            var updatedUser = JsonSerializer.Serialize<User>(user);

            using (var requestContent = new StringContent(updatedUser, Encoding.UTF8, "application/json"))
            {
                using (var response = await _httpClient.PutAsync(RequestLinks.UsersResponseLink, requestContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                    }
                }
            }

            return RedirectToAction("Details", new { id = user.ID });
        }
    }
}
