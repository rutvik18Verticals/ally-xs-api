using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a trend data response that needs to be send out.
    /// </summary>
    public class AppSettingsResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the dictionary of values.
        /// </summary>
        public Dictionary<string, string> Values { get; set; }

    }
}