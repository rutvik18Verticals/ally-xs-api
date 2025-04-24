using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Theta.XSPOC.Apex.Api.RealTimeData
{
    /// <summary>
    /// Create custom authorization logic with a reusable token validation service.
    /// </summary>
    public class TokenMiddleware : IMiddleware
    {

        #region Private Fields

        /// <summary>
        /// The <seealso cref="IConfiguration"/> configurations.
        /// </summary>
        protected IConfiguration AppConfig { get; }

        private readonly IAdminToolsService _adminToolsService;

        private readonly IAllyTimeSeriesNodeMaster _allyNodeMasterStore;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the middleware for authorization championx token or custom authorization attribute.
        /// </summary>
        /// <param name="configuration">The <seealso cref="IConfiguration"/>.</param>
        /// <param name="adminToolsService">The <seealso cref="IAdminToolsService"/>.</param>
        public TokenMiddleware(IConfiguration configuration, IAdminToolsService adminToolsService, IAllyTimeSeriesNodeMaster allyNodeMasterStore)
        {
            AppConfig = configuration;
            _adminToolsService = adminToolsService;
            _allyNodeMasterStore = allyNodeMasterStore ?? throw new ArgumentNullException(nameof(allyNodeMasterStore));
        }

        #endregion

        /// <summary>
        /// Invoke httpContext.
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <param name="next">The request delegate.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(context == null || next == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if(!context.Request.Headers.TryGetValue(CustomHeaders.XSPOC_UI_TRACKING_ID, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers.Add(CustomHeaders.XSPOC_UI_TRACKING_ID, correlationId);
            }
 
            var allyAPIHeader = AppConfig.GetSection("AllyAPIAuthorization:ApiHeader").Value;
            var allyDeploymentMode = AppConfig.GetSection("AppSettings:ApplicationDeploymentMode").Value;

            if (!string.IsNullOrEmpty(allyDeploymentMode) && allyDeploymentMode.ToLower() == "onprem")
            {
                // Check for custom authorization header
                if (allyAPIHeader != null && context.Request.Headers.ContainsKey(allyAPIHeader))
                {
                    if (context.Request.Headers.TryGetValue(allyAPIHeader, out var extractedApiKey))
                    {
                        var apiKey = AppConfig.GetSection("AllyAPIAuthorization:ApiSecret").Value;
                        if (apiKey != null && !apiKey.Equals(extractedApiKey, StringComparison.Ordinal))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync("Unauthorized client.");

                            return;
                        }
                        else
                        {
                            var ApiUser = AppConfig.GetSection("AllyAPIAuthorization:ApiUser").Value;
                            var inputWithCorrelationId = new WithCorrelationId<string>(string.Empty, ApiUser);
                            var refresh_token = _adminToolsService.GetJwtRefresh(inputWithCorrelationId);

                            if (!context.Request.Headers.ContainsKey("Authorization"))
                            {
                                context.Request.Headers.TryAdd("Authorization", $"Bearer {refresh_token}");
                            }
                        }

                        await next.Invoke(context);
                    }

                    return;
                }
            }

            if (!string.IsNullOrEmpty(allyDeploymentMode) && allyDeploymentMode.ToLower()=="cloud")
            {

                context.Request.Headers.TryGetValue(allyAPIHeader, out var extractedApiKey);               
                context.Request.Headers.TryGetValue("userAccount", out var extractedUserAccount);

                bool isValidCustomer = false;

                if (!extractedApiKey.IsNullOrEmpty() && !extractedUserAccount.IsNullOrEmpty())
                {
                    var custData = await _allyNodeMasterStore.ValidateCustomerAsync(extractedUserAccount, allyAPIHeader, extractedApiKey, correlationId);
                    if (custData != null)
                    {
                        
                        if (custData.Count > 0)
                        {
                            isValidCustomer = custData[0].IsValid;
                        }
                    }
                }
                if (!isValidCustomer)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized client.");

                    return;
                }
                else
                {
                    var ApiUser = AppConfig.GetSection("AllyAPIAuthorization:ApiUser").Value;
                    var inputWithCorrelationId = new WithCorrelationId<string>(string.Empty, ApiUser);
                    var refresh_token = _adminToolsService.GetJwtRefresh(inputWithCorrelationId);

                    if (!context.Request.Headers.ContainsKey("Authorization"))
                    {
                        context.Request.Headers.TryAdd("Authorization", $"Bearer {refresh_token}");
                    }
                }

                await next.Invoke(context);
                return;

            }

            if (context?.Request?.Cookies == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                return;
            }
            if (next == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                return;
            }

            var authCookie = context.Request.Cookies["Ally-Authorization"];
            if (!context.Request.Headers.ContainsKey("Authorization") && authCookie != null)
            {
                context.Request.Headers.TryAdd("Authorization", $"Bearer {authCookie}");
                await next.Invoke(context);
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }
}
