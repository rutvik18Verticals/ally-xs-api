using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Represent the class for rod lift analysis input data.
    /// </summary>
    public class RodLiftAnalysisInput
    {

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the card date.
        /// </summary>
        public string CardDate { get; set; }

    }
}
