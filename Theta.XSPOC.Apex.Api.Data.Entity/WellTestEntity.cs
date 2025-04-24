using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The well test history table.
    /// </summary>
    [Table("tblWellTests")]
    public class WellTestEntity
    {

        #region Public Properties

        /// <summary>
        /// Gets or sets whether the well test is approved.
        /// </summary>
        public bool? Approved { get; set; }

        /// <summary>
        /// Gets or sets the gas rate.
        /// </summary>
        [Column("GasRate")]
        public float? GasRate { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the well test date.
        /// </summary>
        [Column("TestDate")]
        [Required]
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Gets or sets the oil rate.
        /// </summary>
        [Column("OilRate")]
        public float? OilRate { get; set; }

        /// <summary>
        /// Gets or sets the water rate.
        /// </summary>
        [Column("WaterRate")]
        public float? WaterRate { get; set; }

        /// <summary>
        /// Gets or sets the water rate.
        /// </summary>
        [Column("FluidAbovePump")]
        public float? FluidAbovePump { get; set; }

        /// <summary>
        /// Gets or sets the water rate.
        /// </summary>
        [Column("Duration")]
        public float? Duration { get; set; }

        #endregion

    }
}
