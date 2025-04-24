using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Represents well action data for a well control action.
    /// </summary>
    public class WellActionData : WellControlPayloadBase
    {

        /// <summary>
        /// Gets or sets the log event data list.
        /// </summary>
        public IList<LogEventData> LogEventDataList { get; set; }

        /// <summary>
        /// Gets or sets the process reset facility tags flag.
        /// </summary>
        public bool ProcessResetFacilityTags { get; set; }

    }
}
