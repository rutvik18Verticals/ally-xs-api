namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Class representing the input fields of analysis result from response GL analysis.
    /// </summary>
    public class GLAnalysisInput : AnalysisInputBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the vertical gas injection depth
        /// </summary>
        public float? GasInjectionDepth { get; set; } // Length

        /// <summary>
        /// Gets or sets the vertical well depth.
        /// </summary>
        public float? VerticalWellDepth { get; set; } // Length

        /// <summary>
        /// Gets or sets the measured well depth
        /// </summary>
        public float? MeasuredWellDepth { get; set; } // Length

        /// <summary>
        /// Gets or sets the gas injection rate.
        /// </summary>
        public float? GasInjectionRate { get; set; } // Volume

        /// <summary>
        /// Gets or sets the gas injection pressure.
        /// </summary>
        public float? GasInjectionPressure { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the oil specific gravity.
        /// </summary>
        public float? OilSpecificGravity { get; set; } // RelativeDensity

        /// <summary>
        /// Gets or sets the gas specific gravity
        /// </summary>
        public float? GasSpecificGravity { get; set; } // RelativeDensity

        /// <summary>
        /// Gets or sets the tubing outer diameter.
        /// </summary>
        public float? TubingOuterDiameter { get; set; } // Length

        /// <summary>
        /// Gets or sets the casing inner diameter.
        /// </summary>
        public float? CasingInnerDiameter { get; set; } // Length

        /// <summary>
        /// Gets or sets the wellhead temperature
        /// </summary>
        public float? WellheadTemperature { get; set; } // Temperature

        /// <summary>
        /// Gets or sets the reservoir temperature
        /// </summary>
        public float? BottomholeTemperature { get; set; } // Temperature

        /// <summary>
        /// Gets or sets the wellhead pressure
        /// </summary>
        public float? WellheadPressure { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the reservoir pressure
        /// </summary>
        public float? ReservoirPressure { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the bubblepoint pressure.
        /// </summary>
        public float? BubblepointPressure { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the formation gas oil ratio
        /// </summary>
        public double? FormationGasOilRatio { get; set; }

        /// <summary>
        /// Gets or sets the mole percent H2S of gas
        /// </summary>
        public float? PercentH2S { get; set; } // Fraction

        /// <summary>
        /// Gets or sets the mole percent N2 of gas
        /// </summary>
        public float? PercentN2 { get; set; } // Fraction

        /// <summary>
        /// Gets or sets the mole percent CO2 of gas
        /// </summary>
        public float? PercentCO2 { get; set; } // Fraction

        /// <summary>
        /// Gets or sets the oil viscosity correlation
        /// </summary>
        public AnalysisCorrelation OilViscosityCorrelation { get; set; }

        /// <summary>
        /// Gets or sets the oil formation volume factor correlation
        /// </summary>
        public AnalysisCorrelation OilFormationVolumeFactorCorrelation { get; set; }

        /// <summary>
        /// Gets or sets the solution gas oil ratio correlation
        /// </summary>
        public AnalysisCorrelation SolutionGasOilRatioCorrelation { get; set; }

        /// <summary>
        /// Gets or sets the z-factor correlation
        /// </summary>
        public AnalysisCorrelation ZFactorCorrelation { get; set; }

        /// <summary>
        /// Gets or sets the tubing critical velocity correlation
        /// </summary>
        public AnalysisCorrelation TubingCriticalVelocityCorrelation { get; set; }

        /// <summary>
        /// Gets or sets whether we should estimate an injection depth rather than take a user input
        /// </summary>
        public bool EstimateInjectionDepth { get; set; }

        /// <summary>
        /// Gets or sets whether we should use the downhole gauge reading in analysis
        /// </summary>
        public bool UseDownholeGaugeForAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the downhole gauge depth
        /// </summary>
        public float? DownholeGaugeDepth { get; set; } // Length

        /// <summary>
        /// Gets or sets the pressure the downhole gauge  was reading
        /// </summary>
        public float? DownholeGaugePressure { get; set; } // Pressure

        #endregion

    }
}
