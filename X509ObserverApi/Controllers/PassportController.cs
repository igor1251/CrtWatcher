using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.Identity.Basic;
using X509Observer.Identity.Database;
using X509Observer.Reporters;

namespace X509ObserverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassportController : ControllerBase
    {
        private readonly ILogger<PassportController> _logger;
        private readonly IApiUsersRepository _apiUsersRepository;

        public PassportController(ILogger<PassportController> logger, IApiUsersRepository apiUsersRepository)
        {
            _logger = logger;
            _apiUsersRepository = apiUsersRepository;
        }

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
