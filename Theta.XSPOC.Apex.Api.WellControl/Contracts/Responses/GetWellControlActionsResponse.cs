using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.WellControl.Contracts.Responses
{
    /// <summary>
    /// Represents the response for getting well control actions.
    /// </summary>
    public class GetWellControlActionsResponse
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
