using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Asp.NetCore.DualAuthenticationScheme.Controllers
{
    [Produces("application/json")]
    [Route("api/Values")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ValuesController : Controller
    {
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(new List<string>
            {
                "hola","adeu","holii"
            });
        }
    }
}