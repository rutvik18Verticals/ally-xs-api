using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the class for IPR analysis curve coordinate response.
    /// </summary>
    public class IPRAnalysisCurveCoordinateModel
    {

        /// <summary>
        /// The node master data object.
        /// </summary>
        public NodeMasterModel NodeMasterData { get; set; }

        /// <summary>
        /// The IPR Analysis Result object. 
        /// </summary>
        public IPRAnalysisResultModel IPRAnalysisResultEntity { get; set; }

        /// <summary>
        /// The ESP Analysis Result object. 
        /// </summary>
        public IList<AnalysisResultCurvesModel> AnalysisResultCurvesEntities { get; set; }

    }
}
