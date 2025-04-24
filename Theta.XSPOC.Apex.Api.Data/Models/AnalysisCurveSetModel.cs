namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the analysis curve set model.
    /// </summary>
    public class AnalysisCurveSetModel
    {

        /// <summary>
        /// The collection list of curve set coordinates.
        /// </summary>
        public CurveSetCoordinatesModel CurveSetCoordinatesModels { get; set; }

        /// <summary>
        /// The collection list of analysis curve set member.
        /// </summary>
        public AnalysisCurveSetMemberModel AnalysisCurveSetMemberModels { get; set; }

        /// <summary>
        /// The collection list of analysis curve set.
        /// </summary>
        public AnalysisCurveSetDataModel AnalysisCurveSetDataModels { get; set; }

    }
}
