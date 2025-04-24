namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the curve entity model data model.
    /// </summary>
    public class CurveEntityModel
    {

        /// <summary>
        /// Gets or sets the analysis result curve id.
        /// </summary>
        public int AnalysisResultCurveId { get; set; }

        /// <summary>
        /// Gets or sets the analysis result id.
        /// </summary>
        public int AnalysisResultId { get; set; }

        /// <summary>
        /// Gets or sets the curve type id.
        /// </summary>
        public int CurveTypeId { get; set; }

        /// <summary>
        /// Gets or sets the curve coordinate  id.
        /// </summary>
        public int? CurveCoordinateId { get; set; }

        /// <summary>
        /// Gets or sets the curve coordinate  x.
        /// </summary>
        public double? CurveCoordinateX { get; set; }

        /// <summary>
        /// Gets or sets the curve coordinate  y.
        /// </summary>
        public double? CurveCoordinateY { get; set; }

    }
}
