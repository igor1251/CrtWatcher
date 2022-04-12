using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.Identity.Entities;
using X509Observer.Identity.Repositories;
using X509Observer.Reporters;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace X509ObserverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassportController : ControllerBase
    {
        private readonly ILogger<PassportController> _logger;
        private readonly IApiUsersRepository _apiUsersRepository;
        private readonly IConfiguration _configuration;

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
                user.Role = ApiRole.User;
                await _apiUsersRepository.AddApiUserAsync(user);
                var token = new JwtSecurityTokenHandler();
                var descriptor = new SecurityTokenDescriptor();
                descriptor.Subject = new System.Security.Claims.ClaimsIdentity()
                var apiKey = "";
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
