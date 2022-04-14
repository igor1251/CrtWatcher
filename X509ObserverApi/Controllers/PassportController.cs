using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using X509Observer.Identity.Entities;
using X509Observer.Identity.MaintenanceTools;
using X509Observer.Identity.Repositories;
using X509Observer.Reporters;

namespace X509ObserverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassportController : ControllerBase
    {
        private readonly ILogger<PassportController> _logger;
        private readonly IApiUsersRepository _apiUsersRepository;
        private readonly JwtTokensOperator _jwtTokenOperator;

        public PassportController(ILogger<PassportController> logger, IApiUsersRepository apiUsersRepository, JwtTokensOperator jwtTokenOperator)
        {
            _logger = logger;
            _apiUsersRepository = apiUsersRepository;
            _jwtTokenOperator = jwtTokenOperator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(ApiUser user)
        {
            try
            {
                user.Role = ApiRole.User;
                await _apiUsersRepository.AddApiUserAsync(user);
                var apiKey = _jwtTokenOperator.Generate(user, 365, "secret");
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
        public async Task<IActionResult> Login(ApiUser user)
        {
            try
            {
                //логика авторизации. вообще лучше запилить сервис авторизации
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
