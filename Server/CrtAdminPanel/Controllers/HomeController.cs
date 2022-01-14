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

        public async Task<IActionResult> Index()
        {
            return View(await _certificateLoader.ExtractCertificatesListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> EditSettings()
        {
            return View(await _settingsLoader.LoadSettingsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> EditSettings(Settings settings)
        {
            await _settingsLoader.SaveSettingsAsync(settings);
            return RedirectToAction("EditSettings");
        }

        public async Task<IActionResult> EditCertificate(uint id)
        {
            return View(await _certificateTool.GetCertificateByIDAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> EditCertificate(Certificate certificate)
        {
            await _certificateTool.UpdateCertificateInDatabaseAsync(certificate);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("DeleteCertificate")]
        public async Task<ActionResult> ConfirmDelete(uint id)
        {
            return View(await _certificateTool.GetCertificateByIDAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCertificate(Certificate certificate)
        {
            await _certificateTool.DeleteCertificateFromDatabaseAsync(certificate);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
