using Microsoft.IdentityModel.Tokens;
using System;

namespace Asp.NetCore.SecuredSpa.Security
{
    public class TokenOptions
    {
        public TokenOptions(string issuer,
                            string audience,
                            SecurityKey signingKey,
                            int tokenExpiryInMinutes = 5)
        {
            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new ArgumentNullException("Suitable error message");
            }

            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new ArgumentNullException("Suitable error message");
            }

            this.Audience = audience;
            this.Issuer = issuer;
            this.SigningKey = signingKey ?? throw new
                          ArgumentNullException("Suitable error message");
            this.TokenExpiryInMinutes = tokenExpiryInMinutes;
        }

        public SecurityKey SigningKey { get; private set; }

        public string Issuer { get; private set; }

        public string Audience { get; private set; }

        public int TokenExpiryInMinutes { get; private set; }
    }
}
