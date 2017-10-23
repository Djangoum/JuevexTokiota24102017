using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Asp.NetCore.CookieAuthentication.Controllers
{
    [Authorize]
    public class GreetingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}