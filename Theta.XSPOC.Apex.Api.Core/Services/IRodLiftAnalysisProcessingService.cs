using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Interface for processing rod lift analysis.
    /// </summary>
    public interface IRodLiftAnalysisProcessingService
    {

        /// <summary>
        /// Processes the provided rod lift analysis request and generates response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="RodLiftAnalysisInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="RodLiftAnalysisDataOutput"/></returns>
        Task<RodLiftAnalysisDataOutput> GetRodLiftAnalysisResultsAsync(WithCorrelationId<RodLiftAnalysisInput> data);

        /// <summary>
        /// Processes the provided card date request and generates card date based on that data.
        /// </summary>
        /// <param name = "requestWithCorrelationId" > The <seealso cref="CardDateInput"/> to act on, annotated
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="CardDatesOutput"/>.</returns>
        CardDatesOutput GetCardDate(WithCorrelationId<CardDateInput> requestWithCorrelationId);

        /// <summary>
        /// Processes the provided card coordinate request and generates card coordinates based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="CardCoordinateInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The list of <seealso cref="CardCoordinateDataOutput"/></returns>
        CardCoordinateDataOutput GetCardCoordinateResults(WithCorrelationId<CardCoordinateInput> data);

    }
}
