using Asp.NetCore.SecuredSpa.Middlewares;
using Asp.NetCore.SecuredSpa.Security;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Vue2Spa
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Environment = env;

            configuration = builder.Build();
        }

        public IHostingEnvironment Environment;
        public IConfigurationRoot configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // namespace
            SecurityKey signingKey = new SymmetricSecurityKey(
                                          Encoding.ASCII.GetBytes(
                                            configuration["Token:SigningKey"]));

            services.AddDataProtection(options =>
                options.ApplicationDiscriminator = $"{Environment.ApplicationName}")
                       .SetApplicationName($"{Environment.ApplicationName}");

            services.AddScoped<IDataSerializer<AuthenticationTicket>,
                    TicketSerializer>();

            // inject the token generator, identity resolver and the token options
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>((serviceProvider) =>
                new JwtTokenGenerator(new InMemoryIdentityResolver(),
                new TokenOptions(configuration["Token:Issuer"],
                 configuration["Token:Audience"], signingKey)));


            TokenValidationParameters tokenValidationParameters =
                new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    // these are the bare minimum you should validate in a JWT for it to be
                    // <span                 data-mce-type="bookmark"                id="mce_SELREST_start"              data-mce-style="overflow:hidden;line-height:0"              style="overflow:hidden;line-height:0"           >﻿</span>a meaningful authentication tool
                    ValidateAudience = true,
                    ValidAudience = configuration["Token:Audience"],

                    ValidateIssuer = true,
                    ValidIssuer = configuration["Token:Issuer"],

                    IssuerSigningKey = signingKey,
                    ValidateIssuerSigningKey = true,

                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };

            var serialiser = services.BuildServiceProvider().GetService<IDataSerializer<AuthenticationTicket>>(); ;

            var dataProtector = services.BuildServiceProvider()
                    .GetDataProtector(new string[] { $"{Environment.ApplicationName}-Auth" });

            services.AddAuthentication(options =>
                {
                    // these must be set other ASP.NET Core will throw exception that no
                    // default authentication scheme or default challenge scheme is set.
                    options.DefaultAuthenticateScheme =
                            CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                            CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Expiration = TimeSpan.FromMinutes(5);
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    options.Cookie.HttpOnly = true; // This is set by default
                    //options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                    options.TicketDataFormat = new
                                            JwtTokenValidator(SecurityAlgorithms.HmacSha256,
                                                              tokenValidationParameters,
                                                              serialiser,
                                                              dataProtector);
                    options.Events.OnRedirectToLogin = (context) =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                });

            services.AddSingleton(configuration);

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            // Add framework services.
            services.AddMvc(options => {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                //options.Filters.Add(new RequireHttpsAttribute());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IAntiforgery antiforgery)
        {
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                await next();
            });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                await next();
            });

            app.UseAntiForgeryTokenProvider(antiforgery);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
