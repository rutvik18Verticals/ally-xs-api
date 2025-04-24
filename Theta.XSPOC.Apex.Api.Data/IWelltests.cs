using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents Well Tests.
    /// </summary>
    public interface IWellTests
    {

        /// <summary>
        /// Get the Well Tests data by asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId">The correlation GUID.</param>
        /// <returns>The <seealso cref="IList{WellTestModel}"/>.</returns>
        IList<WellTestModel> GetESPWellTestsData(Guid assetId, string correlationId);

        /// <summary>
        /// Get the Well Tests data by asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="analysisTypeSensitivityKey">analysis Type Sensitivity Key.</param>
        /// <param name="analysisTypeWellTest">The Analysis Type Well Test.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{GLAnalysisResultModel}"/>.</returns>
        IList<GLAnalysisResultModel> GetGLAnalysisWellTestsData(Guid assetId, int analysisTypeSensitivityKey,
            int analysisTypeWellTest, string correlationId);

    }
}
