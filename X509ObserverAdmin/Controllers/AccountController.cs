using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetworkOperators.Identity.MaintananceTools;
using NetworkOperators.Identity.Repositories;
using NetworkOperators.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X509ObserverAdmin.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace X509ObserverAdmin.Controllers
{
    public class AccountController : Controller
    {
        private ILogger<AccountController> _logger;
        private readonly IUsersRepository _usersRepository;

        public AccountController(ILogger<AccountController> logger,
                                 IUsersRepository usersRepository)
        {
            _logger = logger;
            _usersRepository = usersRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var tmpPasswordHash = await SHA2HashOperator.Generate(model.Password);
                var user = await _usersRepository.GetUserByAuthenticationDataAsync(model.UserName, tmpPasswordHash);
                if (user != null)
                {
                    await Authenticate(model.UserName);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var tmpPasswordHash = await SHA2HashOperator.Generate(model.Password);
                var user = await _usersRepository.GetUserByAuthenticationDataAsync(model.UserName, tmpPasswordHash);
                if (user == null)
                {
                    await _usersRepository.AddUserAsync(new User
                    {
                        UserName = model.UserName,
                        PasswordHash = tmpPasswordHash,
                        Permissions = (ushort)Role.User
                    });
                    await Authenticate(model.UserName);
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Имя пользователя уже занято");
                }
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
