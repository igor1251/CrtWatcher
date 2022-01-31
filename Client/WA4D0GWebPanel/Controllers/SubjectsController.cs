using ElectrnicDigitalSignatire.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WA4D0GWebPanel.Models;

namespace WA4D0GWebPanel.Controllers
{
    public class SubjectsController : Controller
    {
        #region Load methods

        [HttpGet]
        public async Task<IActionResult> SubjectsList()
        {
            var subjects = new List<CertificateSubject>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:5001/subjects"))
                {
                    string apiResource = await response.Content.ReadAsStringAsync();
                    subjects = JsonSerializer.Deserialize<List<CertificateSubject>>(apiResource);
                }
            }
            return View(new SubjectsListViewModel(subjects));
            /*
            var subjects = await _store.GetSubjects();
            return View(new SubjectsListViewModel(subjects));
            */
        }

        [HttpGet]
        public async Task<IActionResult> LoadFromSystemStore()
        {
            //var localStoreCertificates = await _localStore.LoadCertificateSubjectsAndCertificates();
            //await _store.InsertSubject(localStoreCertificates);
            return RedirectToAction("SubjectsList");
        }

        [HttpGet]
        public async Task<IActionResult> SubjectDetails(int id)
        {
            var subjectDetailsViewModel = new SubjectDetailsViewModel();
            //subjectDetailsViewModel.Subject = await _store.GetSubjectByID(id);
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
                //subjectDetailsViewModel.Subject = await _store.GetSubjectByID(subject.ID); ;
                return View("SubjectDetails", subjectDetailsViewModel);
            }

            //await _store.UpdateSubject(subject);
            return RedirectToAction("SubjectDetails", new { id = subject.ID });
        }

        #endregion

        #region Delete methods

        [HttpPost]
        public async Task<IActionResult> SubjectDelete(int id)
        {
            //await _store.DeleteSubject(id);
            return RedirectToAction("SubjectsList");
        }

        [HttpPost]
        public async Task<IActionResult> CertificateDelete(int subjectID, int certificateID)
        {
            //await _store.DeleteCertificate(certificateID);
            return RedirectToAction("SubjectDetails", new { id = subjectID });
        }

        #endregion
    }
}
