using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The GL Well Details database table.
    /// </summary>
    [Table("tblGLWellDetails")]
    public class GLWellDetailEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Key]
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the gas injection depth.
        /// </summary>
        public float? GasInjectionDepth { get; set; }

        /// <summary>
        /// Gets or sets the gas injection pressure.
        /// </summary>
        public float? GasInjectionPressure { get; set; }

        /// <summary>
        /// Gets or sets the formation gas oil ratio.
        /// </summary>
        public float? FormationGasOilRatio { get; set; }

        /// <summary>
        /// Gets or sets the percent H2S.
        /// </summary>
        [Column("PercentH2S")]
        public float? PercentH2S { get; set; }

        /// <summary>
        /// Gets or sets the percent N2.
        /// </summary>
        public float? PercentN2 { get; set; }

        /// <summary>
        /// Gets or sets the percent co2.
        /// </summary>
        [Column("PercentCO2")]
        public float? PercentCO2 { get; set; }

        /// <summary>
        /// Gets or sets the injected gas specific gravity.
        /// </summary>
        public float? InjectedGasSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the value indicating injecting below tubing is set.
        /// </summary>
        public bool InjectingBelowTubing { get; set; }

        /// <summary>
        /// Gets or sets the estimate injection depth.
        /// </summary>
        public bool EstimateInjectionDepth { get; set; }

        /// <summary>
        /// Gets or sets the valve configuration option.
        /// </summary>
        public int ValveConfigurationOption { get; set; }

        /// <summary>
        /// Gets or sets the downhole gage depth.
        /// </summary>
        public float? DownholeGageDepth { get; set; }

        /// <summary>
        /// Gets or sets the downhole gage in analysis.
        /// </summary>
        public bool UseDownholeGageInAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the compression cost.
        /// </summary>
        public float? CompressionCost { get; set; }

        /// <summary>
        /// Gets or sets the injection cost.
        /// </summary>
        public float? InjectionCost { get; set; }

        /// <summary>
        /// Gets or sets the separation cost.
        /// </summary>
        public float? SeparationCost { get; set; }

        /// <summary>
        /// Gets or sets the cost type id.
        /// </summary>
        public int? CostTypeId { get; set; }

    }
}
