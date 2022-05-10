using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetworkOperators.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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
        private IConfiguration _configuration;

        public SettingsController(ILogger<SettingsController> logger,
                                  PassportControl passportControl,
                                  HttpClient httpClient,
                                  IConfiguration configuration)
        {
            _logger = logger;
            _passportControl = passportControl;
            _httpClient = httpClient;
            _configuration = configuration;
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

                        using (FileStream fs = new FileStream(Environment.CurrentDirectory + "\\appsettings.json", 
                                                              FileMode.Open, 
                                                              FileAccess.ReadWrite, 
                                                              FileShare.None, 
                                                              8, 
                                                              FileOptions.WriteThrough))
                        {
                            var config = await JsonSerializer.DeserializeAsync<IDictionary<string, object>>(fs);
                            fs.Seek(0, SeekOrigin.Begin);
                            config.Add("ApiKey", model.ApiKey);
                            await JsonSerializer.SerializeAsync(fs, config, new JsonSerializerOptions()
                            { 
                                WriteIndented = true
                            });
                        }
                    }
                }
            }
            return View("Index", model);
        }
    }
}
