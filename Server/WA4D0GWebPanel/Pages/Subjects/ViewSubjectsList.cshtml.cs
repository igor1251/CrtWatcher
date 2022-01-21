using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using WA4D0GWebPanel.Models;
using WA4D0GWebPanel.Services;

namespace WA4D0GWebPanel.Pages.Subjects
{
    public class ViewSubjectsListModel : PageModel
    {
        private readonly ICertificateRepository _repository;

        public IEnumerable<Subject> Subjects { get; set; }

        public ViewSubjectsListModel(ICertificateRepository repository)
        {
            _repository = repository;
        }

        public async void OnGetAsync()
        {
            Subjects = await _repository.GetSubjectsList();
        }
    }
}
