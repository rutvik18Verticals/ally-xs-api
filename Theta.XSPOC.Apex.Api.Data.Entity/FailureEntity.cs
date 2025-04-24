using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The tblFailures database table.
    /// </summary>
    [Table("tblFailures")]
    public class FailureEntity
    {

        /// <summary>
        /// The id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// The node id.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// The failure date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        /// <summary>
        /// The identified date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? IdentifiedDate { get; set; }

        /// <summary>
        /// The rig up date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? RigUpDate { get; set; }

        /// <summary>
        /// The rig down date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? RigDownDate { get; set; }

        /// <summary>
        /// The recovery date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? RecoveryDate { get; set; }

        /// <summary>
        /// The component id.
        /// </summary>
        [Column("ComponentID")]
        public int? ComponentId { get; set; }

        /// <summary>
        /// The sub component id.
        /// </summary>
        [Column("SubcomponentID")]
        public int? SubcomponentId { get; set; }

        /// <summary>
        /// The node id.
        /// </summary>
        [Column("ModeID")]
        public int? ModeId { get; set; }

        /// <summary>
        /// The mechanism id.
        /// </summary>
        [Column("MechanismID")]
        public int? MechanismId { get; set; }

        /// <summary>
        /// The total cost.
        /// </summary>
        [Column(TypeName = "money")]
        public decimal? TotalCost { get; set; }

        /// <summary>
        /// The requested service id.
        /// </summary>
        [Column("RequestedServiceID")]
        public int? RequestedServiceId { get; set; }

        /// <summary>
        /// The pull reason id.
        /// </summary>
        [Column("PullReasonID")]
        public int? PullReasonId { get; set; }

        /// <summary>
        /// The notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// The value specifying the record is locked or not.
        /// </summary>
        public bool Locked { get; set; }

    }
}
