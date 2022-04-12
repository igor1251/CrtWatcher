using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace X509Observer.Identity.Network.Middleware
{
    public class AuthentificationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthentificationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);
        }
    }
}
