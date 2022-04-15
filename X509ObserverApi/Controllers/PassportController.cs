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
        private readonly IConfiguration _configuration;

        public PassportController(ILogger<PassportController> logger, 
                                  IApiUsersRepository apiUsersRepository, 
                                  JwtTokensOperator jwtTokenOperator,
                                  IConfiguration configuration)
        {
            _logger = logger;
            _apiUsersRepository = apiUsersRepository;
            _jwtTokenOperator = jwtTokenOperator;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(ApiUser user)
        {
            try
            {
                var isApiUserExists = await _apiUsersRepository.IsApiUserExistsAsync(user);
                if (isApiUserExists)
                {
                    RedirectToAction("Login", user);
                }
                user.Role = ApiRole.USER;
                await _apiUsersRepository.AddApiUserAsync(user);
                var createdUser = await _apiUsersRepository.GetApiUserByUserNameAsync(user.UserName);
                var apiKey = _jwtTokenOperator.Generate(createdUser, int.Parse(_configuration["KeyValidityPeriod"]), _configuration["Secret"]);
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
                var isApiUserExists = await _apiUsersRepository.IsApiUserExistsAsync(user);
                if (!isApiUserExists)
                {
                    return NotFound("Incorrect login-password pair");
                }
                var apiKey = _jwtTokenOperator.Generate(user, int.Parse(_configuration["KeyValidityPeriod"]), _configuration["Secret"]);
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
