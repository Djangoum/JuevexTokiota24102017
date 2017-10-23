using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asp.NetCore.UltraBasicAuthentication.Controllers
{
    [Authorize]
    public class ValuesController : Controller
    {
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await Task.FromResult(new List<string>()
            {
                "hola",
                "adios",
                "gatito",
                "mierda"
            }));
        }
    }
}