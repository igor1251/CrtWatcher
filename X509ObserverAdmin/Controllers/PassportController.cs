using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X509ObserverAdmin.Models;

namespace X509ObserverAdmin.Controllers
{
    public class PassportController : Controller
    {
        private readonly ILogger<PassportController> _logger;

        public PassportController(ILogger<PassportController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}