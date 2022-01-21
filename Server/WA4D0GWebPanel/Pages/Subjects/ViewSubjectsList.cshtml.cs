using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using WA4D0GWebPanel.Models.Classes;
using WA4D0GWebPanel.Services.Interfaces;

namespace WA4D0GWebPanel.Pages.Subjects
{
    public class ViewSubjectsListModel : PageModel
    {
        private readonly ILocalStore _dbStore;

        public List<CertificateSubject> Subjects { get; set; }

        public ViewSubjectsListModel(ILocalStore dbStore)
        {
            _dbStore = dbStore;
        }

        public async void OnGetAsync()
        {
            Subjects = await _dbStore.LoadCertificateSubjectsAndCertificates();
        }
    }
}
