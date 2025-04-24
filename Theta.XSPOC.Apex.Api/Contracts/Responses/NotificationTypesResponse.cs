using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a notification response that needs to be send out.
    /// </summary>
    public class NotificationTypesResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<NotificationTypesData> Values { get; set; }

    }
}
