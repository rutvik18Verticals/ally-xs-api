using System;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents esp analysis.
    /// </summary>
    public interface IESPAnalysis
    {

        /// <summary>
        /// Get the esp analysis data by asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="testDate">The card date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ESPAnalysisResponse"/>.</returns>
        ESPAnalysisResponse GetESPAnalysisData(Guid assetId, string testDate, string correlationId);

        /// <summary>
        /// Get the esp analysis result by node id and test date.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="testDate">The test date.</param>
        /// <param name="correlationId"></param>
        /// <returns>
        /// The <seealso cref="ESPAnalysisResultModel"/>.
        /// </returns>
        ESPAnalysisResultModel GetESPAnalysisResult(string nodeId, DateTime testDate, string correlationId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="testDate"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        ESPPressureProfileModel GetESPPressureProfileData(Guid assetId, DateTime testDate, string correlationId);

    }
}
