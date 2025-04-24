using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{

    /// <summary>
    /// Represents the GlAnalysisSurveyCurveCoordinate Data model.
    /// </summary>
    public class GlAnalysisSurveyCurveCoordinateDataModel
    {

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the NodeId.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the SurveyDate.
        /// </summary>
        public DateTime SurveyDate { get; set; }

    }
}
