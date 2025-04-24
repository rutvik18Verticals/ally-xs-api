using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The events table.
    /// </summary>
    [Table("tblEvents")]
    public class EventsEntity
    {

        /// <summary>
        /// Gets or sets the event id.
        /// </summary>
        [Column("EventID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the event type id.
        /// </summary>
        [Column("EventTypeID")]
        public int EventTypeId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [Column("Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Column("Description")]
        [MaxLength(50)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [Column("Status")]
        [MaxLength(50)]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the Note.
        /// </summary>
        [Column("Note", TypeName = "text")]
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Column("UserID")]
        [MaxLength(50)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Column("TransactionID")]
        public int? TransactionId { get; set; }

    }
}
