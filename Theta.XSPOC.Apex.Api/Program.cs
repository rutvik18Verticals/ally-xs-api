using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using theta.XSPOC.Apex.Api.Contracts.JWTToken;
using Theta.XSPOC.Apex.Api.Common.Converters;
using Theta.XSPOC.Apex.Api.Contracts.JWTToken;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;
using Theta.XSPOC.Apex.Api.Common.Calculators.ESP;
using Theta.XSPOC.Apex.Api.Common.Calculators.Well;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Core.Services.AssetStatus;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Alarms;
using Theta.XSPOC.Apex.Api.Data.Asset;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.HistoricalData;
using Theta.XSPOC.Apex.Api.Data.Influx;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Mongo;
using Theta.XSPOC.Apex.Api.Data.Sql;
using Theta.XSPOC.Apex.Api.Data.Sql.Alarm;
using Theta.XSPOC.Apex.Api.Data.Sql.Asset;
using Theta.XSPOC.Apex.Api.Data.Sql.HistoricalData;
using Theta.XSPOC.Apex.Kernel.Data;
using Theta.XSPOC.Apex.Kernel.Data.Sql;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.KeyVault;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.UnitConversion;
using Microsoft.ApplicationInsights.Extensibility;
using System.Net.Http;
using Theta.XSPOC.Apex.Api.Core.Models.Configuration;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Core.Middleware;
using Theta.XSPOC.Apex.Api.Core.Services.DashboardService;
using Theta.XSPOC.Apex.Api.Core.Services.UserAccountService;

