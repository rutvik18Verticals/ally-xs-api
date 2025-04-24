namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The represents analysis curve set data model.
    /// </summary>
    public class AnalysisCurveSetDataModel
    {

        /// <summary>
        /// The analysis result source id.
        /// </summary>
        public int AnalysisResultSourceId { get; set; }

        /// <summary>
        /// The analysis result id.
        /// </summary>
        public int AnalysisResultId { get; set; }

        /// <summary>
        /// The curve set id.
        /// </summary>
        public int CurveSetId { get; set; }

        /// <summary>
        /// The curve set type id.
        /// </summary>
        public int CurveSetTypeId { get; set; }

    }
}
