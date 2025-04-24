using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Represent the class for gl analysis input data.
    /// </summary>
    public class GLAnalysisInput
    {

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the test date.
        /// </summary>
        public string TestDate { get; set; }

        /// <summary>
        /// Gets or sets the analysis type id.
        /// </summary>
        public int? AnalysisTypeId { get; set; }

        /// <summary>
        /// Gets or sets the analysis result id.
        /// </summary>
        public int? AnalysisResultId { get; set; }

    }
}
