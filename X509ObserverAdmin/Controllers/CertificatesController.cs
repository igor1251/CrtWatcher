using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X509ObserverAdmin.Models;

namespace X509ObserverAdmin.Controllers
{
    [Authorize]
    public class CertificatesController : Controller
    {
        private readonly ILogger<CertificatesController> _logger;

        public CertificatesController(ILogger<CertificatesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new CertificatesViewModel());
        }
    }
}
