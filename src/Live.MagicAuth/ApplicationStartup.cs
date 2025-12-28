using Live.MagicAuth.Application.Infrastructure;
using Live.MagicAuth.Application.Migrations;
using Live.MagicAuth.Assertion;
using Live.MagicAuth.Attestation;
using Live.MagicAuth.Domain.Infrastructure.Data;
using Live.MagicAuth.Migrations;
using Live.MagicAuth.SignOut;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using System;
using System.Collections.Generic;

namespace Live.MagicAuth
{
    /// <summary>
    /// Represents the application startup class
    /// </summary>
    public class ApplicationStartup
    {
        #region Constructor

        public ApplicationStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the application services to the service container
        /// </summary>
        /// <param name="serviceCollection">Service container</param>
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddRazorPages();
            serviceCollection.AddOpenApi();
            serviceCollection.AddMagicAuthApplicationServices(Configuration);

            var origins = new[] { "https://localhost:7286", "https://localhost:44368/", "http://localhost:5166" };
            var originsHashSet = new HashSet<string>(origins);

            serviceCollection.AddFido2(options =>
            {
                options.ServerDomain = "localhost";
                options.ServerName = "Fido Server";
                options.Origins = originsHashSet;
                options.TimestampDriftTolerance = 300000;
                options.MDSCacheDirPath = "";
            });

            serviceCollection.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Unspecified;
            });
            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/AccessDenied";
                options.LogoutPath = "/auth/logout";
                options.AccessDeniedPath = "/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(24);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });
            serviceCollection.AddAuthorization();
        }

        /// <summary>
        /// Configures the application HTTP request pipeline
        /// </summary>
        /// <param name="applicationBuilder">Application builder</param>
        /// <param name="webHostEnvironment">Web host environment</param>
        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }
            applicationBuilder.RunMigrations(Configuration.GetConnectionString("MagicAuth"));
            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseSession();
            applicationBuilder.UseStaticFiles();
            applicationBuilder.UseRouting();
            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();
            applicationBuilder.UseEndpoints(endpoints => 
            {
                endpoints.MapRazorPages();
                endpoints.MapAttestationRoutes();
                endpoints.MapAssertionRoutes();
                endpoints.MapSignOutRoutes();
                endpoints.MapOpenApi();
                endpoints.MapScalarApiReference();
            });
        }

        #endregion
    }
}
