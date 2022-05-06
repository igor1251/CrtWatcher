using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetworkOperators.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X509ObserverAdmin.Models;

namespace X509ObserverAdmin.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var connectionParameters = await ConnectionParametersLoader.ReadServiceParameters();
            return View(new SettingsViewModel()
            {
                RemoteRegistrationServiceAddress = connectionParameters.RemoteRegistrationServiceAddress,
                RemoteAuthenticationServiceAddress = connectionParameters.RemoteAuthenticationServiceAddress,
                RemoteX509VaultStoreService = connectionParameters.RemoteX509VaultStoreService,
                RemoteServiceLogin = connectionParameters.RemoteServiceLogin,
                RemoteServicePassword = connectionParameters.RemoteServicePassword,
                ApiKey = connectionParameters.ApiKey
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(SettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                await ConnectionParametersLoader.WriteServiceParameters(new ConnectionParameters()
                {
                    RemoteRegistrationServiceAddress = model.RemoteRegistrationServiceAddress,
                    RemoteAuthenticationServiceAddress = model.RemoteAuthenticationServiceAddress,
                    RemoteX509VaultStoreService = model.RemoteX509VaultStoreService,
                    RemoteServiceLogin = model.RemoteServiceLogin,
                    RemoteServicePassword = model.RemoteServicePassword,
                    ApiKey = model.ApiKey
                });
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }
            return View("Index", model);
        }
    }
}
