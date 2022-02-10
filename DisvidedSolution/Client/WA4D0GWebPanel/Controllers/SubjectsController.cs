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
    public class SubjectsController : Controller
    {
        private readonly ILogger<SubjectsController> _logger;
        private HttpClient _httpClient;

        public SubjectsController(ILogger<SubjectsController> logger,
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

        private async Task<CertificateSubject> LoadSubjectInfo(int id)
        {
            return await LoadInfo<CertificateSubject>(RequestLinks.SubjectsResponseLink + id); ;
        }

        public async Task<IActionResult> SubjectsList()
        {
            var subjects = await LoadInfo<List<CertificateSubject>>(RequestLinks.GetSubjectsFromDbLink);
            return View(new SubjectsListViewModel(subjects));
        }

        public async Task<IActionResult> LoadFromSystemStore()
        {
            var message = new HttpRequestMessage();
            message.RequestUri = new Uri(RequestLinks.GetSubjectsFromSystemStoreLink);
            message.Method = HttpMethod.Put;
            await _httpClient.SendAsync(message);

            return RedirectToAction("SubjectsList");
        }

        public async Task<IActionResult> SubjectDetails(int id)
        {
            var subjectDetailsViewModel = new SubjectDetailsViewModel();
            subjectDetailsViewModel.Subject = await LoadSubjectInfo(id);

            return View(subjectDetailsViewModel);
        }

        #endregion

        #region Edit methods

        [HttpPost]
        public async Task<IActionResult> SubjectEdit(CertificateSubject subject)
        {
            if (!ModelState.IsValid)
            {
                var subjectDetailsViewModel = new SubjectDetailsViewModel();
                subjectDetailsViewModel.Subject = await LoadSubjectInfo(subject.ID);
                return View("SubjectDetails", subjectDetailsViewModel);
            }

            var updatedSubject = JsonSerializer.Serialize<CertificateSubject>(subject);

            using (var requestContent = new StringContent(updatedSubject, Encoding.UTF8, "application/json"))
            {
                using (var response = await _httpClient.PutAsync(RequestLinks.SubjectsResponseLink, requestContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                    }
                }
            }

            return RedirectToAction("SubjectDetails", new { id = subject.ID });
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
        public async Task<IActionResult> SubjectDelete(int id)
        {
            await DeleteItem(RequestLinks.SubjectsResponseLink + id);
            return RedirectToAction("SubjectsList");
        }

        [HttpPost]
        public async Task<IActionResult> CertificateDelete(int subjectID, int certificateID)
        {
            await DeleteItem(RequestLinks.CertificatesResponseLink + certificateID);
            return RedirectToAction("SubjectDetails", new { id = subjectID });
        }

        #endregion
    }
}
