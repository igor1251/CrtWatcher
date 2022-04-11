using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using X509Observer.DatabaseOperators.Network;
using X509Observer.Primitives.Database;
using X509Observer.Primitives.Network;

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
            //Пока что просто тест
            var apiUser = new ApiUser()
            {
                UserName = "babichew.i@yandex.ru",
                PasswordHash = Convert.ToHexString(MD5.HashData(Encoding.ASCII.GetBytes("password")))
            };
            ApiKeysRepository apiKeysRepo = new ApiKeysRepository(new DbContext());
            var apiKey = await apiKeysRepo.GenerateApiKeyAsync(apiUser);
            _logger.LogInformation("\n" + apiKey.Value + "\n" + apiKey.ExpirationTime.ToString());

            await _next.Invoke(context);
        }
    }
}
