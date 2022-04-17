using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetworkOperators.Identity.Entities;

namespace X509ObserverApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private string _role;

        public AuthorizationAttribute(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                var authorizationResult = new JsonResult("Invalid credentials");
                authorizationResult.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = authorizationResult;
            }
            else if (user.Role != _role)
            {
                var authorizationResult = new JsonResult("Insufficient rights to access the resource");
                authorizationResult.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = authorizationResult;
            }
        }
    }
}
