using System;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Interface for processing esp analysis.
    /// </summary>
    public interface IESPAnalysisProcessingService
    {

        /// <summary>
        /// Processes the provided esp analysis request and generates response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="ESPAnalysisInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="ESPAnalysisDataOutput"/></returns>
        ESPAnalysisDataOutput GetESPAnalysisResults(WithCorrelationId<ESPAnalysisInput> data);

        /// <summary>
        /// Processes the provided curve coordinate request and generates curve coordinates based on that data.
        /// </summary>
        /// <param name="data">
        /// The <seealso cref="CurveCoordinatesInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="CurveCoordinateDataOutput"/></returns>
        CurveCoordinateDataOutput GetCurveCoordinate(WithCorrelationId<CurveCoordinatesInput> data);

        /// <summary>
        /// Gets the pressure profile data.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="testDate">The test date.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <see cref="ESPPressureProfileData"/>.</returns>
        ESPPressureProfileData GetPressureProfile(Guid assetId, DateTime testDate, string correlationId);

    }
}
