using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the class for asset data.
    /// </summary>
    public class AssetData
    {

        /// <summary>
        /// Gets or sets the asset GUID.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the asset name.
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// Gets or sets the Industry Application Id.
        /// </summary>
        public int? IndustryApplicationId { get; set; }

    }
}
