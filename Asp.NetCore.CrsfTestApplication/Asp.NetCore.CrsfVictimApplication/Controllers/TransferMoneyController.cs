using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Asp.NetCore.CrsfVictimApplication.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookie")]
    public class TransferMoneyController : Controller
    {
        [HttpPost]
        public IActionResult TransferMoney(string FromAccount,string ToAccount, decimal Amount)
        {
            return new JsonResult("Money Transfered Successfully");
        }
    }
}