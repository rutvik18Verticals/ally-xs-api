using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{

    /// <summary>
    /// Represents the get data payload for post processing.
    /// </summary>
    public class GetDataPayload : PostProcessingBase
    {

        /// <summary>
        /// Gets or sets the log event data items.
        /// </summary>
        public IList<LogEventData> LogEventDataItems { get; set; }

        /// <summary>
        /// Gets or sets the output to save back to the transaction table.
        /// </summary>
        public byte[] Output { get; set; }

    }
}
