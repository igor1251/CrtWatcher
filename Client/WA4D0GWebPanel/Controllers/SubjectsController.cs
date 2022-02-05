using ElectrnicDigitalSignatire.Models.Classes;
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

namespace WA4D0GWebPanel.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ILogger<SubjectsController> _logger;
        private HttpClient _httpClient;

        public SubjectsController(ILogger<SubjectsController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        #region Load methods

        private async Task<CertificateSubject> LoadSubjectInfo(int id)
        {
            var subject = new CertificateSubject();

            using (var response = await _httpClient.GetAsync(RequestLinks.SubjectsResponseLink + id))
            {
                subject = JsonSerializer.Deserialize<CertificateSubject>(await response.Content.ReadAsStringAsync());
            }

            return subject;
        }

        public async Task<IActionResult> SubjectsList()
        {
            var subjects = new List<CertificateSubject>();

            using (var response = await _httpClient.GetAsync(RequestLinks.GetSubjectsFromDbLink))
            {
                subjects = JsonSerializer.Deserialize<List<CertificateSubject>>(await response.Content.ReadAsStringAsync());
            }
        
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
                    response.EnsureSuccessStatusCode();
                }
            }

            return RedirectToAction("SubjectDetails", new { id = subject.ID });
        }

        #endregion

        #region Delete methods

        [HttpPost]
        public async Task<IActionResult> SubjectDelete(int id)
        {
            using (var response = await _httpClient.DeleteAsync(RequestLinks.SubjectsResponseLink + id))
            {
                response.EnsureSuccessStatusCode();
            }

            return RedirectToAction("SubjectsList");
        }

        [HttpPost]
        public async Task<IActionResult> CertificateDelete(int subjectID, int certificateID)
        {
            using (var response = await _httpClient.DeleteAsync(RequestLinks.CertificatesResponseLink + certificateID))
            {
                response.EnsureSuccessStatusCode();
            }

            return RedirectToAction("SubjectDetails", new { id = subjectID });
        }

        #endregion
    }
}
