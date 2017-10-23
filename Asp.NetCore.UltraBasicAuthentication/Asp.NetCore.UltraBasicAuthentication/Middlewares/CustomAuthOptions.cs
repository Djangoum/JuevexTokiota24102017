using Microsoft.AspNetCore.Authentication;

namespace Asp.NetCore.UltraBasicAuthentication
{
    public class CustomAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Basic";
        public string Scheme => DefaultScheme;
        public string AuthKey { get; set; }
    }
}