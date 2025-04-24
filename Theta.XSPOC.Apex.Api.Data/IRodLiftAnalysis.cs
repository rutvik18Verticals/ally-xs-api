using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents rod lift analysis.
    /// </summary>
    public interface IRodLiftAnalysis
    {

        /// <summary>
        /// Get the rod left analysis data by asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="cardDate">The card date.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="RodLiftAnalysisResponse"/>.</returns>
        RodLiftAnalysisResponse GetRodLiftAnalysisData(Guid assetId, string cardDate,
            string correlationId);

        /// <summary>
        /// Get the card date by asset id.
        /// </summary>
        /// <param name="assetId">The asset id/node id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="IList{CardDateModel}"/>.</returns>
        IList<CardDateModel> GetCardDatesByAssetId(Guid assetId, string correlationId);

    }
}
