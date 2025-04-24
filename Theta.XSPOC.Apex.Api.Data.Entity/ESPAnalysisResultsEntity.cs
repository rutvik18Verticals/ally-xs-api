using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The esp analysis result table.
    /// </summary>
    [Table("tblESPAnalysisResults")]
    public class ESPAnalysisResultsEntity
    {

        /// <summary>
        /// Gets or sets NodeId.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        [Required]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets test date.
        /// </summary>
        [Column(TypeName = "datetime")]
        [Required]
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Gets or sets processed date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime ProcessedDate { get; set; }

        /// <summary>
        /// Gets or sets esp pump id.
        /// </summary>
        [Column("ESPPumpID")]
        public int? EsppumpId { get; set; }

        /// <summary>
        /// Gets or sets number of stages.
        /// </summary>
        public int? NumberOfStages { get; set; }

        /// <summary>
        /// Gets or sets vertical pump depth.
        /// </summary>
        public float? VerticalPumpDepth { get; set; }

        /// <summary>
        /// Gets or sets measured pump depth.
        /// </summary>
        public float? MeasuredPumpDepth { get; set; }

        /// <summary>
        /// Gets or sets oil rate.
        /// </summary>
        public float? OilRate { get; set; }

        /// <summary>
        /// Gets or sets water rate.
        /// </summary>
        public float? WaterRate { get; set; }

        /// <summary>
        /// Gets or sets gas rate.
        /// </summary>
        public float? GasRate { get; set; }

        /// <summary>
        /// Gets or sets pump intake pressure.
        /// </summary>
        public float? PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets gross rate.
        /// </summary>
        public float? GrossRate { get; set; }

        /// <summary>
        /// Gets or sets fluid level above pump.
        /// </summary>
        public float? FluidLevelAbovePump { get; set; }

        /// <summary>
        /// Gets or sets tubing pressure.
        /// </summary>
        public float? TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets casing pressure.
        /// </summary>
        public float? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets frequency.
        /// </summary>
        public int? Frequency { get; set; }

        /// <summary>
        /// Gets or sets productivity index.
        /// </summary>
        public float? ProductivityIndex { get; set; }

        /// <summary>
        /// Gets or sets pressure across pump.
        /// </summary>
        public float? PressureAcrossPump { get; set; }

        /// <summary>
        /// Gets or sets pump discharge pressure.
        /// </summary>
        public float? PumpDischargePressure { get; set; }

        /// <summary>
        /// Gets or sets head across pump.
        /// </summary>
        public float? HeadAcrossPump { get; set; }

        /// <summary>
        /// Gets or sets frictional loss in tubing.
        /// </summary>
        public float? FrictionalLossInTubing { get; set; }

        /// <summary>
        /// Gets or sets pump efficiency.
        /// </summary>
        public float? PumpEfficiency { get; set; }

        /// <summary>
        /// Gets or sets calculated fluid level above pump.
        /// </summary>
        public float? CalculatedFluidLevelAbovePump { get; set; }

        /// <summary>
        /// Gets or sets fluid specific gravity.
        /// </summary>
        public float? FluidSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets pump static pressure.
        /// </summary>
        public float? PumpStaticPressure { get; set; }

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
        /// Gets or sets iprslope.
        /// </summary>
        [Column("IPRSlope")]
        public float? IPRSlope { get; set; }

        /// <summary>
        /// Gets or sets water cut.
        /// </summary>
        public int? WaterCut { get; set; }

        /// <summary>
        /// Gets or sets success.
        /// </summary>
        [Required]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets result message.
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets pump intake pressure source.
        /// </summary>
        public int? PumpIntakePressureSource { get; set; }

        /// <summary>
        /// Gets or sets fluid level above pump source.
        /// </summary>
        public int? FluidLevelAbovePumpSource { get; set; }

        /// <summary>
        /// Gets or sets tubing pressure source.
        /// </summary>
        public int? TubingPressureSource { get; set; }

        /// <summary>
        /// Gets or sets casing pressure source.
        /// </summary>
        public int? CasingPressureSource { get; set; }

        /// <summary>
        /// Gets or sets frequency source.
        /// </summary>
        public int? FrequencySource { get; set; }

        /// <summary>
        /// Gets or sets enable gas handling.
        /// </summary>
        public bool EnableGasHandling { get; set; }

        /// <summary>
        /// Gets or sets specific gravity of gas.
        /// </summary>
        public float? SpecificGravityOfGas { get; set; }

        /// <summary>
        /// Gets or sets bottom hole temperature.
        /// </summary>
        public float? BottomholeTemperature { get; set; }

        /// <summary>
        /// Gets or sets gas separator efficiency.
        /// </summary>
        public float? GasSeparatorEfficiency { get; set; }

        /// <summary>
        /// Gets or sets iol api.
        /// </summary>
        [Column("OilAPI")]
        public float? OilApi { get; set; }

        /// <summary>
        /// Gets or sets casing id.
        /// </summary>
        [Column("CasingID")]
        public float? CasingId { get; set; }

        /// <summary>
        /// Gets or sets tubing od.
        /// </summary>
        [Column("TubingOD")]
        public float? TubingOd { get; set; }

        /// <summary>
        /// Gets or sets casing valve closed.
        /// </summary>
        public bool CasingValveClosed { get; set; }

        /// <summary>
        /// Gets or sets gas oil ration at pump.
        /// </summary>
        public float? GasOilRatioAtPump { get; set; }

        /// <summary>
        /// Gets or sets specific gravity of oil.
        /// </summary>
        public float? SpecificGravityOfOil { get; set; }

        /// <summary>
        /// Gets or sets formation volumn factor.
        /// </summary>
        public float? FormationVolumeFactor { get; set; }

        /// <summary>
        /// Gets or sets gas copressibility factor.
        /// </summary>
        public float? GasCompressibilityFactor { get; set; }

        /// <summary>
        /// Gets or sets gas voulmn factor.
        /// </summary>
        public float? GasVolumeFactor { get; set; }

        /// <summary>
        /// Gets or sets producing gor.
        /// </summary>
        [Column("ProducingGOR")]
        public float? ProducingGor { get; set; }

        /// <summary>
        /// Gets or sets gas in solution.
        /// </summary>
        public float? GasInSolution { get; set; }

        /// <summary>
        /// Gets or sets free gas at pump.
        /// </summary>
        public float? FreeGasAtPump { get; set; }

        /// <summary>
        /// Gets or sets oil volumn at pump.
        /// </summary>
        public float? OilVolumeAtPump { get; set; }

        /// <summary>
        /// Gets or sets gas volumn at pump.
        /// </summary>
        public float? GasVolumeAtPump { get; set; }

        /// <summary>
        /// Gets or sets total volumn at pump.
        /// </summary>
        public float? TotalVolumeAtPump { get; set; }

        /// <summary>
        /// Gets or sets free gas.
        /// </summary>
        public float? FreeGas { get; set; }

        /// <summary>
        /// Gets or sets turpin parameter.
        /// </summary>
        public float? TurpinParameter { get; set; }

        /// <summary>
        /// Gets or sets composite tubing specific gravity.
        /// </summary>
        public float? CompositeTubingSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets gas density.
        /// </summary>
        public float? GasDensity { get; set; }

        /// <summary>
        /// Gets or sets liquid density.
        /// </summary>
        public float? LiquidDensity { get; set; }

        /// <summary>
        /// Gets or sets annular separation efficiency.
        /// </summary>
        public float? AnnularSeparationEfficiency { get; set; }

        /// <summary>
        /// Gets or sets tubing gas.
        /// </summary>
        public float? TubingGas { get; set; }

        /// <summary>
        /// Gets or sets tubing gor.
        /// </summary>
        [Column("TubingGOR")]
        public float? TubingGor { get; set; }

        /// <summary>
        /// Gets or sets flowing bph.
        /// </summary>
        [Column("FlowingBHP")]
        public float? FlowingBhp { get; set; }

        /// <summary>
        /// Gets or sets water specific gravity.
        /// </summary>
        public float? WaterSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets tubing id.
        /// </summary>
        [Column("TubingID")]
        public float? TubingId { get; set; }

        /// <summary>
        /// Gets or sets well head temperature.
        /// </summary>
        public float? WellHeadTemperature { get; set; }

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets head relative to recommended range.
        /// </summary>
        public float? HeadRelativeToRecommendedRange { get; set; }

        /// <summary>
        /// Gets or sets flow relative to recommeded range.
        /// </summary>
        public float? FlowRelativeToRecommendedRange { get; set; }

        /// <summary>
        /// Gets or sets design score.
        /// </summary>
        public float? DesignScore { get; set; }

        /// <summary>
        /// Gets or sets head relative to well performance.
        /// </summary>
        public float? HeadRelativeToWellPerformance { get; set; }

        /// <summary>
        /// Gets or sets head relative to pump curve.
        /// </summary>
        public float? HeadRelativeToPumpCurve { get; set; }

        /// <summary>
        /// Gets or sets pump degradation.
        /// </summary>
        public float? PumpDegradation { get; set; }

        /// <summary>
        /// Gets or sets max potential production rate.
        /// </summary>
        public float? MaxPotentialProductionRate { get; set; }

        /// <summary>
        /// Gets or sets max potential frequency.
        /// </summary>
        public float? MaxPotentialFrequency { get; set; }

        /// <summary>
        /// Gets or sets production increase percentage.
        /// </summary>
        public float? ProductionIncreasePercentage { get; set; }

        /// <summary>
        /// Gets or sets well head temperature source.
        /// </summary>
        public int? WellHeadTemperatureSource { get; set; }

        /// <summary>
        /// Gets or sets bottom hole temperature source.
        /// </summary>
        public int? BottomholeTemperatureSource { get; set; }

        /// <summary>
        /// Gets or sets oil specific gravity source.
        /// </summary>
        public int? OilSpecificGravitySource { get; set; }

        /// <summary>
        /// Gets or sets water specific gravity source.
        /// </summary>
        public int? WaterSpecificGravitySource { get; set; }

        /// <summary>
        /// Gets or sets gas specific gravity source.
        /// </summary>
        public int? GasSpecificGravitySource { get; set; }

        /// <summary>
        /// Gets or sets use discharge gage in analysis.
        /// </summary>
        public bool UseDischargeGageInAnalysis { get; set; }

        /// <summary>
        /// Gets or sets discharge gage pressure source.
        /// </summary>
        public int? DischargeGagePressureSource { get; set; }

        /// <summary>
        /// Gets or sets oil rate source.
        /// </summary>
        public int? OilRateSource { get; set; }

        /// <summary>
        /// Gets or sets water rate source.
        /// </summary>
        public int? WaterRateSource { get; set; }

        /// <summary>
        /// Gets or sets gas rate source.
        /// </summary>
        public int? GasRateSource { get; set; }

        /// <summary>
        /// Gets or sets max running frequency.
        /// </summary>
        public float? MaxRunningFrequency { get; set; }

        /// <summary>
        /// Gets or sets motor load percentage.
        /// </summary>
        public float? MotorLoadPercentage { get; set; }

        /// <summary>
        /// Gets or sets result message template.
        /// </summary>
        public string ResultMessageTemplate { get; set; }

        /// <summary>
        /// Gets or sets result message template.
        /// </summary>
        [Column("DischargeGagePressure")]
        public float? DischargeGaugePressure { get; set; }

        /// <summary>
        /// Gets or sets result message template.
        /// </summary>
        [Column("DischargeGageDepth")]
        public float? DischargeGaugeDepth { get; set; }

        /// <summary>
        /// Gets or sets result message template.
        /// </summary>
        public bool UseTVD { get; set; }

        ///// <summary>
        ///// Gets or sets pump depth tvd calculated.
        ///// </summary>
        //[Column("PumpDepthTVDCalculated")]
        //public float? PumpDepthTvdcalculated { get; set; }

        ///// <summary>
        ///// Gets or sets use tvd.
        ///// </summary>
        //[Column("UseTVD")]
        //public bool UseTvd { get; set; }

    }
}
