using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common.Converters;
using Theta.XSPOC.Apex.Api.Core.Middleware;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Mongo;
using Theta.XSPOC.Apex.Api.Data.Sql;
using Theta.XSPOC.Apex.Api.Users.Configuration;
using Theta.XSPOC.Apex.Kernel.Data;
using Theta.XSPOC.Apex.Kernel.Data.Sql;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.KeyVault;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Users
{
    /// <summary>
    /// The main program.
    /// </summary>
    public static class Program
    {

        private const string CORS_POLICY_NAME = "CorsPolicy";

        /// <summary>
        /// The main entry point.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
            {
                KeyVaultIntegration.AddKeyVaultConfiguration(config);
            });

            var corsEnabled = builder.Configuration.GetValue<bool>("CorsPolicy:Enabled");

            if (corsEnabled)
            {
                var originsConfig = builder.Configuration.GetSection("CorsPolicy:Origins").Get<string>();
                var originList = originsConfig.Split('|');

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(CORS_POLICY_NAME,
                        policy =>
                        {
                            policy.AllowAnyHeader();
                            policy.WithMethods("POST", "GET", "PUT");
                            policy.WithOrigins(originList);
                            policy.AllowCredentials();
                        });
                });
            }

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new CustomDateTimeConverter());
            });

            builder.Services.AddMemoryCache();
            builder.Services.AddLogging();

            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            // API Controller versioning
            builder.Services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new MediaTypeApiVersionReader("x-api-version"));
            });

            // API version explorer
            builder.Services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            var appsettingsConfiguration = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appsettingsConfiguration);
            var appSettings = appsettingsConfiguration.Get<AppSettings>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.ClaimsIssuer = appSettings.AudienceIssuer;
                options.Audience = appSettings.Audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(appSettings.AudienceSecret)),
                    ValidateIssuer = true,
                    ValidIssuer = appSettings.AudienceIssuer,
                    ValidateAudience = true,
                    ValidAudience = appSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Swagger API documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v2", new OpenApiInfo { Title = "Well Control API", Version = "v1" });
                option.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer",
                    }
                );
                option.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                },
                            },
                            Array.Empty<string>()
                        }
                    }
                );
            });
            builder.Services.AddSwaggerGenNewtonsoftSupport();
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

            builder.Host.ConfigureServices((hostContext, services) =>
            {
                // Configure services
                ConfigureServices(hostContext, services);
            });

            //Adding ApplicationInsights configuration to logging
            if (!string.IsNullOrEmpty(builder.Configuration["AdvancedLogging:ApplicationInsights:ConnectionString"]))
            {
                builder.Services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions()
                {
                    EnableAdaptiveSampling =
                        Convert.ToBoolean(builder.Configuration["AdvancedLogging:ApplicationInsights:EnableAdaptiveSampling"]),
                    EnableDebugLogger =
                        Convert.ToBoolean(
                            builder.Configuration[
                                "AdvancedLogging:ApplicationInsights:EnablePerformanceCounterCollectionModule"]),
                    ConnectionString = builder.Configuration["AdvancedLogging:ApplicationInsights:ConnectionString"]
                });
            }
            AddApplicationInsightsTelemetry(builder);

            var app = builder.Build();

            if (corsEnabled)
            {
                app.UseCors(CORS_POLICY_NAME);
            }

            // Configure the HTTP request pipeline.
            app.UsePathBase(new PathString("/usersapi"));
            app.UseRouting();
            if (app.Environment.IsDevelopment())
            {
                var basePath = "/usersapi";
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentName}/swagger.json";
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer { Url = $"https://{httpReq.Host.Value}{basePath}" },
                            new OpenApiServer { Url = $"http://{httpReq.Host.Value}{basePath}" }
                        };
                    });
                });
                app.UseSwaggerUI();
            }

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseResponseCompression();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection serviceCollection)
        {

            serviceCollection.AddSingleton<IVerbosityStore, VerbosityFromConfigurationStore>();
            serviceCollection.AddSingleton<IThetaLoggerFactory, ThetaLoggerFactory>();
            serviceCollection.AddSingleton<IDateTimeConverter, DateTimeConverter>();
            serviceCollection.AddSingleton<IDbConnectionInterceptor, NoLockDbConnectionInterceptor>();
            serviceCollection.AddSingleton<ITelemetryInitializer, CustomTelemetryInitializer>();

            #region Services

            serviceCollection.AddSingleton<IUserService, UserService>();
            serviceCollection.AddScoped<AuthenticationMiddleware>();

            #endregion

            #region SQL Server Setup

            serviceCollection.AddSqlDbContext<XspocDbContext>(
                hostContext.Configuration.GetConnectionString("XspocDBConnection"));
            serviceCollection.AddSqlDbContext<NoLockXspocDbContext>(
                hostContext.Configuration.GetConnectionString("NoLockXspocDBConnection"));
            serviceCollection.AddSqlDbContext<XspocCommonDbContextBase>(
                hostContext.Configuration.GetConnectionString("XspocDBConnection"));

            #endregion

            #region Data Stores

            serviceCollection.AddSingleton(serviceProvider =>
            {
                var databaseName = hostContext.Configuration.GetSection("XSPOCDatabase:DatabaseName").Value;
                var connectionString = hostContext.Configuration.GetSection("XSPOCDatabase:ConnectionString").Value;
                var userName = hostContext.Configuration.GetSection("XSPOCDatabase:Username").Value;
                var password = hostContext.Configuration.GetSection("XSPOCDatabase:Password").Value;

                var clientSettings = MongoClientSettings.FromConnectionString(connectionString);

                if (string.IsNullOrWhiteSpace(userName) == false &&
                    string.IsNullOrWhiteSpace(password) == false)
                {
                    clientSettings.Credential = new MongoCredential("SCRAM-SHA-256",
                        new MongoInternalIdentity(databaseName, userName),
                        new PasswordEvidence(password));
                }

                var mongoClient = new MongoClient(clientSettings);

                return mongoClient.GetDatabase(databaseName);
            });

            serviceCollection.AddSingleton<IMongoOperations, MongoOperations>();

            serviceCollection.AddSingleton<IUserDefaultStore, UserDefaultSQLStore>();

            #endregion

        }

        private static void AddApplicationInsightsTelemetry(WebApplicationBuilder builder)
        {
            var applicationInsightsUrl = builder.Configuration.GetSection("ApplicationInsights:ConnectionString").Value;
            if (string.IsNullOrWhiteSpace(applicationInsightsUrl) == false)
            {
                builder.Services.AddApplicationInsightsTelemetry(options =>
                {
                    options.ConnectionString = applicationInsightsUrl;
                    options.EnableActiveTelemetryConfigurationSetup = true;
                });
            }
            else
            {
                // Removing this will cause an exception in CustomTelemetryInitializer when the services are built.
                builder.Services.AddHttpContextAccessor();
            }
        }

    }
}
