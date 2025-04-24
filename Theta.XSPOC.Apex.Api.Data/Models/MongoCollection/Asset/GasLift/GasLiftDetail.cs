using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.GasLift
{
    /// <summary>
    /// A class representing the details of a gas lift asset.
    /// </summary>
    public class GasLiftDetail : WellDetailsBase
    {

        /// <summary>
        /// Gets or sets the gas injection depth.
        /// </summary>
        public double? GasInjectionDepth { get; set; }

        /// <summary>
        /// Gets or sets the gas injection pressure.
        /// </summary>
        public double? GasInjectionPressure { get; set; }

        /// <summary>
        /// Gets or sets the formation gas oil ratio.
        /// </summary>
        public double? FormationGasOilRatio { get; set; }

        /// <summary>
        /// Gets or sets the percentage of H2S.
        /// </summary>
        public double? PercentH2S { get; set; }

        /// <summary>
        /// Gets or sets the percentage of CO2.
        /// </summary>
        public double? PercentN2 { get; set; }

        /// <summary>
        /// Gets or sets the percentage of CO2.
        /// </summary>
        public double? PercentCO2 { get; set; }

        /// <summary>
        /// Gets or sets the valve configuration option.
        /// </summary>
        public Lookup.Lookup ValveConfigurationOption { get; set; }

        /// <summary>
        /// Gets or sets the downhole gage depth.
        /// </summary>
        public double? DownholeGageDepth { get; set; }

        /// <summary>
        /// Gets or sets if the downhole gage is used in the analysis.
        /// </summary>
        public bool UseDownholeGageInAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the injected gas specific gravity.
        /// </summary>
        public double? InjectedGasSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets if the gas is injected below the tubing.
        /// </summary>
        public bool IsInjectingBelowTubing { get; set; }

        /// <summary>
        /// Gets or sets if the injection depth is estimated.
        /// </summary>
        public bool EstimateInjectionDepth { get; set; }

        /// <summary>
        /// Gets or sets the compression cost.
        /// </summary>
        public double? CompressionCost { get; set; }

        /// <summary>
        /// Gets or sets the injection cost.
        /// </summary>
        public double? InjectionCost { get; set; }

        /// <summary>
        /// Gets or sets the separation cost.
        /// </summary>
        public double? SeparationCost { get; set; }

        /// <summary>
        /// Gets or sets the cost type.
        /// </summary>
        public Lookup.Lookup CostType { get; set; }

        /// <summary>
        /// Gets or sets the well valves.
        /// </summary>
        public IList<WellValve> WellValves { get; set; }

        /// <summary>
        /// Gets or sets the well orifices.
        /// </summary>
        public WellOrifice WellOrifices { get; set; }

    }
}
