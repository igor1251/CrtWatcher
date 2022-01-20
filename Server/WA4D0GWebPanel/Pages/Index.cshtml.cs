using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WA4D0GWebPanel.Models;
using WA4D0GWebPanel.Services;

namespace WA4D0GWebPanel.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICertificateRepository _repository;

        public IEnumerable<Certificate> Certificates { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ICertificateRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public void OnGet()
        {
            Certificates = _repository.GetCertificatesList();
        }
    }
}
