using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WA4D0GWebPanel.Models;
using WA4D0GWebPanel.Services;

namespace WA4D0GWebPanel.Pages.Subjects
{
    public class SubjectDetailsModel : PageModel
    {
        private readonly ICertificateRepository _repository;

        [BindProperty]
        public Subject Subject { get; set; }

        public SubjectDetailsModel(ICertificateRepository repository)
        {
            _repository = repository;
        }

        public void OnGet(string json)
        {
            this.Subject = JsonSerializer.Deserialize<Subject>(json);
        }
    }
}
