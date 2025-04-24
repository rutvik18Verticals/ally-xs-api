using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Service which handles processing of application settings.
    /// </summary>
    public class AppSettingsService : IAppSettingsService
    {

        #region Private Fields

        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constants

        private const string LOGIN_MODE = "formlogin";
        private const string DEPLOYMENT_MODE = "onprem";
        private const string ENABLE_PUMPCHECKER = "false";
        private const string HOSTING_ENVIRONMENT = "XSPOC";

        #endregion

        #region Protected Fields

        /// <summary>
        /// The <seealso cref="IConfiguration"/> configurations.
        /// </summary>
        protected IConfiguration AppConfig { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of a <see cref="AssetDataService"/>.
        /// </summary>
        public AppSettingsService(IThetaLoggerFactory loggerFactory, IConfiguration appConfig)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
        }

        #endregion

        #region IAppSettingsService Implementation

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        public AppplicationSettingsOutput GetApplicationSettings()
        {
            var logger = _loggerFactory.Create(LoggingModel.APIService);
            logger.Write(Level.Trace, $"Starting {nameof(AssetDataService)} {nameof(GetApplicationSettings)}");

            var appDeploymentMode = AppConfig.GetSection("AppSettings:ApplicationDeploymentMode").Value ?? DEPLOYMENT_MODE;
            var isPumpCheckerEnabled = AppConfig.GetSection("AppSettings:EnablePumpChecker").Value ?? ENABLE_PUMPCHECKER;
            var hostingEnvironment = AppConfig.GetSection("AppSettings:HostingEnvironment").Value ?? HOSTING_ENVIRONMENT;

            Dictionary<string, string> settings = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(appDeploymentMode))
            {
                logger.Write(Level.Error, "ApplicationDeploymentMode app settings is missing.");
            }
            else
            {
                settings.Add("ApplicationDeploymentMode", appDeploymentMode);
            }

            if (string.IsNullOrEmpty(isPumpCheckerEnabled))
            {
                logger.Write(Level.Error, "EnablePumpChecker app settings is missing.");
            }
            else
            {
                settings.Add("EnablePumpChecker", isPumpCheckerEnabled);
            }

            if (string.IsNullOrEmpty(hostingEnvironment))
            {
                logger.Write(Level.Error, "HostingEnvironment app settings is missing.");
            }
            else
            {
                settings.Add("HostingEnvironment", hostingEnvironment);
            }
            logger.Write(Level.Trace, $"Finished {nameof(AssetDataService)} {nameof(GetApplicationSettings)}");

            if(settings.Count == 0)
            {
                return new AppplicationSettingsOutput()
                {
                    Result = new MethodResult<string>(false, "Failed to get application settings from configuration.")
                };
            }

            return new AppplicationSettingsOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
                ApplicationSettings = settings
            };
        }
        
        /// <summary>
        /// Gets the redirect urls for onboarding app and ally connect web ui from app settings.
        /// </summary>
        /// <returns></returns>
        public AppplicationSettingsOutput GetAppURLs()
        {
            var logger = _loggerFactory.Create(LoggingModel.APIService);
            logger.Write(Level.Trace, $"Starting {nameof(AssetDataService)} {nameof(GetAppURLs)}");

            var onboardingURL = AppConfig.GetSection("AppSettings:AllyOnboardingUIURL").Value;
            var pumpcheckerURL = AppConfig.GetSection("AppSettings:PumpCheckerURL").Value;
            var allyConnectLoginURL = AppConfig.GetSection("AppSettings:LoginURL").Value;

            Dictionary<string, string> settings = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(onboardingURL))
            {
                logger.Write(Level.Error, "AllyOnboardingUIURL app settings is missing.");
            }
            else
            {
                settings.Add("AllyOnboardingUIURL", onboardingURL);
            }

            if (string.IsNullOrEmpty(pumpcheckerURL))
            {
                logger.Write(Level.Error, "PumpCheckerURL app settings is missing.");
            }
            else
            {
                settings.Add("PumpCheckerURL", pumpcheckerURL);
            }

            if (string.IsNullOrEmpty(allyConnectLoginURL))
            {
                logger.Write(Level.Error, "LoginURL app settings is missing.");
            }
            else
            {
                settings.Add("LoginURL", allyConnectLoginURL);
            }

            logger.Write(Level.Trace, $"Finished {nameof(AssetDataService)} {nameof(GetAppURLs)}");

            if (settings.Count == 0)
            {
                return new AppplicationSettingsOutput()
                {
                    Result = new MethodResult<string>(false, "Failed to get application settings from configuration.")
                };
            }

            return new AppplicationSettingsOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
                ApplicationSettings = settings
            };
        }

        /// <summary>
        /// Gets the deployment settings from app settings
        /// </summary>
        public AppplicationSettingsOutput GetDeploymentSettings()
        {
            var logger = _loggerFactory.Create(LoggingModel.APIService);
            logger.Write(Level.Trace, $"Starting {nameof(AssetDataService)} {nameof(GetDeploymentSettings)}");

            var appDeploymentMode = AppConfig.GetSection("AppSettings:ApplicationDeploymentMode").Value ?? DEPLOYMENT_MODE;
            var loginMode = AppConfig.GetSection("AppSettings:LoginMode").Value ?? LOGIN_MODE;
            var loginURL = AppConfig.GetSection("AppSettings:LoginURL").Value;

            Dictionary<string, string> settings = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(loginURL))
            {
                logger.Write(Level.Error, "LoginURL app settings is missing.");
            }
            else
            {
                settings.Add("LoginURL", loginURL);
            }

            if (string.IsNullOrEmpty(appDeploymentMode))
            {
                logger.Write(Level.Error, "ApplicationDeploymentMode app settings is missing.");
            }
            else
            {
                settings.Add("ApplicationDeploymentMode", appDeploymentMode);
            }

            if (string.IsNullOrEmpty(loginMode))
            {
                logger.Write(Level.Error, "LoginMode app settings is missing.");
            }
            else
            {
                settings.Add("LoginMode", loginMode);
            }

            logger.Write(Level.Trace, $"Finished {nameof(AssetDataService)} {nameof(GetDeploymentSettings)}");

            if (settings.Count == 0)
            {
                return new AppplicationSettingsOutput()
                {
                    Result = new MethodResult<string>(false, "Failed to get application settings from configuration.")
                };
            }

            return new AppplicationSettingsOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
                ApplicationSettings = settings
            };
        }

        #endregion

    }
}
