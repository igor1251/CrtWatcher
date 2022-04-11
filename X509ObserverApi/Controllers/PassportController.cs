﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.DatabaseOperators.Basic;
using X509Observer.Primitives.Network;
using X509Observer.Reporters;

namespace X509ObserverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassportController : ControllerBase
    {
        private ILogger<PassportController> _logger;
        private IApiUsersRepository _apiUsersRepository;

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(ApiUser user)
        {
            try
            {
                await _apiUsersRepository.AddApiUserAsync(user);
                var apiKey = "test-api-key";
                return Ok(apiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await ErrorReporter.MakeReport("Register(ApiUser user)", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(object request)
        {
            try
            {
                var apiKey = "test-api-key";
                return Ok(apiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await ErrorReporter.MakeReport("Login(object request)", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
