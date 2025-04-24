using System;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// The data contract for processing well control requests.
    /// </summary>
    public class ProcessDeviceOperationData
    {

        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        public Action ActionType { get; set; }

        /// <summary>
        /// Gets or sets the communication status.
        /// </summary>
        public string CommunicationStatus { get; set; }

        /// <summary>
        /// Gets or sets the device id.
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the payload type.
        /// </summary>
        public PayloadType PayloadType { get; set; }

        /// <summary>
        /// Get or sets the payload. This is a JSON string representation.
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// Gets or sets the date time the action was performed in utc time.
        /// </summary>
        public DateTime OperationUtcDate { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the consecutive communication fails count.
        /// </summary>
        public int ConsecutiveCommFails { get; set; }

        /// <summary>
        /// Gets or sets the port id.
        /// </summary>
        public int PortId { get; set; }

        /// <summary>
        /// Gets or sets the number of tries for the transaction.
        /// </summary>
        public int TransactionTries { get; set; }

        /// <summary>
        /// Gets or sets the response meta data in case of web-api calls.
        /// </summary>
        public string ResponseMetadata { get; set; }

    }
}
