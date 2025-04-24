using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Contracts.JWTToken;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models.Identity;
using Theta.XSPOC.Apex.Api.Data.Sql;

namespace theta.XSPOC.Apex.Api.Contracts.JWTToken
{
    /// <summary>
    /// represents the AdminToolsExtension.
    /// </summary>
    public static class AdminToolsExtension
    {

        /// <summary>
        /// The Add Admin Tools Authentication.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="options">The options.</param>
        public static IServiceCollection AddAdminToolsAuthentication(this IServiceCollection services, Action<AdminToolsAuthenticationOptions> options)
        {
            var authenticationOptions = new AdminToolsAuthenticationOptions();

            options?.Invoke(authenticationOptions);

            var connection = authenticationOptions.Connection;

            _ = services.AddDbContext<XspocDbContext>(opts =>
            {
                opts.UseSqlServer(connection, contextOptionsBuilder => contextOptionsBuilder.EnableRetryOnFailure());
                opts.EnableSensitiveDataLogging();

            }, ServiceLifetime.Transient);

            _ = services.AddTransient<PasswordValidator<AppUser>>();

            _ = services.AddIdentityCore<AppUser>()
            .AddEntityFrameworkStores<XspocDbContext>()
            .AddDefaultTokenProviders();
            _ = services.AddTransient<IAdminToolsService, AdminToolsService>();
            _ = services.AddTransient<IAuthService, AuthServiceSQLStore>();

            return services;
        }

        /// <summary>
        /// Configure Admin Tools Jwt Auth Service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="options">The options.</param>
        public static IServiceCollection ConfigureAdminToolsJwtAuthService(this IServiceCollection services, Action<AdminToolsJwtAuthenticationOptions> options)
        {

            var jwtAuthenticationOptions = new AdminToolsJwtAuthenticationOptions();

            options?.Invoke(jwtAuthenticationOptions);

            AdminToolsService.AudienceSecret = jwtAuthenticationOptions.AudienceSecret;
            AdminToolsService.AudienceIssuer = jwtAuthenticationOptions.AudienceIssuer;
            AdminToolsService.Audience = jwtAuthenticationOptions.Audience;
            AdminToolsService.TimeOut = jwtAuthenticationOptions.TimeOut;
            AdminToolsService.UseSecureCookie = jwtAuthenticationOptions.UseSecureCookies;

            var symmetricKeyAsBase64 = jwtAuthenticationOptions.AudienceSecret;

            if (symmetricKeyAsBase64 != null)
            {
                var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
                var signingKey = new SymmetricSecurityKey(keyByteArray);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                    .AddJwtBearer(cfg =>
                    {
                        cfg.RequireHttpsMetadata = false;
                        cfg.SaveToken = true;
                        cfg.TokenValidationParameters = tokenValidationParameters;
                        cfg.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {
                                return Task.CompletedTask;
                            }
                        };
                    });
            }

            return services;
        }

    }
}
