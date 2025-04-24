using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Setpoint.Contracts.Requests
{
    /// <summary>
    /// Represent the class for put request  input data.
    /// </summary>
    public class PutRequest
    {

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the address value.
        /// </summary>
        public IDictionary<string, string> AddressValues { get; set; }

    }
}
