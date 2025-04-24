using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Service which handles processing of asset data.
    /// </summary>
    public interface IAppSettingsService
    {

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        public AppplicationSettingsOutput GetApplicationSettings();

        /// <summary>
        /// Gets the deployment settings from app settings
        /// </summary>
        /// <returns></returns>
        public AppplicationSettingsOutput GetDeploymentSettings();

        /// <summary>
        /// Gets the redirect urls for onboarding app and ally connect web ui from app settings.
        /// </summary>
        /// <returns></returns>
        public AppplicationSettingsOutput GetAppURLs();

    }
}
