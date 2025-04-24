using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the esp analysis results model.
    /// </summary>
    public class ESPAnalysisResultModel
    {

        /// <summary>
        /// Gets or sets NodeId.
        /// </summary>
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets test date.
        /// </summary>
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Gets or sets the processed date.
        /// </summary>
        public DateTime ProcessedDate { get; set; }

        /// <summary>
        /// Gets or sets esp pump id.
        /// </summary>
        public int? EsppumpId { get; set; }

        /// <summary>
        /// Gets or sets the vertical pump depth.
        /// </summary>
        public float? VerticalPumpDepth { get; set; }

        /// <summary>
        /// Gets or sets the measured pump depth
        /// </summary>
        public float? MeasuredPumpDepth { get; set; }

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
        /// Gets or sets the pump in-take pressure.
        /// </summary>
        public float? PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets the gross rate.
        /// </summary>
        public float? GrossRate { get; set; }

        /// <summary>
        /// Gets or sets the fluid level above pump.
        /// </summary>
        public float? FluidLevelAbovePump { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure.
        /// </summary>
        public float? TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        public float? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        public int? Frequency { get; set; }

        /// <summary>
        /// Gets or sets the discharge gauge pressure.
        /// </summary>
        public float? DischargeGaugePressure { get; set; }

        /// <summary>
        /// Gets or sets the discharge gauge depth.
        /// </summary>
        public float? DischargeGageDepth { get; set; }

        /// <summary>
        /// Gets or sets the value indicating the use of discharge gauge in analysis.
        /// </summary>
        public bool UseDischargeGageInAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the value indicating gas hadling is enabled or not.
        /// </summary>
        public bool EnableGasHandling { get; set; }

        /// <summary>
        /// Gets or sets the specific gravity of gas.
        /// </summary>
        public float? SpecificGravityOfGas { get; set; }

        /// <summary>
        /// Gets or sets the bottom hole temperature.
        /// </summary>
        public float? BottomholeTemperature { get; set; }

        /// <summary>
        /// Gets or sets the gas separator efficiency.
        /// </summary>
        public float? GasSeparatorEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the oil API.
        /// </summary>
        public float? OilApi { get; set; }

        /// <summary>
        /// Gets or sets the casing id.
        /// </summary>
        public float? CasingId { get; set; }

        /// <summary>
        /// Gets or sets the tubing outer diameter.
        /// </summary>
        public float? TubingOd { get; set; }

        /// <summary>
        /// Gets or sets the value indicating casing valve closed or not.
        /// </summary>
        public bool CasingValveClosed { get; set; }

        /// <summary>
        /// Gets or sets the productivity index.
        /// </summary>
        public float? ProductivityIndex { get; set; }

        /// <summary>
        /// Gets or sets the pressure across pump.
        /// </summary>
        public float? PressureAcrossPump { get; set; }

        /// <summary>
        /// Gets or sets the pump discharge pressure.
        /// </summary>
        public float? PumpDischargePressure { get; set; }

        /// <summary>
        /// Gets or sets the head across pump.
        /// </summary>
        public float? HeadAcrossPump { get; set; }

        /// <summary>
        /// Gets or sets the frictional loss in tubing.
        /// </summary>
        public float? FrictionalLossInTubing { get; set; }

        /// <summary>
        /// Gets or sets the pump efficiency.
        /// </summary>
        public float? PumpEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the calculated fluid level above pump.
        /// </summary>
        public float? CalculatedFluidLevelAbovePump { get; set; }

        /// <summary>
        /// Gets or sets the fluid specific gravity.
        /// </summary>
        public float? FluidSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the pump static pressure.
        /// </summary>
        public float? PumpStaticPressure { get; set; }

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
        public float? Iprslope { get; set; }

        /// <summary>
        /// Gets or sets the water cut.
        /// </summary>
        public int? WaterCut { get; set; }

        /// <summary>
        /// Gets or sets the gas oil ratio at pump.
        /// </summary>
        public float? GasOilRatioAtPump { get; set; }

        /// <summary>
        /// Gets or sets the specific gravity of oil.
        /// </summary>
        public float? SpecificGravityOfOil { get; set; }

        /// <summary>
        /// Gets or sets the formation volume factor.
        /// </summary>
        public float? FormationVolumeFactor { get; set; }

        /// <summary>
        /// Gets or sets the gas compressibility factor.
        /// </summary>
        public float? GasCompressibilityFactor { get; set; }

        /// <summary>
        /// Gets or sets the gas volume factor.
        /// </summary>
        public float? GasVolumeFactor { get; set; }

        /// <summary>
        /// Gets or sets the producing gas oil ratio.
        /// </summary>
        public float? ProducingGor { get; set; }

        /// <summary>
        /// Gets or sets the gas in solution.
        /// </summary>
        public float? GasInSolution { get; set; }

        /// <summary>
        /// Gets or sets the free gas at pump.
        /// </summary>
        public float? FreeGasAtPump { get; set; }

        /// <summary>
        /// Gets or sets the oil volume at pump.
        /// </summary>
        public float? OilVolumeAtPump { get; set; }

        /// <summary>
        /// Gets or sets the gas volume at pump.
        /// </summary>
        public float? GasVolumeAtPump { get; set; }

        /// <summary>
        /// Gets or sets the total volume at pump.
        /// </summary>
        public float? TotalVolumeAtPump { get; set; }

        /// <summary>
        /// Gets or sets the free gas.
        /// </summary>
        public float? FreeGas { get; set; }

        /// <summary>
        /// Gets or sets the turpin parameter.
        /// </summary>
        public float? TurpinParameter { get; set; }

        /// <summary>
        /// Gets or sets the composite tubing specific gravity.
        /// </summary>
        public float? CompositeTubingSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the gas density.
        /// </summary>
        public float? GasDensity { get; set; }

        /// <summary>
        /// Gets or sets the liquid density.
        /// </summary>
        public float? LiquidDensity { get; set; }

        /// <summary>
        /// Gets or sets the annular separation efficiency.
        /// </summary>
        public float? AnnularSeparationEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the tubing gas.
        /// </summary>
        public float? TubingGas { get; set; }

        /// <summary>
        /// Gets or sets the tubing gas oil ratio.
        /// </summary>
        public float? TubingGor { get; set; }

        /// <summary>
        /// Gets or sets the value indicating sucess or not.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the result message template.
        /// </summary>
        public string ResultMessageTemplate { get; set; }

        /// <summary>
        /// Gets or sets the pump intake pressure source.
        /// </summary>
        public int? PumpIntakePressureSource { get; set; }

        /// <summary>
        /// Gets or sets the fluid level above pump source.
        /// </summary>
        public int? FluidLevelAbovePumpSource { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure source.
        /// </summary>
        public int? TubingPressureSource { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure source.
        /// </summary>
        public int? CasingPressureSource { get; set; }

        /// <summary>
        /// Gets or sets the frequency source.
        /// </summary>
        public int? FrequencySource { get; set; }

        /// <summary>
        /// Gets or sets the well head temperature source.
        /// </summary>
        public int? WellHeadTemperatureSource { get; set; }

        /// <summary>
        /// Gets or sets the bottom hole temperature source.
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
        /// Gets or sets the discharge gage pressure source.
        /// </summary>
        public int? DischargeGagePressureSource { get; set; }

        /// <summary>
        /// Gets or sets the max running frequency.
        /// </summary>
        public float? MaxRunningFrequency { get; set; }

        /// <summary>
        /// Gets or sets the motor load percentage.
        /// </summary>
        public float? MotorLoadPercentage { get; set; }

        /// <summary>
        /// Gets or sets the flowing brake hp.
        /// </summary>
        public float? FlowingBhp { get; set; }

        /// <summary>
        /// Gets or sets the water specific gravity.
        /// </summary>
        public float? WaterSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the tubing id.
        /// </summary>
        public float? TubingId { get; set; }

        /// <summary>
        /// Gets or sets the well head temperature.
        /// </summary>
        public float? WellHeadTemperature { get; set; }

        /// <summary>
        /// Gets or sets the head relative to pump curve.
        /// </summary>
        public float? HeadRelativeToPumpCurve { get; set; }

        /// <summary>
        /// Gets or sets the head relative to well performance.
        /// </summary>
        public float? HeadRelativeToWellPerformance { get; set; }

        /// <summary>
        /// Gets or sets the head relative to recommended range.
        /// </summary>
        public float? HeadRelativeToRecommendedRange { get; set; }

        /// <summary>
        /// Gets or sets the flow relative to recommended range.
        /// </summary>
        public float? FlowRelativeToRecommendedRange { get; set; }

        /// <summary>
        /// Gets or sets the design score.
        /// </summary>
        public float? DesignScore { get; set; }

        /// <summary>
        /// Gets or sets the pump degradation.
        /// </summary>
        public float? PumpDegradation { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the max potential production rate.
        /// </summary>
        public float? MaxPotentialProductionRate { get; set; }

        /// <summary>
        /// Gets or sets the max potential frequency.
        /// </summary>
        public float? MaxPotentialFrequency { get; set; }

        /// <summary>
        /// Gets or sets the production increase percentage.
        /// </summary>
        public float? ProductionIncreasePercentage { get; set; }

        /// <summary>
        /// Gets or sets the value to use TVD.
        /// </summary>
        public bool UseTVD { get; set; }

        /// <summary>
        /// Gets or sets number of stages.
        /// </summary>
        public int? NumberOfStages { get; set; }

    }
}
