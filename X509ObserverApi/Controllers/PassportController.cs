﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetworkOperators.Identity.DataTransferObjects;
using NetworkOperators.Identity.Entities;
using NetworkOperators.Identity.MaintananceTools;
using NetworkOperators.Identity.Repositories;
using System;
using System.Threading.Tasks;
using Tools.Reporters;

namespace X509ObserverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassportController : ControllerBase
    {
        private readonly ILogger<PassportController> _logger;
        private readonly IUsersRepository _usersRepository;
        private readonly JwtTokensOperator _jwtTokenOperator;
        private readonly IConfiguration _configuration;

        public PassportController(ILogger<PassportController> logger, 
                                  IUsersRepository apiUsersRepository, 
                                  JwtTokensOperator jwtTokenOperator,
                                  IConfiguration configuration)
        {
            _logger = logger;
            _usersRepository = apiUsersRepository;
            _jwtTokenOperator = jwtTokenOperator;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserAuthorizationRequest request)
        {
            try
            {
                var newUser = new User()
                {
                    UserName = request.UserName,
                    PasswordHash = await SHA2HashOperator.Generate(request.Password),
                    Role = Role.USER
                };
                var isApiUserExists = await _usersRepository.IsUserExistsAsync(newUser);
                if (isApiUserExists)
                {
                    RedirectToAction("Login", request);
                }
                await _usersRepository.AddUserAsync(newUser);
                var createdUser = await _usersRepository.GetUserByAuthenticationDataAsync(newUser.UserName, newUser.PasswordHash);
                var apiKey = await _jwtTokenOperator.Generate(createdUser, int.Parse(_configuration["KeyValidityPeriod"]), _configuration["Secret"]);
                return Ok(new UserAuthorizationResponse() { Token = apiKey });
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
        public async Task<IActionResult> Login(UserAuthorizationRequest user)
        {
            try
            {
                var foundedUser = await _usersRepository.GetUserByAuthenticationDataAsync(user.UserName, await SHA2HashOperator.Generate(user.Password));
                if (foundedUser == null)
                {
                    return NotFound();
                }
                var apiKey = await _jwtTokenOperator.Generate(foundedUser, int.Parse(_configuration["KeyValidityPeriod"]), _configuration["Secret"]);
                return Ok(new UserAuthorizationResponse() { Token = apiKey });
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
