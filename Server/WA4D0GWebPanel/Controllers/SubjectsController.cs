using ElectrnicDigitalSignatire.Models.Classes;
using ElectrnicDigitalSignatire.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WA4D0GWebPanel.Models;

namespace WA4D0GWebPanel.Controllers
{
    public class SubjectsController : Controller
    {
        IDbStore _store;
        ILocalStore _localStore;

        public SubjectsController(IDbStore store, ILocalStore localStore)
        {
            _store = store;
            _localStore = localStore;
        }

        #region Load methods

        [HttpGet]
        public async Task<IActionResult> SubjectsList()
        {
            var subjects = await _store.GetSubjects();
            return View(new SubjectsListViewModel(subjects));
        }

        [HttpGet]
        public async Task<IActionResult> LoadFromSystemStore()
        {
            var localStoreCertificates = await _localStore.LoadCertificateSubjectsAndCertificates();
            await _store.InsertSubject(localStoreCertificates);
            return RedirectToAction("SubjectsList");
        }

        [HttpGet]
        public async Task<IActionResult> SubjectDetails(int id)
        {
            var subjectDetailsViewModel = new SubjectDetailsViewModel();
            subjectDetailsViewModel.Subject = await _store.GetSubjectByID(id); ;
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
                subjectDetailsViewModel.Subject = await _store.GetSubjectByID(subject.ID); ;
                return View("SubjectDetails", subjectDetailsViewModel);
            }

            await _store.UpdateSubject(subject);
            return RedirectToAction("SubjectDetails", new { id = subject.ID });
        }

        #endregion

        #region Delete methods

        [HttpPost]
        public async Task<IActionResult> SubjectDelete(int id)
        {
            await _store.DeleteSubject(id);
            return RedirectToAction("SubjectsList");
        }

        [HttpPost]
        public async Task<IActionResult> CertificateDelete(int subjectID, int certificateID)
        {
            await _store.DeleteCertificate(certificateID);
            return RedirectToAction("SubjectDetails", new { id = subjectID });
        }

        #endregion
    }
}
