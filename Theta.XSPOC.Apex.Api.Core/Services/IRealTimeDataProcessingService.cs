using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Service that handles processing of real time trend data.
    /// </summary>
    public interface IRealTimeDataProcessingService
    {
        /// <summary>
        /// Processes the Data History Time Series Trend Data Items.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{TimeSeriesTrendDataInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="TimeSeriesTrendDataOutput"/>.</returns>
        Task<TimeSeriesOutput> GetAllyTimeSeriesTrendDataAsync(WithCorrelationId<TimeSeriesInput> input);

        /// <summary>
        /// Processes the Data History Time Series Trend Data Items.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{TimeSeriesTrendDataInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="TimeSeriesTrendDataOutput"/>.</returns>
        Task<TimeSeriesOutput> GetTimeSeriesDataAsync(WithCorrelationId<TimeSeriesInput> input);

        /// <summary>
        /// Processes the request to get assets by customerid.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{AssetByCustomerInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="AssetDetailsOutput"/>.</returns>
        Task<AssetDetailsOutput> GetAssetsByCustomerIdAsync(WithCorrelationId<AssetByCustomerInput> input);

        /// <summary>
        /// Processes the request to get all parameter standard types
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{AssetByCustomerInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="ParameterStandardTypeOutput"/>.</returns>
        Task<ParameterStandardTypeOutput> GetAllParameterStandardTypesAsync(WithCorrelationId<string> input);

        /// <summary>
        /// Processes the request to validate customer token
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{ValidateCustomerInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="ValidateCustomerOutput"/>.</returns>
        Task<ValidateCustomerOutput> ValidateCustomerAsync(WithCorrelationId<ValidateCustomerInput> input);
    }
}
