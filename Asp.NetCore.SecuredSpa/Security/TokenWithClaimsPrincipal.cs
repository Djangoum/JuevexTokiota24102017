using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asp.NetCore.SecuredSpa.Security
{
    public class TokenWithClaimsPrincipal
    {
        public string AccessToken { get; internal set; }

        public ClaimsPrincipal ClaimsPrincipal { get; internal set; }
    }
}
