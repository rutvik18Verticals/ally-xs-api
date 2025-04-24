using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// The flow control device statuses
    /// </summary>
    public class FlowControlDeviceStatuses
    {

        /// <summary>
        /// Gets or sets the status of each valve
        /// </summary>
        public IList<ValveStatus> ValveStatuses { get; set; }

        /// <summary>
        /// Gets or sets the status of the orifice ( if orifice is present )
        /// </summary>
        public OrificeStatus OrificeStatus { get; set; }

        /// <summary>
        /// Gets or sets whether we are injecting gas below the tubing.
        /// </summary>
        public bool InjectingGasBelowTubing { get; set; }

    }
}
