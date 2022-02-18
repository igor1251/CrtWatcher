using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WA4D0GWebPanel.ViewModels;
using WA4D0GWebPanel.Models;
using System.Text;
using System;
using ElectronicDigitalSignatire.Models.Classes;
using ElectronicDigitalSignatire.Models.Interfaces;

namespace WA4D0GWebPanel.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private HttpClient _httpClient;

        public UsersController(ILogger<UsersController> logger,
                                  IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("SubjectsHttpClient");
        }

        #region Load methods

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

        public async Task<IActionResult> UsersList()
        {
            var users = await LoadInfo<List<User>>(RequestLinks.GetUsersFromDbLink);
            return View(new UsersListViewModel(users));
        }

        public async Task<IActionResult> LoadFromSystemStore()
        {
            var message = new HttpRequestMessage();
            message.RequestUri = new Uri(RequestLinks.GetUsersFromSystemStoreLink);
            message.Method = HttpMethod.Put;
            await _httpClient.SendAsync(message);

            return RedirectToAction("SubjectsList");
        }

        public async Task<IActionResult> UserDetails(int id)
        {
            var userDetailsViewModel = new UserDetailsViewModel();
            userDetailsViewModel.User = await LoadUserInfo(id);

            return View(userDetailsViewModel);
        }

        #endregion

        #region Edit methods

        [HttpPost]
        public async Task<IActionResult> UserEdit(User user)
        {
            if (!ModelState.IsValid)
            {
                var subjectDetailsViewModel = new UserDetailsViewModel();
                subjectDetailsViewModel.User = await LoadUserInfo(user.ID);
                return View("UserDetails", subjectDetailsViewModel);
            }

            var updatedSubject = JsonSerializer.Serialize<User>(user);

            using (var requestContent = new StringContent(updatedSubject, Encoding.UTF8, "application/json"))
            {
                using (var response = await _httpClient.PutAsync(RequestLinks.UsersResponseLink, requestContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                    }
                }
            }

            return RedirectToAction("UserDetails", new { id = user.ID });
        }

        #endregion

        #region Delete methods

        private async Task DeleteItem(string request)
        {
            using (var response = await _httpClient.DeleteAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> UserDelete(int id)
        {
            await DeleteItem(RequestLinks.UsersResponseLink + id);
            return RedirectToAction("UsersList");
        }

        [HttpPost]
        public async Task<IActionResult> CertificateDelete(int subjectID, int certificateID)
        {
            await DeleteItem(RequestLinks.CertificatesResponseLink + certificateID);
            return RedirectToAction("UserDetails", new { id = subjectID });
        }

        #endregion
    }
}
