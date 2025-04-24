using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The Port master table.
    /// </summary>
    [Table("tblPortMaster")]
    public class PortMaster
    {

        /// <summary>
        /// Gets or sets the port id.
        /// </summary>
        [Column("PortID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short PortId { get; set; }

        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        [MaxLength(20)]
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the comm port.
        /// </summary>
        [Column("CommPort")]
        public short CommPort { get; set; }

        /// <summary>
        /// Gets or sets the baud rate.
        /// </summary>
        [Column("BaudRate")]
        public int? BaudRate { get; set; }

        /// <summary>
        /// Gets or sets the parity.
        /// </summary>
        [Column("Parity")]
        public short Parity { get; set; }

        /// <summary>
        /// Gets or sets the data bits.
        /// </summary>
        [Column("DataBits")]
        public short DataBits { get; set; }

        /// <summary>
        /// Gets or sets the stop bits.
        /// </summary>
        [Column("StopBits")]
        public short StopBits { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        [Column("TimeOut")]
        public short TimeOut { get; set; }

        /// <summary>
        /// Gets or sets the key up delay.
        /// </summary>
        [Column("KeyUPDelay")]
        public short KeyUpDelay { get; set; }

        /// <summary>
        /// Gets or sets the key down delay.
        /// </summary>
        [Column("KeyDownDelay")]
        public short KeyDownDelay { get; set; }

        /// <summary>
        /// Gets or sets the RTSCTS control flag.
        /// </summary>
        [Column("RTSCTSControl")]
        public bool RTSCTSControl { get; set; }

        /// <summary>
        /// Gets or sets the CTS timeout.
        /// </summary>
        [Column("CTSTimeout")]
        public short CTSTimeout { get; set; }

        /// <summary>
        /// Gets or sets the retries.
        /// </summary>
        [Column("Retries")]
        public short Retries { get; set; }

        /// <summary>
        /// Gets or sets the enabled flag.
        /// </summary>
        [Column("Enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Column("Description")]
        [MaxLength(30)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the wait for CTS flag.
        /// </summary>
        [Column("WaitForCTS")]
        public bool WaitForCTS { get; set; }

        /// <summary>
        /// Gets or sets the port type.
        /// </summary>
        [Column("PortType")]
        public int? PortType { get; set; }

        /// <summary>
        /// Gets or sets the turn around delay.
        /// </summary>
        [Column("TurnaroundDelay")]
        public int? TurnaroundDelay { get; set; }

        /// <summary>
        /// Gets or sets the inter char timeout.
        /// </summary>
        [Column("InterCharTimeout")]
        public int? InterCharTimeout { get; set; }

        /// <summary>
        /// Gets or sets the ip hostname.
        /// </summary>
        [Column("ipHostname")]
        [MaxLength(20)]
        public string IpHostname { get; set; }

        /// <summary>
        /// Gets or sets the ip port.
        /// </summary>
        [Column("ipPort")]
        public int? IpPort { get; set; }

        /// <summary>
        /// Gets or sets the suspend date.
        /// </summary>
        [Column("SuspendDate")]
        public DateTime? SuspendDate { get; set; }

        /// <summary>
        /// Gets or sets the contact list id.
        /// </summary>
        [Column("ContactListID")]
        public int? ContactListId { get; set; }

        /// <summary>
        /// Gets or sets the consecutive comm fails.
        /// </summary>
        [Column("CommConsecFails")]
        public int? ConsecutiveCommFails { get; set; }

    }
}