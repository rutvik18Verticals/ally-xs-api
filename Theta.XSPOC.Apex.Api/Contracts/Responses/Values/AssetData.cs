using System;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{

    /// <summary>
    /// Represents a response asset data model.
    /// </summary>
    public class AssetData
    {

        /// <summary>
        /// The asset GUID.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// The asset name.
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// The Industry Application Id.
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
