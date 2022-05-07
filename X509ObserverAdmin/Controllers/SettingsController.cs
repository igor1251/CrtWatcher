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
        private readonly PassportControl _passportControl;

        public SettingsController(ILogger<SettingsController> logger,
                                  PassportControl passportControl)
        {
            _logger = logger;
            _passportControl = passportControl;
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
        public async Task<IActionResult> Apply(SettingsViewModel model, string action)
        {
            if (ModelState.IsValid)
            {
                var connectionParameters = new ConnectionParameters()
                {
                    RemoteRegistrationServiceAddress = model.RemoteRegistrationServiceAddress,
                    RemoteAuthenticationServiceAddress = model.RemoteAuthenticationServiceAddress,
                    RemoteX509VaultStoreService = model.RemoteX509VaultStoreService,
                    RemoteServiceLogin = model.RemoteServiceLogin,
                    RemoteServicePassword = model.RemoteServicePassword,
                    ApiKey = model.ApiKey
                };
                
                if (action == "SaveAndConnect")
                {
                    connectionParameters.ApiKey = await _passportControl.RegisterClient(connectionParameters);
                    model.ApiKey = connectionParameters.ApiKey;
                    if (string.IsNullOrEmpty(connectionParameters.ApiKey))
                    {
                        ModelState.AddModelError("", "Некорректные регистрционные данные (логин, пароль или адреса сервисов)");
                    }
                }

                await ConnectionParametersLoader.WriteServiceParameters(connectionParameters);
            }
            return View("Index", model);
        }
    }
}
