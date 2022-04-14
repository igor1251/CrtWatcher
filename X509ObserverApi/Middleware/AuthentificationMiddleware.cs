using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using X509Observer.Identity.MaintenanceTools;
using X509Observer.Identity.Repositories;
using X509Observer.Reporters;

namespace X509ObserverApi.Middleware
{
    public class AuthentificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthentificationMiddleware> _logger;
        private readonly IApiUsersRepository _apiUserRepository;
        private readonly JwtTokensOperator _jwtTokensOperator;
        private readonly IConfiguration _configuration;

        public AuthentificationMiddleware(RequestDelegate next, 
                                          ILogger<AuthentificationMiddleware> logger,
                                          IApiUsersRepository apiUsersRepository,
                                          JwtTokensOperator jwtTokensOperator,
                                          IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _apiUserRepository = apiUsersRepository;
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
                var apiUser = await _jwtTokensOperator.ValidateAsync(token, _configuration["Secret"]);
                context.Items["User"] = await _apiUserRepository.GetApiUserByUserNameAsync(apiUser.UserName);
                //context.Items["User"] = await _jwtTokensOperator.ValidateAsync(token, _configuration["Secret"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await ErrorReporter.MakeReport("MatchWithUser(HttpContext context, string token)", ex);
            }
        }
    }
}
