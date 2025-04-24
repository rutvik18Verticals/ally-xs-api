using System;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Ports
{
    /// <summary>
    /// Represents a port master.
    /// </summary>
    public class Ports : DocumentBase
    {

        /// <summary>
        /// Gets or sets the port ID.
        /// </summary>
        public int PortID { get; set; }

        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the communication port.
        /// </summary>
        public int CommPort { get; set; }

        /// <summary>
        /// Gets or sets the baud rate.
        /// </summary>
        public int? BaudRate { get; set; }

        /// <summary>
        /// Gets or sets the parity.
        /// </summary>
        public int Parity { get; set; }

        /// <summary>
        /// Gets or sets the data bits.
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// Gets or sets the stop bits.
        /// </summary>
        public int StopBits { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// Gets or sets the key up delay.
        /// </summary>
        public int KeyUPDelay { get; set; }

        /// <summary>
        /// Gets or sets the key down delay.
        /// </summary>
        public int KeyDownDelay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether RTS/CTS control is enabled.
        /// </summary>
        public bool RTSCTSControl { get; set; }

        /// <summary>
        /// Gets or sets the CTS timeout.
        /// </summary>
        public int CTSTimeout { get; set; }

        /// <summary>
        /// Gets or sets the number of retries.
        /// </summary>
        public int Retries { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the port is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to wait for CTS.
        /// </summary>
        public bool WaitForCTS { get; set; }

        /// <summary>
        /// Gets or sets the port type.
        /// </summary>
        public int? PortType { get; set; }

        /// <summary>
        /// Gets or sets the turnaround delay.
        /// </summary>
        public int? TurnaroundDelay { get; set; }

        /// <summary>
        /// Gets or sets the inter-character timeout.
        /// </summary>
        public int? InterCharTimeout { get; set; }

        /// <summary>
        /// Gets or sets the IP hostname.
        /// </summary>
        public string IpHostname { get; set; }

        /// <summary>
        /// Gets or sets the IP port.
        /// </summary>
        public int? IpPort { get; set; }

        /// <summary>
        /// Gets or sets the suspend date.
        /// </summary>
        public DateTime? SuspendDate { get; set; }

        /// <summary>
        /// Gets or sets the contact list ID.
        /// </summary>
        public int? ContactListID { get; set; }

        /// <summary>
        /// Gets or sets the consecutive communication failures.
        /// </summary>
        public int? CommConsecFails { get; set; }

    }
}
