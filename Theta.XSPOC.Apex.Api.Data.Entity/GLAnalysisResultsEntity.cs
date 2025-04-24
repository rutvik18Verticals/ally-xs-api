using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The GL Analysis Results database table.
    /// </summary>
    [Table("tblGLAnalysisResults")]
    public class GLAnalysisResultsEntity
    {

        /// <summary>
        /// Gets or sets the gl analysis results id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the test date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Gets or sets the procesed date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime ProcessedDate { get; set; }

        /// <summary>
        /// Gets or sets the value indicating success or not.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the gas injection depth.
        /// </summary>
        public float? GasInjectionDepth { get; set; }

        /// <summary>
        /// Gets or sets the measured well depth.
        /// </summary>
        public float? MeasuredWellDepth { get; set; }

        /// <summary>
        /// Gets or sets the oil rate.
        /// </summary>
        public float? OilRate { get; set; }

        /// <summary>
        /// Gets or sets the water rate.
        /// </summary>
        public float? WaterRate { get; set; }

        /// <summary>
        /// Gets or sets the gas rate.
        /// </summary>
        public float? GasRate { get; set; }

        /// <summary>
        /// Gets or sets the well head pressure.
        /// </summary>
        public float? WellheadPressure { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        public float? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the water cut.
        /// </summary>
        public int? WaterCut { get; set; }

        /// <summary>
        /// Gets or sets the gas specific gravity.
        /// </summary>
        public float? GasSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the water specific gravity.
        /// </summary>
        public float? WaterSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the well head temperature.
        /// </summary>
        public float? WellheadTemperature { get; set; }

        /// <summary>
        /// Gets or sets the bottom hole temperature.
        /// </summary>
        public float? BottomholeTemperature { get; set; }

        /// <summary>
        /// Gets or sets the oil specific gravity.
        /// </summary>
        public float? OilSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the casing id.
        /// </summary>
        [Column("CasingID")]
        public float? CasingId { get; set; }

        /// <summary>
        /// The tubing id.
        /// </summary>
        [Column("TubingID")]
        public float? TubingId { get; set; }

        /// <summary>
        /// Gets or sets the tubing outer diameter.
        /// </summary>
        [Column("TubingOD")]
        public float? TubingOD { get; set; }

        /// <summary>
        /// Gets or sets the reservoir pressure.
        /// </summary>
        public float? ReservoirPressure { get; set; }

        /// <summary>
        /// Gets or sets the bubble point pressure.
        /// </summary>
        public float? BubblepointPressure { get; set; }

        /// <summary>
        /// Gets or sets the productive index.
        /// </summary>
        public float? ProductivityIndex { get; set; }

        /// <summary>
        /// Gets or sets the rate at bubble point.
        /// </summary>
        public float? RateAtBubblePoint { get; set; }

        /// <summary>
        /// Gets or sets the rate at max oil.
        /// </summary>
        public float? RateAtMaxOil { get; set; }

        /// <summary>
        /// Gets or sets the rate at max liquid.
        /// </summary>
        public float? RateAtMaxLiquid { get; set; }

        /// <summary>
        /// Gets or sets the IPR slope.
        /// </summary>
        [Column("IPRSlope")]
        public float? IPRSlope { get; set; }

        /// <summary>
        /// Gets or sets the blowing bhp.
        /// </summary>
        [Column("FlowingBHP")]
        public float? FlowingBhp { get; set; }

        /// <summary>
        /// Gets or sets the minimum flowing blowing bhp.
        /// </summary>
        [Column("MinimumFBHP")]
        public float? MinimumFbhp { get; set; }

        /// <summary>
        /// Gets or sets the injected gas liquid ratio.
        /// </summary>
        [Column("InjectedGLR")]
        public float? InjectedGLR { get; set; }

        /// <summary>
        /// Gets or sets the injected gas rate.
        /// </summary>
        public float? InjectedGasRate { get; set; }

        /// <summary>
        /// Gets or sets the max liquid rate.
        /// </summary>
        public float? MaxLiquidRate { get; set; }

        /// <summary>
        /// Gets or sets the injected rate for max liquid rate.
        /// </summary>
        public float? InjectionRateForMaxLiquidRate { get; set; }

        /// <summary>
        /// Gets or sets the glr for max liquid rate.
        /// </summary>
        [Column("GLRForMaxLiquidRate")]
        public float? GLRForMaxLiquidRate { get; set; }

        /// <summary>
        /// Gets or sets the optimum liquid rate.
        /// </summary>
        public float? OptimumLiquidRate { get; set; }

        /// <summary>
        /// Gets or sets the injected rate for optimum liquid rate.
        /// </summary>
        public float? InjectionRateForOptimumLiquidRate { get; set; }

        /// <summary>
        /// Gets or sets the glr for optimum liquid rate.
        /// </summary>
        [Column("GLRForOptimumLiquidRate")]
        public float? GlrforOptimumLiquidRate { get; set; }

        /// <summary>
        /// Gets or sets the flow correlation id.
        /// </summary>
        [Column("FlowCorrelationID")]
        public int FlowCorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the oil viscosity correlation id.
        /// </summary>
        [Column("OilViscosityCorrelationID")]
        public int OilViscosityCorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the oil formation volume factor correlation id.
        /// </summary>
        [Column("OilFormationVolumeFactorCorrelationID")]
        public int OilFormationVolumeFactorCorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the solution gas oil ratio correlationid.
        /// </summary>
        [Column("SolutionGasOilRatioCorrelationID")]
        public int SolutionGasOilRatioCorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure source.
        /// </summary>
        public int? TubingPressureSource { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure source.
        /// </summary>
        public int? CasingPressureSource { get; set; }

        /// <summary>
        /// Gets or sets the co2 percent.
        /// </summary>
        [Column("PercentCO2")]
        public float? PercentCO2 { get; set; }

        /// <summary>
        /// Gets or sets the n2 percent.
        /// </summary>
        public float? PercentN2 { get; set; }

        /// <summary>
        /// Gets or sets the h2s percent.
        /// </summary>
        [Column("PercentH2S")]
        public float? PercentH2S { get; set; }

        /// <summary>
        /// Gets or sets the formation gas oil ratio.
        /// </summary>
        [Column("FormationGOR")]
        public float? FormationGor { get; set; }

        /// <summary>
        /// Gets or sets the kill fluid level.
        /// </summary>
        public float? KillFluidLevel { get; set; }

        /// <summary>
        /// Gets or sets the reservoir fluid level.
        /// </summary>
        public float? ReservoirFluidLevel { get; set; }

        /// <summary>
        /// Gets or sets the vertical well depth.
        /// </summary>
        public float? VerticalWellDepth { get; set; }

        /// <summary>
        /// Gets or sets the Z factor correlation id.
        /// </summary>
        [Column("ZFactorCorrelationID")]
        public int? ZfactorCorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the value indicating injecting below tubing is set or not.
        /// </summary>
        public bool? IsInjectingBelowTubing { get; set; }

        /// <summary>
        /// Gets or sets the gross rate.
        /// </summary>
        public float? GrossRate { get; set; }

        /// <summary>
        /// Gets or sets the valve critical velocity.
        /// </summary>
        public float? ValveCriticalVelocity { get; set; }

        /// <summary>
        /// Gets or sets the tubing critical velocity.
        /// </summary>
        public float? TubingCriticalVelocity { get; set; }

        /// <summary>
        /// Gets or sets the flowing bhp at injection depth.
        /// </summary>
        [Column("FlowingBHPAtInjectionDepth")]
        public float? FlowingBHPAtInjectionDepth { get; set; }

        /// <summary>
        /// Gets or sets the value indicating estimate injection depth is set or not.
        /// </summary>
        public bool? EstimateInjectionDepth { get; set; }

        /// <summary>
        /// Gets or sets the injection rate for tubing critical velocity.
        /// </summary>
        public float? InjectionRateForTubingCriticalVelocity { get; set; }

        /// <summary>
        /// Gets or sets the tubing critical velocity correlation id.
        /// </summary>
        [Column("TubingCriticalVelocityCorrelationID")]
        public int TubingCriticalVelocityCorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the flowing bhp for optimum liquid rate.
        /// </summary>
        [Column("FBHPForOptimumLiquidRate")]
        public float? FbhpforOptimumLiquidRate { get; set; }

        /// <summary>
        /// Gets or sets the well head temperature source.
        /// </summary>
        public int? WellHeadTemperatureSource { get; set; }

        /// <summary>
        /// Gets or sets the bottomhole temperature source.
        /// </summary>
        public int? BottomholeTemperatureSource { get; set; }

        /// <summary>
        /// Gets or sets the oil specific gravity source.
        /// </summary>
        public int? OilSpecificGravitySource { get; set; }

        /// <summary>
        /// Gets or sets the water specific gravity source.
        /// </summary>
        public int? WaterSpecificGravitySource { get; set; }

        /// <summary>
        /// Gets or sets the gas specific gravity source.
        /// </summary>
        public int? GasSpecificGravitySource { get; set; }

        /// <summary>
        /// Gets or sets the injected gas rate source.
        /// </summary>
        public int? InjectedGasRateSource { get; set; }

        /// <summary>
        /// Gets or sets the downhole gage depth.
        /// </summary>
        public float? DownholeGageDepth { get; set; }

        /// <summary>
        /// Gets or sets the downhole gage pressure.
        /// </summary>
        public float? DownholeGagePressure { get; set; }

        /// <summary>
        /// Gets or sets the downhole gage pressure source.
        /// </summary>
        public int? DownholeGagePressureSource { get; set; }

        /// <summary>
        /// Gets or sets the value indicating downhole is set in analysis.
        /// </summary>
        public bool? UseDownholeGageInAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the oil rate source.
        /// </summary>
        public int? OilRateSource { get; set; }

        /// <summary>
        /// Gets or sets the water rate source.
        /// </summary>
        public int? WaterRateSource { get; set; }

        /// <summary>
        /// Gets or sets the gas rate source.
        /// </summary>
        public int? GasRateSource { get; set; }

        /// <summary>
        /// Gets or sets the adjusted analysis to downhole gauge reading.
        /// </summary>
        public bool? AdjustedAnalysisToDownholeGaugeReading { get; set; }

        /// <summary>
        /// Gets or sets the analysis type.
        /// </summary>
        public int AnalysisType { get; set; }

        /// <summary>
        /// Gets or sets the value indicating analysis result is processed.
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// Gets or sets the multiphase flow correlation source.
        /// </summary>
        public int? MultiphaseFlowCorrelationSource { get; set; }

        /// <summary>
        /// Gets or sets the measured injection depth from analysis.
        /// </summary>
        public float? MeasuredInjectionDepthFromAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the vertical injection depth from analysis.
        /// </summary>
        public float? VerticalInjectionDepthFromAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the result message template.
        /// </summary>
        public string ResultMessageTemplate { get; set; }

        /// <summary>
        /// Gets or sets the value indicating to use tvd is set.
        /// </summary>
        [Column("UseTVD")]
        public bool UseTVD { get; set; }

    }
}
