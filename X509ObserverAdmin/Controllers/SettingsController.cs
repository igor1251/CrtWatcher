using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetworkOperators.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using X509ObserverAdmin.Models;

namespace X509ObserverAdmin.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly PassportControl _passportControl;
        private ConnectionParameters _connectionParameters;
        private readonly HttpClient _httpClient;

        public SettingsController(ILogger<SettingsController> logger,
                                  PassportControl passportControl,
                                  HttpClient httpClient)
        {
            _logger = logger;
            _passportControl = passportControl;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            _connectionParameters = await ConnectionParametersLoader.ReadServiceParameters();
            return View(new SettingsViewModel()
            {
                RemoteRegistrationServiceAddress = _connectionParameters.RemoteRegistrationServiceAddress,
                RemoteAuthenticationServiceAddress = _connectionParameters.RemoteAuthenticationServiceAddress,
                RemoteX509VaultStoreService = _connectionParameters.RemoteX509VaultStoreService,
                RemoteServiceLogin = _connectionParameters.RemoteServiceLogin,
                RemoteServicePassword = _connectionParameters.RemoteServicePassword,
                ApiKey = _connectionParameters.ApiKey
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(SettingsViewModel model, string action)
        {
            if (ModelState.IsValid)
            {
                _connectionParameters = new ConnectionParameters()
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
                    _connectionParameters.ApiKey = await _passportControl.RegisterClient(_connectionParameters);
                    
                    if (string.IsNullOrEmpty(_connectionParameters.ApiKey))
                    {
                        ModelState.AddModelError("", "Некорректные регистрционные данные (логин, пароль или адреса сервисов)");
                    }
                    else
                    {
                        model.ApiKey = _connectionParameters.ApiKey;
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _connectionParameters.ApiKey);
                        await ConnectionParametersLoader.WriteServiceParameters(_connectionParameters);
                    }
                }
            }
            return View("Index", model);
        }
    }
}
