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
        private Role _role;

        public AuthorizationAttribute(Role role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                //var authorizationResult = new JsonResult("Invalid credentials");
                //authorizationResult.StatusCode = StatusCodes.Status401Unauthorized;
                //context.Result = authorizationResult;
                context.Result = new JsonResult("Invalid credentials") { StatusCode = StatusCodes.Status401Unauthorized };
            }
            //else if (user.Role != _role)
            //{
            //    var authorizationResult = new JsonResult("Insufficient rights to access the resource");
            //    authorizationResult.StatusCode = StatusCodes.Status401Unauthorized;
            //    context.Result = authorizationResult;
            //}
            else if ((user.Permissions & (ushort)_role) == 0)
            {
                context.Result = new JsonResult("Insufficient rights to access the resource") { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
