namespace Theta.XSPOC.Apex.Api.WellControl.Contracts.Responses
{
    /// <summary>
    /// Represents the broadcast message response from well control actions.
    /// </summary>
    public class WellControlResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the payload data.
        /// </summary>
        public string Payload { get; set; }

    }
}
