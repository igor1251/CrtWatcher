using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.DatabaseOperators.Basic;
using X509Observer.Primitives.Basic;
using X509Observer.Reporters;

namespace X509ObserverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private ILogger<AuthenticationController> _logger;
        private IApiUsersRepository _apiUsersRepository;

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(ApiUser user)
        {
            return BadRequest("go away!");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(object request)
        {
            return BadRequest("go away!");
        }
    }
}
