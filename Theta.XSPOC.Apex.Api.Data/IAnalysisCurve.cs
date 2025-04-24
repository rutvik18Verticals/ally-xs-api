using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents analysis result curves.
    /// </summary>
    public interface IAnalysisCurve
    {

        /// <summary>
        /// Get the curve set analysis result curves by analysis result id.
        /// </summary>
        /// <param name="analysisResultId">The analysis result id.</param>
        /// <param name="applicationTypeId">The application type id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="AnalysisCurveModel"/>.</returns>
        IList<AnalysisCurveModel> GetAnalysisResultCurves(int analysisResultId, int applicationTypeId, string correlationId);

        /// <summary>
        /// Get the Analysis Curve Model.
        /// </summary>
        /// <param name="analysisResultId"></param>
        /// <param name="applicationTypeId"></param>
        /// <param name="curveTypeIds"></param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="AnalysisCurveModel"/>.</returns>
        IList<AnalysisCurveModel> Fetch(int analysisResultId, int applicationTypeId,
            IEnumerable<int> curveTypeIds, string correlationId);

    }
}
