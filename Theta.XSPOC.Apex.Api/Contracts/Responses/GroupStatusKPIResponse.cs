using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{

    /// <summary>
    /// Represents the response for Group Status KPI.
    /// </summary>
    public class GroupStatusKPIResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the Values.
        /// </summary>
        public List<GroupStatusKPIValue> Values { get; set; }

    }
}
