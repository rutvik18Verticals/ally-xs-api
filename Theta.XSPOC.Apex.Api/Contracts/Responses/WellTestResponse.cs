using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a WellTestResponse value that needs to be send out.
    /// </summary>
    public class WellTestResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<WellTestData> Values { get; set; }

    }
}
