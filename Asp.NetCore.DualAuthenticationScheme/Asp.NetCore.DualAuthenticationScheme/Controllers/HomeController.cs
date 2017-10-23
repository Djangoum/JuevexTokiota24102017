using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore.DualAuthenticationScheme.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookie")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}