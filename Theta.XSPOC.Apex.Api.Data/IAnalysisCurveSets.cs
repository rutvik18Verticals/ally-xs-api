using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents analysis result curves set.
    /// </summary>
    public interface IAnalysisCurveSets
    {

        /// <summary>
        /// Get the analysis curve set.
        /// </summary>
        /// <param name="analysisResultId"></param>
        /// <param name="analysisResultSourceId"></param>
        /// <param name="curveSetTypeId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        IList<AnalysisCurveSetModel> GetAnalysisCurvesSet(int analysisResultId,
            int analysisResultSourceId, int curveSetTypeId, string correlationId);

        /// <summary>
        /// Get the glr curve set.
        /// </summary>
        /// <param name="curveSetMemberIds"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        IList<GasLiquidRatioCurveAnnotationModel> GetGLRCurveSetAnnotations(IList<int> curveSetMemberIds,
            string correlationId);

    }
}
