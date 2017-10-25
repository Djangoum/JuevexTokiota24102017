using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore.XssTestApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Reflect(string jsCode)
        {
            return View("Reflect", jsCode);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}