using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The IPR Analysis Results table.
    /// </summary>
    [Table("tblIPRAnalysisResults")]
    public partial class IPRAnalysisResultsEntity
    {

        /// <summary>
        /// Gets or sets the auto generated id of the table.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets node id.
        /// </summary>
        [Required]
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets test date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Gets or sets static bottom hole pressure.
        /// </summary>
        public float? StaticBottomholePressure { get; set; }

        /// <summary>
        /// Gets or sets bubble point pressure.
        /// </summary>
        public float? BubblepointPressure { get; set; }

        /// <summary>
        /// Gets or sets flowing bottom hole pressure.
        /// </summary>
        public float? FlowingBottomholePressure { get; set; }

        /// <summary>
        /// Gets or sets gross rate.
        /// </summary>
        public float? GrossRate { get; set; }

        /// <summary>
        /// Gets or sets water rate.
        /// </summary>
        public float? WaterRate { get; set; }

        /// <summary>
        /// Gets or sets water cut rate.
        /// </summary>
        public float? WaterCut { get; set; }

        /// <summary>
        /// Gets or sets the ipr slope.
        /// </summary>
        [Column("IPRSlope")]
        public float? IPRSlope { get; set; }

        /// <summary>
        /// Gets or sets the productivity index.
        /// </summary>
        public float? ProductivityIndex { get; set; }

        /// <summary>
        /// Gets or sets rate at bubble point.
        /// </summary>
        public float? RateAtBubblePoint { get; set; }

        /// <summary>
        /// Gets or sets rate at max oil.
        /// </summary>
        public float? RateAtMaxOil { get; set; }

        /// <summary>
        /// Gets or sets rate at max liquid.
        /// </summary>
        public float? RateAtMaxLiquid { get; set; }

        /// <summary>
        /// Gets or sets analysis correlation id.
        /// </summary>
        [Column("AnalysisCorrelationID")]
        public int? AnalysisCorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the fbhp calculation used defaults.
        /// </summary>
        [Column("FBHPCalculationUsedDefaults")]
        public bool FBHPCalculationUsedDefaults { get; set; }

        /// <summary>
        /// Gets or sets the fbhp calculation result.
        /// </summary>
        [Column("FBHPCalculationResult")]
        public string FBHPCalculationResult { get; set; }

        /// <summary>
        /// Gets or sets the sbhp calculation result.
        /// </summary>
        [Column("SBHPCalculationResult")]
        public string SBHPCalculationResult { get; set; }

        /// <summary>
        /// Gets or sets the bubble point pressure source.
        /// </summary>
        public int? BubblepointPressureSource { get; set; }

        /// <summary>
        /// Gets or sets the fbhp calculation used measured depth.
        /// </summary>
        [Column("FBHPCalculationUsedMeasuredDepth")]
        public bool? FBHPCalculationUsedMeasuredDepth { get; set; }

    }
}
