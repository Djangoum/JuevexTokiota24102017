using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Asp.NetCore.JwtAuthentication.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ValuesController : Controller
    {
        public IActionResult Index()
        {
            return new JsonResult(new List<string>
            {
                "hola","adeu","holii"
            });
        }
    }
}