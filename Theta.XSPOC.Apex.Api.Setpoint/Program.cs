using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common.Communications;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql;
using Theta.XSPOC.Apex.Kernel.Data;
using Theta.XSPOC.Apex.Kernel.Data.Sql;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.KeyVault;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Setpoint
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
                            policy.WithOrigins(originList);
                        });
                });
            }

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson();

            builder.Services.AddMemoryCache();

            //
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //

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

            // Swagger API documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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

            var app = builder.Build();

            if (corsEnabled)
            {
                app.UseCors(CORS_POLICY_NAME);
            }

            // Configure the HTTP request pipeline.
            app.UsePathBase(new PathString("/setpointapi"));
            app.UseRouting();
            if (app.Environment.IsDevelopment())
            {
                var basePath = "/setpointapi";
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentName}/swagger.json";
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{basePath}" } };
                    });
                });
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionInterceptor, NoLockDbConnectionInterceptor>();

            services.AddSqlDbContext<XspocDbContext>(
                hostContext.Configuration.GetConnectionString("XspocDBConnection"));
            services.AddSqlDbContext<NoLockXspocDbContext>(
                hostContext.Configuration.GetConnectionString("NoLockXspocDBConnection"));
            services.AddSqlDbContext<XspocCommonDbContextBase>(
                hostContext.Configuration.GetConnectionString("XspocDBConnection"));

            services.AddSingleton<IVerbosityStore, VerbosityFromConfigurationStore>();
            services.AddSingleton<IThetaLoggerFactory, ThetaLoggerFactory>();
            services.AddSingleton<ITransactionPayloadCreator, TransactionPayloadCreator>();
            services.AddSingleton<INodeMaster, NodeMastersSQLStore>();
            services.AddSingleton<IParameterDataType, ParameterDataTypeSQLStore>();
            services.AddSingleton<ISetpointGroup, SetpointGroupSQLStore>();
            services.AddSingleton<ITransaction, TransactionSQLStore>();
            services.AddSingleton<IDateTimeConverter, DateTimeConverter>();

        }

    }
}
