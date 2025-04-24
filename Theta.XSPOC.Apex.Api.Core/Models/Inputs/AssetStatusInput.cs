using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// This class represents the asset status input data.
    /// </summary>
    public class AssetStatusInput
    {

        /// <summary>
        /// Gets or sets the asset id represented as a GUID.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the customer id represented as a GUID.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the user id of the user who making requesting.
        /// </summary>
        public string UserId { get; set; }

    }
}
