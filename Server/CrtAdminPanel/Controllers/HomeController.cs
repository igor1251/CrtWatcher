using CrtAdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CrtAdminPanel.Models.Classes;
using CrtAdminPanel.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace CrtAdminPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ICertificateLoader _loader;

        public HomeController(ILogger<HomeController> logger, ICertificateLoader loader)
        {
            _logger = logger;
            _loader = loader;
        }

        public IActionResult Index()
        {
            return View(_loader.ExtractCertificatesListAsync().GetAwaiter().GetResult());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
