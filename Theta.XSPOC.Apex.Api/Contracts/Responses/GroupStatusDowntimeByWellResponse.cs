using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{

    /// <summary>
    /// Represents the response for group status downtime by well.
    /// </summary>
    public class GroupStatusDowntimeByWellResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the downtime by well values.
        /// </summary>
        public GroupStatusDowntimeByWellValue Values { get; set; }

    }
}
