using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Antiforgery;

namespace Asp.NetCore.SecuredSpa.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AntiForgeryTokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryTokenProviderMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string path = httpContext.Request.Path.Value;
            if (path != null && !path.ToLower().Contains("/api"))
            {
                // XSRF-TOKEN used by angular in the $http if provided
                var tokens = _antiforgery.GetAndStoreTokens(httpContext);
                httpContext.Response.Cookies.Append("XSRF-TOKEN",
                  tokens.RequestToken, new CookieOptions
                  {
                      HttpOnly = false
                  }
                );
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AntiForgeryTokenProviderMiddlewareExtensions
    {
        public static IApplicationBuilder UseAntiForgeryTokenProvider(this IApplicationBuilder builder, IAntiforgery antiforgery)
        {
            return builder.UseMiddleware<AntiForgeryTokenProviderMiddleware>(antiforgery);
        }
    }
}
