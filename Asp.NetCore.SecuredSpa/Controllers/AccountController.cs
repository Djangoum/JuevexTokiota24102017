using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Asp.NetCore.SecuredSpa.Security;
using Vue2Spa.Controllers;

namespace Asp.NetCore.SecuredSpa.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AccountController(IConfiguration config, IJwtTokenGenerator tokenGenerator)
        {
            _config = config;
            _tokenGenerator = tokenGenerator;
        }   

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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authorize([FromBody]LoginModel loginModel)
        { 
            var tokenWithClaimsPrincipal =
               _tokenGenerator.GenerateAccessTokenIfIdentityConfirmed(
                                 loginModel.Username, loginModel.Password);

            if (!string.IsNullOrWhiteSpace(tokenWithClaimsPrincipal.AccessToken))
            {
                AuthenticationProperties authProps = new AuthenticationProperties();
                authProps.Items.Add(new KeyValuePair<string, string>("jwt",
                                     tokenWithClaimsPrincipal.AccessToken));

                await HttpContext.SignInAsync(
                             CookieAuthenticationDefaults.AuthenticationScheme,
                             tokenWithClaimsPrincipal.ClaimsPrincipal,
                             authProps);

                return new JsonResult(new { success = true });
            }
            else
            {
                return BadRequest();
            }
        }
        
        public JsonResult CheckLogin()
        {
            return new JsonResult(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
