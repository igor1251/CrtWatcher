using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetworkOperators.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using X509KeysVault.Entities;
using X509ObserverAdmin.Models;

namespace X509ObserverAdmin.Controllers
{
    [Authorize]
    public class CertificatesController : Controller
    {
        private readonly ILogger<CertificatesController> _logger;
        private readonly HttpClient _httpClient;

        public CertificatesController(ILogger<CertificatesController> logger,
                                      HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            //да-да, это костыль, я знаю, потом поправлю, когда придумаю что-то получше
            var connectionParameters = await ConnectionParametersLoader.ReadServiceParameters();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", connectionParameters.ApiKey);

            using (var response = await _httpClient.GetAsync(connectionParameters.RemoteX509VaultStoreService + "db"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var subjects = await JsonSerializer.DeserializeAsync<List<Subject>>(await response.Content.ReadAsStreamAsync());
                    return View(new CertificatesViewModel()
                    {
                        Subjects = subjects
                    });
                }
                return Content(response.ReasonPhrase);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            return Content(id.ToString());
        }
    }
}
