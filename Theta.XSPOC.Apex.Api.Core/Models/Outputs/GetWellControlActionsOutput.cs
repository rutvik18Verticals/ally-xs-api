using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents a control action performable on a well.
    /// </summary>
    public class GetWellControlActionsOutput : CoreOutputBase
    {

        /// <summary>
        /// The well control actions.
        /// </summary>
        public IList<WellControlAction> WellControlActions { get; set; }

        /// <summary>
        /// Indicates whether the user can config the well, used to include enable/disable well.
        /// </summary>
        public bool CanConfigWell { get; set; }

    }
}
