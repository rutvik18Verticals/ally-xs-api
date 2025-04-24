using System;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a base response class that needs to be sent out.
    /// </summary>
    public class ResponseBase
    {

        /// <summary>
        /// Gets or sets the response id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the response date and time.
        /// </summary>
        public DateTime DateCreated { get; set; }

    }
}
