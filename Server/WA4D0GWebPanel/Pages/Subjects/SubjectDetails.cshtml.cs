using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Threading.Tasks;
using WA4D0GWebPanel.Models.Classes;
using WA4D0GWebPanel.Services.Interfaces;

namespace WA4D0GWebPanel.Pages.Subjects
{
    public class SubjectDetailsModel : PageModel
    {
        private readonly ILocalStore _dbStore;

        [BindProperty]
        public CertificateSubject Subject { get; private set; }

        public SubjectDetailsModel(ILocalStore dbStore)
        {
            _dbStore = dbStore;
        }

        public IActionResult OnGet(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return NotFound();
            }

            this.Subject = JsonSerializer.Deserialize<CertificateSubject>(json);

            if (this.Subject == null)
            {
                return NotFound();
            }

            return Page();
        }

        //public IActionResult OnPostDelete(int subjectID, int certificateID)
        //{
        //    this.Subject = new CertificateSubject();
        //    return Page();
        //}
    }
}
