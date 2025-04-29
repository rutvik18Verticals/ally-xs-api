using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models.Dashboard;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// The interface that defines the methods for the dashboard store.
    /// </summary>
    public interface IDashboardStore
    {

        /// <summary>
        /// Gets the dashboard widgets for the <paramref name="dashboardType"/> and user <paramref name="userId"/>.
        /// </summary>
        /// <param name="dashboardType">The dashhboard type.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="isStacked">Is Stacked view dashboard.</param>
        Task<DashboardWidgetDataModel> GetDashboardWidgets(string dashboardType, string userId,
            string correlationId, bool isStacked);

        /// <summary>
        /// Updates the user preference for the dashboard widget
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        Task<bool> SaveDashboardWidgetUserPrefeernces(DashboardWidgetPreferenceDataModel input, string userId, string correlationId);

        /// <summary>
        /// Gets the dashboard widget properties for the <paramref name="dashboardName"/> and <paramref name="widgetName"/>.
        /// </summary>
        /// <param name="dashboardName">dashboard name. </param>
        /// <param name="widgetName">widget name.</param>
        /// <param name="userId">lgged in user id.</param>
        /// <param name="correlationId">correlation id.</param>
        /// <returns></returns>
        Task<WidgetPropertyDataModel> GetDashboardWidgetData(string dashboardName, string widgetName, string userId, string correlationId);


        /// <summary>
        /// Resets the user preferences for the dashboard widget
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        Task<bool> ResetDashboardWidgetUserPrefeernces(DashboardWidgetPreferenceDataModel input, string userId, string correlationId);
    }
}
