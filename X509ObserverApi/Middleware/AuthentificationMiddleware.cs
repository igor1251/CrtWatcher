using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace X509ObserverApi.Middleware
{
    public class AuthentificationMiddleware
    {
        private readonly RequestDelegate _next;
        ILogger<AuthentificationMiddleware> _logger;

        public AuthentificationMiddleware(RequestDelegate next, ILogger<AuthentificationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation(context.Request.ToString());
        }
    }
}
