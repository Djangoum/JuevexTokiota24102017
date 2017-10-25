using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asp.NetCore.SecuredSpa.Security
{
    public class JwtTokenValidator : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string algorithm;
        private readonly TokenValidationParameters validationParameters;
        private readonly IDataSerializer<AuthenticationTicket>
                                           ticketSerializer;
        private readonly IDataProtector dataProtector;

        public JwtTokenValidator(string algorithm,
                                 TokenValidationParameters validationParameters,
                                 IDataSerializer<AuthenticationTicket>
                                          ticketSerializer,
                                 IDataProtector dataProtector)
        {
            this.algorithm = algorithm;
            this.validationParameters = validationParameters;
            this.ticketSerializer = ticketSerializer;
            this.dataProtector = dataProtector;
        }

        public AuthenticationTicket Unprotect(string protectedText)
            => Unprotect(protectedText, null);

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var handler = new JwtSecurityTokenHandler();
            AuthenticationTicket authTicket = null;

            authTicket = this.ticketSerializer.Deserialize(
                                this.dataProtector.Unprotect(
                                    Base64UrlTextEncoder.Decode(protectedText)));

            if (authTicket.Properties != null &&
                authTicket.Properties.Items.Any())
            {
                if (authTicket.Properties.Items.TryGetValue("jwt", out string
                        embeddedJwt))
                {
                    ClaimsPrincipal principal =
                                    handler.ValidateToken(embeddedJwt,
                                    this.validationParameters,
                                    out SecurityToken validToken);

                    var validJwt = validToken as JwtSecurityToken;

                    if (validJwt == null)
                    {
                        throw new ArgumentException("Invalid JWT");
                    }

                    if (!validJwt.Header.Alg.Equals(algorithm,
                            StringComparison.Ordinal))
                    {
                        throw new ArgumentException($"Algorithm must be '{algorithm}'");
                    }
                }
                else
                {
                    throw new ArgumentException("No JWT was found in the Authentication Ticket");
                }
            }

            return authTicket;
        }

        public string Protect(AuthenticationTicket data) =>
                           this.Protect(data, null);

        public string Protect(AuthenticationTicket data, string purpose)
        {
            byte[] array = this.ticketSerializer.Serialize(data);
            IDataProtector dataProtector = this.dataProtector;

            if (!string.IsNullOrEmpty(purpose))
            {
                dataProtector = dataProtector.CreateProtector(purpose);
            }

            return Base64UrlTextEncoder.Encode(dataProtector.Protect(array));
        }
    }
}
