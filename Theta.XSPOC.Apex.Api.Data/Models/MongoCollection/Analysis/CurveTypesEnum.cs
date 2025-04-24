namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis
{
    /// <summary>
    /// Enumeration for different types of curves.
    /// </summary>
    public enum CurveTypesEnum
    {
        /// <summary>
        /// Gas Lift Production Performance Curve
        /// </summary>
        GasLift_ProductionPerformanceCurve = 1,

        /// <summary>
        /// Gas Lift Pressure Performance Curve
        /// </summary>
        GasLift_PressurePerformanceCurve = 2,

        /// <summary>
        /// Gas Lift Valve Performance Curve
        /// </summary>
        GasLift_ValvePerformanceCurve = 3,

        /// <summary>
        /// Gas Lift Gas Injection Curve
        /// </summary>
        GasLift_GasInjectionCurve = 4,

        /// <summary>
        /// Gas Lift Reservoir Fluid Curve
        /// </summary>
        GasLift_ReservoirFluidCurve = 5,

        /// <summary>
        /// Gas Lift Kill Fluid Curve
        /// </summary>
        GasLift_KillFluidCurve = 6,

        /// <summary>
        /// Gas Lift Temperature Curve
        /// </summary>
        GasLift_TemperatureCurve = 7,

        /// <summary>
        /// Gas Lift Production Fluid Curve
        /// </summary>
        GasLift_ProductionFluidCurve = 8,

        /// <summary>
        /// ESP Pump Curve
        /// </summary>
        ESP_PumpCurve = 9,

        /// <summary>
        /// ESP Well Performance Curve
        /// </summary>
        ESP_WellPerformanceCurve = 10,

        /// <summary>
        /// ESP Power Curve
        /// </summary>
        ESP_PowerCurve = 11,

        /// <summary>
        /// ESP Efficiency Curve
        /// </summary>
        ESP_EfficiencyCurve = 12,

        /// <summary>
        /// ESP Recommended Range Curve - Left
        /// </summary>
        ESP_RecommendedRangeCurve_Left = 13,

        /// <summary>
        /// ESP Recommended Range Curve - Top
        /// </summary>
        ESP_RecommendedRangeCurve_Top = 14,

        /// <summary>
        /// ESP Recommended Range Curve - Right
        /// </summary>
        ESP_RecommendedRangeCurve_Right = 15,

        /// <summary>
        /// ESP Recommended Range Curve - Bottom
        /// </summary>
        ESP_RecommendedRangeCurve_Bottom = 16,

        /// <summary>
        /// ESP Production Fluid Curve
        /// </summary>
        ESP_ProductionFluidCurve = 17,

        /// <summary>
        /// ESP IPR Curve for ESP
        /// </summary>
        ESP_IPRCurveForESP = 18,

        /// <summary>
        /// Gas Lift IPR Curve for Gas Lift
        /// </summary>
        GasLift_IPRCurveForGasLift = 19,

        /// <summary>
        /// Rod Lift IPR Curve for Rod Pump
        /// </summary>
        RodLift_IPRCurveForRodPump = 20,

        /// <summary>
        /// PCP IPR Curve for PCP
        /// </summary>
        PCP_IPRCurveForPCP = 21,

        /// <summary>
        /// Plunger Lift IPR Curve for Plunger Lift
        /// </summary>
        PlungerLift_IPRCurveForPlungerLift = 22,

        /// <summary>
        /// Jet Pump IPR Curve for Jet Pump
        /// </summary>
        JetPump_IPRCurveForJetPump = 23,

        /// <summary>
        /// Plunger Assisted Gas Lift IPR Curve for Plunger Assisted Gas Lift
        /// </summary>
        PlungerAssistedGasLift_IPRCurveForPlungerAssistedGasLift = 24,

        /// <summary>
        /// Gas Lift Temperature Survey Curve
        /// </summary>
        GasLift_TemperatureSurveyCurve = 25,

        /// <summary>
        /// Gas Lift Pressure Survey Curve
        /// </summary>
        GasLift_PressureSurveyCurve = 26,

        /// <summary>
        /// Gas Lift Flowing Bottomhole Pressure Performance Curve
        /// </summary>
        GasLift_FlowingBottomholePressurePerformanceCurve = 27,

        /// <summary>
        /// ESP Tornado Curve
        /// </summary>
        ESP_TornadoCurve = 28,

        /// <summary>
        /// Gas Lift GL Injection Rate For Critical Velocity
        /// </summary>
        GasLift_GLInjectionRateForCriticalVelocity = 29
    }

}
