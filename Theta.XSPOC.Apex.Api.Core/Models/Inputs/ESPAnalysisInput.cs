using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Represent the class for esp analysis input data.
    /// </summary>
    public class ESPAnalysisInput
    {

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the test date.
        /// </summary>
        public string TestDate { get; set; }

    }
}
