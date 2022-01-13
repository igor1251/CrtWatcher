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
        private ICertificateLoader _certificateLoader;
        private ICertificateTool _certificateTool;
        private ISettingsLoader _settingsLoader;

        public HomeController(ILogger<HomeController> logger, 
                              ICertificateLoader certificateLoader,
                              ICertificateTool certificateTool,
                              ISettingsLoader settingsLoader)
        {
            _logger = logger;
            _certificateLoader = certificateLoader;
            _certificateTool = certificateTool;
            _settingsLoader = settingsLoader;
        }

        public IActionResult Index()
        {
            return View(_certificateLoader.ExtractCertificatesListAsync().GetAwaiter().GetResult());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult EditSettings()
        {
            return View(_settingsLoader.LoadSettingsAsync().GetAwaiter().GetResult());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
