using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asp.NetCore.SecuredSpa.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IIdentityResolver identityResolver;
        private TokenOptions tokenOptions;

        public JwtTokenGenerator(IIdentityResolver identityResolver,
                     TokenOptions tokenOptions)
        {
            this.identityResolver = identityResolver ??
                throw new ArgumentNullException("Put a readable error message");
            this.tokenOptions = tokenOptions ??
                throw new ArgumentNullException("Put a readable error message");
        }

        public TokenWithClaimsPrincipal
                GenerateAccessTokenIfIdentityConfirmed(string userName,
                                                       string password)
        {
            string accessToken = string.Empty;
            List<Claim> claims = new List<Claim>();

            if (this.identityResolver.IsIdentityConfirmed(userName, password))
            {
                claims.Add(new Claim(ClaimTypes.Name, userName));
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti,
                                     Guid.NewGuid().ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Iat,
                                     DateTime.UtcNow.TimeOfDay.Ticks.ToString(),
                                     ClaimValueTypes.Integer64));

                var expiration =
                   TimeSpan.FromMinutes(this.tokenOptions.TokenExpiryInMinutes);

                var jwt = new JwtSecurityToken(issuer: this.tokenOptions.Issuer,
                                               audience: this.tokenOptions.Audience,
                                               claims: claims,
                                               notBefore: DateTime.UtcNow,
                                               expires:
                                                DateTime.UtcNow.Add(expiration),
                                               signingCredentials:
                               new SigningCredentials(this.tokenOptions.SigningKey,
                                                    SecurityAlgorithms.HmacSha256));

                accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            }

            return new TokenWithClaimsPrincipal()
            {
                AccessToken = accessToken,
                ClaimsPrincipal =
                             ClaimsPrincipalFactory.CreatePrincipal(claims)
            };
        }
    }
}
