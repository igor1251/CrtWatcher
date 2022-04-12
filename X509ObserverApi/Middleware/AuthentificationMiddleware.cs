using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace X509ObserverApi.Middleware
{
    public class AuthentificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthentificationMiddleware> _logger;

        public AuthentificationMiddleware(RequestDelegate next, ILogger<AuthentificationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Auth analyzer run...");
            var authorizationTokens = context.Request.Headers["Authorization"];
            foreach (var item in authorizationTokens)
            {
                _logger.LogInformation(item);
            }
            
            await _next.Invoke(context);
        }
    }
}
