using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Represent the class for curve coordinates input data.
    /// </summary>
    public class CurveCoordinatesInput
    {

        /// <summary>
        /// Gets or sets the asset GUID.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public string TestDate { get; set; }

    }
}
