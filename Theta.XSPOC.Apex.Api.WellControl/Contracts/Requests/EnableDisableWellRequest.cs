namespace Theta.XSPOC.Apex.Api.WellControl.Contracts.Requests
{
    /// <summary>
    /// Represents the necessary inputs for processing a enable/disable well request.
    /// </summary>
    public class EnableDisableWellRequest
    {
        /// <summary>
        /// Gets or sets the asset guid.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the Enabled value.
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// Gets or sets the DataCollection value.
        /// </summary>
        public string DataCollection { get; set; }

        /// <summary>
        /// Gets or sets the DisableCode value.
        /// </summary>
        public string DisableCode { get; set; }

        /// <summary>
        /// Gets or sets the socket id.
        /// </summary>
        public string SocketId { get; set; }

    }
}