namespace Theta.XSPOC.Apex.Api
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
        public static void Main(string[] args)
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

            //
            // Add services to the container
            //

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new CustomDateTimeConverter());
            });

            builder.Services.AddMemoryCache();

            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

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

            var appsettingsConfiguration = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appsettingsConfiguration);
            var appSettings = appsettingsConfiguration.Get<AppSettings>();

            builder.Services.AddAdminToolsAuthentication(options =>
            {
                options.ClientId = builder.Configuration.GetValue<string>("AppSettings:AdminToolsClientId");
                options.Connection = builder.Configuration.GetConnectionString("XspocDBConnection");
            });

            builder.Services.ConfigureAdminToolsJwtAuthService(options =>
            {
                options.AudienceSecret = appSettings.AudienceSecret;
                options.AudienceIssuer = appSettings.AudienceIssuer;
                options.Audience = appSettings.Audience;
                options.TimeOut = appSettings.TokenExpireInMinutes;
                options.Module = AdminToolsModule.AMPAdminTools;
                options.UseSecureCookies = appSettings.UseSecureCookies;
            });

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v2", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
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
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                );
            });

            AddApplicationInsightsTelemetry(builder);

            var app = builder.Build();

            if (corsEnabled)
            {
                app.UseCors(CORS_POLICY_NAME);
            }

            InitializeEnhancedEnumBaseImplementations(app.Services);

            // Configure the HTTP request pipeline.
            app.UsePathBase(new PathString("/api"));
            app.UseRouting();
            if (app.Environment.IsDevelopment())
            {
                var basePath = "/api";
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

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddScoped<HttpClient>();
            services.AddSingleton<IDbConnectionInterceptor, NoLockDbConnectionInterceptor>();
            services.AddSingleton<IDateTimeConverter, DateTimeConverter>();

            services.AddSqlDbContext<XspocDbContext>(
                hostContext.Configuration.GetConnectionString("XspocDBConnection"));
            services.AddSqlDbContext<NoLockXspocDbContext>(
                hostContext.Configuration.GetConnectionString("NoLockXspocDBConnection"));
            services.AddSqlDbContext<XspocCommonDbContextBase>(
                hostContext.Configuration.GetConnectionString("XspocDBConnection"));

            services.AddSingleton<IAssetDataService, AssetDataService>();

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:AssetData"))
            {
                services.AddSingleton<IAssetData, AssetDataMongoStore>();
            } 
            else
            {
                services.AddSingleton<IAssetData, AssetDataSQLStore>();
            }

            services.AddSingleton<INotificationProcessingService, NotificationProcessingService>();
            services.AddSingleton<IGroupAndAssetService, GroupAndAssetService>();
            services.AddSingleton<IGroupAndAsset, GroupAndAssetMongoStore>();
            services.AddSingleton<INotification, NotificationSQLStore>();
            services.AddSingleton<IVerbosityStore, VerbosityFromConfigurationStore>();
            services.AddSingleton<IThetaLoggerFactory, ThetaLoggerFactory>();

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:CardCoordinate"))
            {
                services.AddSingleton<ICardCoordinate, CardCoordinateMongoStore>();
            } 
            else
            {
                services.AddSingleton<ICardCoordinate, CardCoordinateSQLStore>();
            }

            services.AddSingleton<ICommonService, CommonService>();

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:LocalePhrases"))
            {
                services.AddSingleton<ILocalePhrases, LocalePhrasesMongoStore>();
            } 
            else
            {
                services.AddSingleton<ILocalePhrases, LocalePhrasesSQLStore>();
            }

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:States"))
            {
                services.AddSingleton<IStates, StatesMongoStore>();
            }
            else
            {
                services.AddSingleton<IStates, StatesSQLStore>();
            }

            services.AddSingleton<IRodLiftAnalysisProcessingService, RodLiftAnalysisProcessingService>();

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:RodLiftAnalysis"))
            {
                services.AddSingleton<IRodLiftAnalysis, RodLiftAnalysisMongoStore>();
            }
            else
            {
                services.AddSingleton<IRodLiftAnalysis, RodLiftAnalysisSQLStore>();
            }
            
            services.AddSingleton<IAnalysisCurve, AnalysisCurveSQLStore>();
            services.AddSingleton<ICurveCoordinate, CurveCoordinateSQLStore>();
            services.AddSingleton<IAnalysisCurveSets, AnalysisCurveSetsSQLStore>();
            services.AddSingleton<IESPTornadoCurveSetAnnotation, ESPTornadoCurveSetAnnotationSQLStore>();

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:NodeMaster"))
            {
                services.AddSingleton<INodeMaster, NodeMastersMongoStore>();
            }
            else
            {
                services.AddSingleton<INodeMaster, NodeMastersSQLStore>();
            }

            services.AddSingleton<IAllyTimeSeriesNodeMaster, AllyTimeSeriesNodeMasterMongoStore>();
            services.AddSingleton<IESPAnalysisProcessingService, ESPAnalysisProcessingService>();
            services.AddSingleton<IESPAnalysis, ESPAnalysisSQLStore>();
            services.AddSingleton<IESPPump, ESPPumpDataSQLStore>();
            services.AddSingleton<IGLAnalysisProcessingService, GLAnalysisProcessingService>();
            services.AddSingleton<IGLAnalysis, GLAnalysisSQLStore>();
            services.AddSingleton<IWellAnalysisCorrelation, WellAnalysisCorrelationSQLStore>();

            services.AddScoped<ILoggedInUserProvider, LoggedInUserProvider>();
            #region Dashboard

            services.AddSingleton<IDashboardWidgetService, DashboardWidgetService>();
            services.AddSingleton<IDashboardStore, DashboardMongoStore>();            

            #endregion

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:Manufacturer"))
            {
                services.AddSingleton<IManufacturer, GLManufacturerMongoStore>();
            } 
            else
            {
                services.AddSingleton<IManufacturer, GLManufacturerSQLStore>();
            }

            services.AddSingleton<IGroupStatusProcessingService, GroupStatusProcessingService>();
            services.AddSingleton<IGroupStatus, GroupStatusSQLStore>();

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:ParameterDataType"))
            {
                services.AddSingleton<IParameterDataType, ParameterDataTypeMongoStore>();
            }
            else 
            {
                services.AddSingleton<IParameterDataType, ParameterDataTypeSQLStore>();
            }

            services.AddSingleton<IWellTestsProcessingService, WellTestsProcessingService>();
            services.AddSingleton<IWellTests, WellTestsSQLStore>();
            services.AddSingleton<IDataHistorySQLStore, DataHistorySQLStore>();
            services.AddSingleton<IDataHistoryMongoStore, DataHistoryMongoStore>();
            
            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:PocType"))
            {
                services.AddSingleton<IPocType, PocTypeMongoStore>();
            }
            else
            {
                services.AddSingleton<IPocType, PocTypeSQLStore>();
            }

            services.AddSingleton<IDataHistoryProcessingService, DataHistoryProcessingService>();
            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:EnumEntity"))
            {
                services.AddSingleton<IEnumEntity, EnumEntityMongoStore>();
            } else
            {
                services.AddSingleton<IEnumEntity, EnumEntitySQLStore>();
            }
            services.AddSingleton<IGLAnalysisGetCurveCoordinate, GLAnalysisGetCurveCoordinateSQLStore>();
            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:HostAlarm"))
            {
                services.AddSingleton<IHostAlarm, HostAlarmMongoStore>();
            } 
            else
            {
                services.AddSingleton<IHostAlarm, HostAlarmSQLStore>();
            }
            services.AddSingleton<INodeMasterEndpointProcessingService, NodeMasterEndpointProcessingService>();
            services.AddSingleton<IGetDataHistoryItemsService, GetDataHistoryItemsService>();

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:PortConfigurationStore"))
            {
                services.AddSingleton<IPortConfigurationStore, PortConfigurationMongoStore>();
            }
            else
            {
                services.AddSingleton<IPortConfigurationStore, PortConfigurationSQLStore>();
            }

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:Exception"))
            {
                services.AddSingleton<IException, ExceptionMongoStore>();
            } else
            {
                services.AddSingleton<IException, ExceptionSQLStore>();
            }

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:PumpingUnit"))
            {
                services.AddSingleton<IPumpingUnit, PumpingUnitMongoStore>();
            }
            else
            {
                services.AddSingleton<IPumpingUnit, PumpingUnitSQLStore>();
            }

            services.AddSingleton<ISystemParameter, SystemParameterSQLStore>();

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:PumpingUnitManufacturer"))
            {
                services.AddSingleton<IPumpingUnitManufacturer, PumpingUnitManufacturerMongoStore>();
            } 
            else
            {
                services.AddSingleton<IPumpingUnitManufacturer, PumpingUnitManufacturerSQLStore>();
            }

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:Rod"))
            {
                services.AddSingleton<IRod, RodMongoStore>();
            }
            else
            {
                services.AddSingleton<IRod, RodSQLStore>();
            }

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:StringIdStore"))
            {
                services.AddSingleton<IStringIdStore, StringIdMongoStore>();
            }
            else
            {
                services.AddSingleton<IStringIdStore, StringIdSQLStore>();
            }

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:UserDefaultStore"))
            {
                services.AddSingleton<IUserDefaultStore, UserDefaultMongoStore>();
            } 
            else
            {
                services.AddSingleton<IUserDefaultStore, UserDefaultSQLStore>();
            }

            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ITransaction, TransactionSQLStore>();
            services.AddSingleton<IAssetStore, RodLiftAssetSQLStore>();
            services.AddSingleton<IHistoricalStore, HistoricalSQLStore>();

            if (hostContext.Configuration.GetValue<bool>("MongoDbInjection:AlarmStore"))
            {
                services.AddSingleton<IAlarmStore, AlarmMongoStore>();
            }
            else
            {
                services.AddSingleton<IAlarmStore, AlarmSQLStore>();
            }

            services.AddSingleton<IAlarmInfluxStore, AlarmInfluxStore>();
            services.AddSingleton<IUnitConversion, UnitConversion>();
            services.AddSingleton<ISystemSettingStore, SystemSettingStore>();
            services.AddSingleton<IAssetStatusService, AssetStatusService>();
            services.AddSingleton<IParameterMongoStore, ParameterMongoStore>();
            services.AddSingleton<IOSSInfluxClientFactory, OSSInfluxClientFactory>();
            services.AddSingleton<IEnterpriseInfluxClientFactory, EnterpriseInfluxClientFactory>();
            services.AddSingleton<IAppSettingsService, AppSettingsService>();

            var deploymentMode = hostContext.Configuration.GetSection("AppSettings:DeploymentMode").Value;

            if (deploymentMode == "OSS")
            {
                services.AddSingleton<IDataHistoryTrendData, OSSInfluxDataStore>();
            }
            else if (deploymentMode == "Enterprise")
            {
                services.AddSingleton<IDataHistoryTrendData, EnterpriseInfluxDataStore>();
            }

            var applicationDeploymentMode = hostContext.Configuration.GetSection("AppSettings:ApplicationDeploymentMode").Value;

            if (applicationDeploymentMode == "cloud")
            {
                services.AddScoped<ITokenValidation, CNXTokenValidation>();

                services.Configure<ConnexiaWebBaseUrlSettings>(hostContext.Configuration.GetSection("ConnexiaWebSettings"));
                services.AddScoped<ConnexiaWebBaseUrlSettings>();
            }
            else
            {
                services.AddScoped<ITokenValidation, AllyTokenValidation>();
            }

            services.AddScoped<IHttpClientHelperService, HttpClientHelperService>();

            #region Calculators

            services.AddSingleton<IUnsafeGasAwareESPCalculator, UnsafeESPCalculator>();
            services.AddSingleton<IGasAwareESPCalculator, ESPCalculator>();

            services.AddSingleton<IUnsafeWellCalculator, UnsafeWellCalculator>();
            services.AddSingleton<IUnsafeGasAwareWellCalculator, UnsafeWellCalculator>();
            services.AddSingleton<IWellCalculator, WellCalculator>();
            services.AddSingleton<IGasAwareWellCalculator, WellCalculator>();

            #endregion

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

            services.AddSingleton<IMongoOperations, MongoOperations>();

            AddGroupStatusColumnFormatters(services);

            #region Caches

            services.AddSingleton<IMemoryCache, MemoryCache>();

            #endregion

            services.AddSingleton<ITelemetryInitializer, CustomTelemetryInitializer>();
            services.AddScoped<AuthenticationMiddleware>();
        }

        private static void AddGroupStatusColumnFormatters(IServiceCollection services)
        {
            services.AddScoped<IColumnFormatterCacheService, ColumnFormatterCacheService>();
            services.AddSingleton<IColumnFormatterFactory, ColumnFormatterFactory>();
            services.AddSingleton<IColumnFormatter, ControllerColumnFormatter>();
            services.AddSingleton<IColumnFormatter, CommColumnFormatter>();
            services.AddSingleton<IColumnFormatter, ConditionalColumnFormatter>();
            services.AddSingleton<IColumnFormatter, PercentCommColumnFormatter>();
            services.AddSingleton<IColumnFormatter, HostAlarmColumnFormatter>();
            services.AddSingleton<IColumnFormatter, ExceptionColumnFormatter>();
            services.AddSingleton<IColumnFormatter, PumpingUnitColumnFormatter>();
            services.AddSingleton<IColumnFormatter, PumpingUnitManufacturerColumnFormatter>();
            services.AddSingleton<IColumnFormatter, PercentRTColumnFormatter>();
            services.AddSingleton<IColumnFormatter, RodGradeColumnFormatter>();
            services.AddSingleton<IColumnFormatter, PercentFillColumnFormatter>();
            services.AddSingleton<IColumnFormatter, AlarmColumnFormatter>();
            services.AddSingleton<IColumnFormatter, DRCCColumnFormatter>();
            services.AddSingleton<IColumnFormatter, PercentRTYColumnFormatter>();
            services.AddSingleton<IColumnFormatter, StringIdColumnFormatter>();
            services.AddSingleton<IColumnFormatter, TISColumnFormatter>();
            services.AddSingleton<IColumnFormatter, RunStatusColumnFormatter>();
            services.AddSingleton<IColumnFormatter, FacilityTagAlarmsColumnFormatter>();
            services.AddSingleton<IColumnFormatter, CameraAlarmColumnFormatter>();
            services.AddSingleton<IColumnFormatter, EnabledColumnFormatter>();
            services.AddSingleton<IColumnFormatter, FacilityColumnFormatter>();
            services.AddSingleton<IColumnFormatter, ParamStandardTypeColumnFormatter>();
            services.AddSingleton<IColumnFormatter, ParameterColumnFormatter>();
        }

        private static void InitializeEnhancedEnumBaseImplementations(IServiceProvider serviceProvider)
        {
            var localePhrases = serviceProvider.GetRequiredService<ILocalePhrases>();
            var enumEntityModel = serviceProvider.GetRequiredService<IEnumEntity>();
            var enumNameService = new EnumNameService(localePhrases, enumEntityModel);
            enumNameService.Initialize();
        }

    }
}
