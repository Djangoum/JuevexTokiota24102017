using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Asp.NetCore.DualAuthenticationScheme.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        public new IActionResult Unauthorized()
        {
            return View();
        }

        public async Task<IActionResult> Authorize()
        {
            const string Issuer = "Tokiota S.L";

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, "Andrew", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.Surname, "Lock", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.Country, "UK", ClaimValueTypes.String, Issuer),
                new Claim("ChildhoodHero", "Ronnie James Dio", ClaimValueTypes.String, Issuer)
            };

            await HttpContext.SignInAsync("Cookie", new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookie")),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.AddYears(80)
                });

            return RedirectToAction("Index", "Home");
        }
    }
}