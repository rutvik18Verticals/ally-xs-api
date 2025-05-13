using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Core.Services.DashboardService
{
    /// <summary>
    /// Interface for Dashboard Widget Service.
    /// </summary>
    public interface IDashboardWidgetService
    {

        /// <summary>
        /// Gets the list of all chart widgets for ESP Well.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="dashboardType">The dashboard type.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="isStacked">isStacked flag.</param>
        /// <returns> <seealso cref="DashboardWidgetDataOutput"/>.</returns>
        DashboardWidgetDataOutput GetESPWellChartWidgets(string dashboardType, string userId, string correlationId, bool isStacked);

        /// <summary>
        /// Updates the user preferences for the dashboard widget
        /// </summary>
        /// <param name="input">The dashboard widget preferences to be updated.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        Task<bool> SaveWidgetUserPreferences(DashboardWidgetUserPreferencesInput input, string userId, string correlationId);

        /// <summary>
        /// Resets the user preferences for the dashboard widget
        /// </summary>
        /// <param name="input">The dashboard widget preferences to be reset.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        Task<DashboardWidgetDataOutput> ResetWidgetUserPreferences(DashboardWidgetResetUserPreferencesInput input, string userId, string correlationId);

        /// <summary>
        /// Gets the list of all default parameters for the given lift type.
        /// </summary>
        /// <param name="liftType">list type to be searched for</param>
        /// <param name="correlationId">correlation id</param>
        /// <returns></returns>
        Task<DefaultParameterDataOutput> GetAllDefaultParameters(string liftType, string correlationId);
    }
}
