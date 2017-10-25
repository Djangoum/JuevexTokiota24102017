using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCore.SecuredSpa.Security
{
    public interface IJwtTokenGenerator
    {
        TokenWithClaimsPrincipal GenerateAccessTokenIfIdentityConfirmed(string userName, string password);
    }
}
