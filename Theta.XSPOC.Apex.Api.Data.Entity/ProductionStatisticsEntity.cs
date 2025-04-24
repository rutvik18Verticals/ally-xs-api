using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The production statistics table.
    /// </summary>
    [Table("tblProductionStatistics")]
    public class ProductionStatisticsEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the processed date.
        /// </summary>
        [Column("ProcessedDate", TypeName = "datetime")]
        public DateTime ProcessedDate { get; set; }

        /// <summary>
        /// Gets or sets the latest test date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? LatestTestDate { get; set; }

        /// <summary>
        /// Gets or sets the latest production boe.
        /// </summary>
        [Column("LatestProductionBOE")]
        public float? LatestProduction { get; set; }

        /// <summary>
        /// Gets or sets the peak production.
        /// </summary>
        [Column("PeakProductionBOE")]
        public float? PeakProduction { get; set; }

        /// <summary>
        /// Gets or sets the down production.
        /// </summary>
        [Column("DownProductionBOE")]
        public float? DownProduction { get; set; }

    }
}
