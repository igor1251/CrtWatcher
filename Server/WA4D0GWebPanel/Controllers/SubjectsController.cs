using ElectrnicDigitalSignatire.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WA4D0GWebPanel.Models;

namespace WA4D0GWebPanel.Controllers
{
    public class SubjectsController : Controller
    {
        ILocalStore _localStore;

        public SubjectsController(ILocalStore localStore)
        {
            _localStore = localStore;
        }

        [HttpGet]
        public async Task<IActionResult> SubjectsList()
        {
            return View(new SubjectsListViewModel(await _localStore.LoadCertificateSubjectsAndCertificates()));
        }

        [HttpGet]
        public IActionResult SubjectDetails()
        {
            return View();
        }
    }
}
