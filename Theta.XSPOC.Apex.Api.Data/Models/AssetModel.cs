using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the class for asset data.
    /// </summary>
    public class AssetModel
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
        /// Gets or sets the industry application id.
        /// </summary>
        public int? IndustryApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the well op type.
        /// </summary>
        public int? WellOpType { get; set; }

        /// <summary>
        /// Gets or sets the enabled state.
        /// </summary>
        public bool Enabled { get; set; }

    }
}