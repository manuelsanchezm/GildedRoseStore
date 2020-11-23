using GildedRoseStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GildedRoseStore.Controllers
{
    public class AccountController : Controller
    {
        private const string CONFIG_SECTION = "AppUser";

        private readonly IConfiguration _configuration;
        public InputModel Input { get; set; }

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string loginname, string password, bool rememberme)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = AuthenticateUser(loginname, password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            // create cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.LoginName)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties
            {
                IsPersistent = rememberme,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1)
            });

            return LocalRedirect("/Home/Index");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

            return LocalRedirect("/Home/Index");
        }

        private ApplicationUser AuthenticateUser(string loginname, string password)
        {
            if (string.IsNullOrEmpty(loginname) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _configuration.GetSection(CONFIG_SECTION).Get<InputModel>();

            var passwordHasher = new PasswordHasher<string>();
            if (loginname != user.LoginName ||
                passwordHasher.VerifyHashedPassword(null, user.Password, password) != PasswordVerificationResult.Success)
            {
                return null;
            }

            return new ApplicationUser { LoginName = loginname };
        }
    }
}
