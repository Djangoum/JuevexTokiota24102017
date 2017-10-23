using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Asp.NetCore.UltraBasicAuthentication.Middlewares
{
    internal class CustomAuthHandler : AuthenticationHandler<CustomAuthOptions>
    {
        public CustomAuthHandler(IOptionsMonitor<CustomAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }
        
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Get Authorization header value
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
            {
                return Task.FromResult(AuthenticateResult.Fail("Cannot read authorization header."));
            }

            if(authorization.ToArray()[0].StartsWith("Basic"))
            {
                try
                {
                    var base64AuthorizationString = authorization.ToArray()[0].Split(' ');

                    GetCredentials(base64AuthorizationString[1], out var username, out var password);

                    if (!(username == "ariel" && password == "123123"))
                    {
                        return Task.FromResult(AuthenticateResult.Fail("Invalid auth key."));
                    }

                    // Create authenticated user
                    var identities = new List<ClaimsIdentity> { new ClaimsIdentity("custom auth type") };
                    var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Options.Scheme);

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
                catch
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid auth key."));
                }               
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid auth key."));
            }
        }

        private void GetCredentials (string base64String, out string username, out string password)
        {
            byte[] textAsBytes = Convert.FromBase64String(base64String);
            var decodedText = Encoding.UTF8.GetString(textAsBytes);

            var array = decodedText.Split(':');

            username = array[0];
            password = array[1];
        }
    }
}