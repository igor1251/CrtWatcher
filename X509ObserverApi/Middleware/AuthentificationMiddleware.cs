using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetworkOperators.Identity.MaintananceTools;
using NetworkOperators.Identity.Repositories;
using Tools.Reporters;

namespace X509ObserverApi.Middleware
{
    public class AuthentificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthentificationMiddleware> _logger;
        private readonly IUsersRepository _userRepository;
        private readonly JwtTokensOperator _jwtTokensOperator;
        private readonly IConfiguration _configuration;

        public AuthentificationMiddleware(RequestDelegate next, 
                                          ILogger<AuthentificationMiddleware> logger,
                                          IUsersRepository usersRepository,
                                          JwtTokensOperator jwtTokensOperator,
                                          IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _userRepository = usersRepository;
            _jwtTokensOperator = jwtTokensOperator;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                    await MatchWithUser(context, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await ErrorReporter.MakeReport("Invoke(HttpContext context)", ex);
            }

            await _next.Invoke(context);
        }

        public async Task MatchWithUser(HttpContext context, string token)
        {
            try
            {
                var user = await _jwtTokensOperator.ValidateAsync(token, _configuration["Secret"]);
                if (user != null)
                    context.Items["User"] = await _userRepository.GetUserByAuthenticationDataAsync(user.UserName, user.PasswordHash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await ErrorReporter.MakeReport("MatchWithUser(HttpContext context, string token)", ex);
            }
        }
    }
}
