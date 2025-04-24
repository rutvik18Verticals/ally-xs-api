using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Well control payload base to provide <seealso cref="DeviceScanData"/>.
    /// </summary>
    public class WellControlPayloadBase
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the data that should be saved to saved parameters. The dictionary key is
        /// the address and the value is the value to save.
        /// </summary>
        public IList<IDictionary<int, float>> DataToSave { get; set; }

        /// <summary>
        /// Gets or sets the device scan data.
        /// </summary>
        public DeviceScanData DeviceScanData { get; set; }

    }
}
