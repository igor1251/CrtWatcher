using ElectrnicDigitalSignatire.Models.Classes;
using ElectrnicDigitalSignatire.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<IActionResult> SubjectsList()
        {
            var subjects = await _store.GetSubjects();
            return View(new SubjectsListViewModel(subjects));
        }

        [HttpGet]
        public async Task<IActionResult> LoadFromSystemStore()
        {
            await _store.InsertSubject(await _localStore.LoadCertificateSubjectsAndCertificates());
            return RedirectToAction("SubjectsList");
        }

        [HttpGet]
        public async Task<IActionResult> SubjectDetails(int id)
        {
            var subject = await _store.GetSubjectByID(id);
            return View(new SubjectDetailsViewModel(subject));
        }

        [HttpDelete]
        public async Task<IActionResult> SubjectDelete(int id)
        {
            await _store.DeleteSubject(id);
            return RedirectToAction("SubjectsList");
        }

        [HttpDelete]
        public async Task<IActionResult> CertificateDelete(int subjectID, int certificateID)
        {
            await _store.DeleteCertificate(certificateID);
            return RedirectToAction("SubjectDetails", subjectID);
        }
    }
}
