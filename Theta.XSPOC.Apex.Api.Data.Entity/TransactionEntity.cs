using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The transaction table.
    /// </summary>
    [Table("tblTransactions")]
    public class TransactionEntity
    {

        /// <summary>
        /// Gets or sets the date and time when the transaction was requested.
        /// </summary>
        [Column("DateRequest")]
        public DateTime DateRequested { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the transaction was processed.
        /// </summary>
        [Column("DateProcess")]
        public DateTime? DateProcessed { get; set; }

        /// <summary>
        /// Gets or sets the input used to build requests. This is a binary value that can
        /// contain any number of values.
        /// </summary>
        [Column("Input")]
        public byte[] Input { get; set; }

        /// <summary>
        /// Gets or sets the input text used to build requests.
        /// // todo find out from terry the format.
        /// </summary>
        [Column("InputText", TypeName = "nvarchar")]
        [MaxLength(250)]
        public string InputText { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the output used as the request of the request. This is a binary value that can
        /// contain any number of values.
        /// </summary>
        [Column("Output")]
        public byte[] Output { get; set; }

        /// <summary>
        /// Gets or sets the port id.
        /// </summary>
        [Column("PortID")]
        public int PortId { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        [Column("Priority")]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the source (task or username).
        /// </summary>
        [Column("Source", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        [Column("Result", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Result { get; set; }

        /// <summary>
        /// Gets or set the task to process.
        /// </summary>
        [Column("Task", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string Task { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        [Column("TransactionID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the number of times the transaction has tried to process.
        /// </summary>
        [Column("Tries")]
        public int? Tries { get; set; }

        /// <summary>
        /// Gets or sets the transaction's associated correlation id
        /// </summary>
        [Column("CorrelationId")]
        public Guid? CorrelationId { get; set; }

    }
}
