using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Influx;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models.Identity;
using Theta.XSPOC.Apex.Api.Data.Mongo;
using Theta.XSPOC.Apex.Api.Data.Sql;
using Theta.XSPOC.Apex.Api.RealTimeData.Configuration;
using Theta.XSPOC.Apex.Kernel.Data;
using Theta.XSPOC.Apex.Kernel.Data.Sql;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.KeyVault;
using Theta.XSPOC.Apex.Kernel.Logging;
using System.IO.Compression;

namespace Theta.XSPOC.Apex.Api.RealTimeData
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public static class Program
    {
        private const string CORS_POLICY_NAME = "CorsPolicy";

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
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(s =>
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "Real Time Data API", Version = "v1" }));

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            builder.Services.AddSwaggerGen(option =>
            {
                option.IncludeXmlComments(xmlPath);

                var allyAPIHeader = builder.Configuration.GetValue<string>("AllyAPIAuthorization:ApiHeader");

                var allyDeploymentMode = builder.Configuration.GetValue<string>("AppSettings:ApplicationDeploymentMode");                

                // JWT Authorization header
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
                });

               

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                if (!string.IsNullOrEmpty(allyAPIHeader))
                {
                    // Define the custom header
                    option.AddSecurityDefinition(allyAPIHeader, new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Name = allyAPIHeader,
                        Type = SecuritySchemeType.ApiKey,
                        Description = "Custom header for service-to-service authentication"
                    });

                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                       {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = allyAPIHeader
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                }
                
                if (!string.IsNullOrEmpty(allyDeploymentMode) && allyDeploymentMode.ToLower() == "cloud")
                {
                    option.OperationFilter<CustomHeaderParameter>();                   
                }
            });

            builder.Host.ConfigureServices((hostContext, services) =>
            {
                // Configure services
                ConfigureServices(hostContext, services);
            });

            builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            builder.Services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.SmallestSize;
            });

            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });

            var app = builder.Build();

            if (corsEnabled)
            {
                app.UseCors(CORS_POLICY_NAME);
            }

            // Configure the HTTP request pipeline.
            app.UsePathBase(new PathString("/realtimedataapi"));
            app.UseRouting();
            if (app.Environment.IsDevelopment())
            {
                var basePath = "/realtimedataapi";
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentName}/swagger.json";
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer { Url = $"https://{httpReq.Host.Value}{basePath}" },
                            new OpenApiServer { Url = $"http://{httpReq.Host.Value}{basePath}" },
                        };
                    });
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{basePath}/swagger/v1/swagger.json", "Realtime Data API v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<TokenMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseResponseCompression();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {

            services.AddScoped<HttpClient>();
            services.AddSingleton<IDbConnectionInterceptor, NoLockDbConnectionInterceptor>();
            services.AddSingleton<IDateTimeConverter, DateTimeConverter>();
            services.AddSingleton<IVerbosityStore, VerbosityFromConfigurationStore>();
            services.AddSingleton<IThetaLoggerFactory, ThetaLoggerFactory>();
            services.AddSqlDbContext<XspocDbContext>(hostContext.Configuration.GetConnectionString("XspocDBConnection"));
            services.AddSqlDbContext<NoLockXspocDbContext>(hostContext.Configuration.GetConnectionString("NoLockXspocDBConnection"));
            services.AddSingleton<IOSSInfluxClientFactory, OSSInfluxClientFactory>();
            services.AddSingleton<IRealTimeDataProcessingService, RealTimeDataProcessingService>();
            services.AddSingleton<IAllyTimeSeriesNodeMaster, AllyTimeSeriesNodeMasterMongoStore>();
            services.AddSingleton<IParameterMongoStore, ParameterMongoStore>();
            services.AddSingleton<IGetDataHistoryItemsService, GetDataHistoryItemsService>();
            services.AddSingleton<IEnterpriseInfluxClientFactory, EnterpriseInfluxClientFactory>();

            var deploymentMode = hostContext.Configuration.GetSection("AppSettings:DeploymentMode").Value;

            if (deploymentMode == "OSS")
            {
                services.AddSingleton<IDataHistoryTrendData, OSSInfluxDataStore>();
                services.AddSingleton<IOSSInfluxClientFactory, OSSInfluxClientFactory>();
            }
            else if (deploymentMode == "Enterprise")
            {
                services.AddSingleton<IDataHistoryTrendData, EnterpriseInfluxDataStore>();
            }

            #region MongoDB Setup

            services.AddSingleton(serviceProvider =>
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

            #endregion

            services.AddSingleton<IRealTimeDataProcessingService, RealTimeDataProcessingService>();

            var appsettingsConfiguration = hostContext.Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appsettingsConfiguration);
            var appSettings = appsettingsConfiguration.Get<AppSettings>();

            services.AddAdminToolsAuthentication(hostContext.Configuration);

            services.ConfigureAdminToolsJwtAuthService(appSettings);

            services.AddTransient<IAuthService, AuthServiceSQLStore>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUserDefaultStore, UserDefaultSQLStore>();
            services.AddScoped<TokenMiddleware>();
        }

        /// <summary>
        /// Configure Admin Tools Jwt Auth Service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="appSettings">The app settings.</param>
        private static IServiceCollection ConfigureAdminToolsJwtAuthService(this IServiceCollection services, AppSettings appSettings)
        {
            AdminToolsService.AudienceSecret = appSettings.AudienceSecret;
            AdminToolsService.AudienceIssuer = appSettings.AudienceIssuer;
            AdminToolsService.Audience = appSettings.Audience;
            AdminToolsService.TimeOut = appSettings.TokenExpireInMinutes;

            var symmetricKeyAsBase64 = appSettings.AudienceSecret;

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

        /// <summary>
        /// The Add Admin Tools Authentication.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static IServiceCollection AddAdminToolsAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var clientId = configuration.GetValue<string>("AppSettings:AdminToolsClientId");
            var connection = configuration.GetConnectionString("XspocDBConnection");

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
    }
}