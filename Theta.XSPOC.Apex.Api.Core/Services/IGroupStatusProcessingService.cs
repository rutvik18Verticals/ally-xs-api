using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Interface for processing group status.
    /// </summary>
    public interface IGroupStatusProcessingService
    {

        /// <summary>
        /// Processes the provided group status request based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="GroupStatusInput"/> to act on, annotated.
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GroupStatusOutput"/></returns>
        public GroupStatusOutput GetGroupStatus(WithCorrelationId<GroupStatusInput> data);

        /// <summary>
        /// Processes the provided user id as a request and generates Available views  based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="AvailableViewInput"/> to act on, annotated. 
        /// with a correlation id.</param>
        /// <returns>The list of.<seealso cref="AvailableViewOutput"/></returns>
        AvailableViewOutput GetAvailableViews(WithCorrelationId<AvailableViewInput> data);

        /// <summary>
        /// Gets the widget data for the group.
        /// </summary>
        /// <param name="data">The <seealso cref="GroupStatusInput"/> to act on, annotated.
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GroupStatusWidgetOutput"/></returns>
        public GroupStatusWidgetOutput GetClassificationWidgetData(WithCorrelationId<GroupStatusInput> data);

        /// <summary>
        /// Gets the widget data for the group.
        /// </summary>
        /// <param name="data">The <seealso cref="GroupStatusInput"/> to act on, annotated.
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GroupStatusKPIOutput"/></returns>
        public Task<GroupStatusKPIOutput> GetAlarmsWidgetDataAsync(WithCorrelationId<GroupStatusInput> data);

        /// <summary>
        /// Gets the downtime data for the wells in the group.
        /// </summary>
        /// <param name="input">The <seealso cref="GroupStatusInput"/> with a correlation id.</param>
        /// <returns>The <seealso cref="GroupStatusKPIOutput"/></returns>
        Task<GroupStatusDowntimeByWellOutput> GetDowntimeByWellsAsync(WithCorrelationId<GroupStatusInput> input);

        /// <summary>
        /// Gets the downtime data for the wells in the group based on the run status.
        /// </summary>
        /// <param name="input">The <seealso cref="GroupStatusInput"/> with a correlation id.</param>
        /// <returns>The <seealso cref="GroupStatusKPIOutput"/></returns>
        Task<GroupStatusKPIOutput> GetDowntimeByRunStatusAsync(WithCorrelationId<GroupStatusInput> input);
    }
}
