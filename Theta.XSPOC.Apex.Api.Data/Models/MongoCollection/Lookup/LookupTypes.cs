namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// Defines the lookup types.
    /// </summary>
    public enum LookupTypes
    {

        /// <summary>
        /// Reuse this type later.
        /// </summary>
        ReuseLaterToDo = 1,

        /// <summary>
        /// The alarm event types lookup type.
        /// </summary>
        AlarmEventTypes = 2,

        /// <summary>
        /// The alarm types lookup type.
        /// </summary>
        AlarmTypes = 3,

        /// <summary>
        /// The analysis correlations lookup type.
        /// </summary>
        AnalysisCorrelations = 4,

        /// <summary>
        /// The analysis curveSet types lookup type.
        /// </summary>
        AnalysisCurveSetTypes = 5,

        /// <summary>
        /// The analysis types lookup type.
        /// </summary>
        AnalysisTypes = 6,

        /// <summary>
        /// The application param standard types lookup type.
        /// </summary>
        ApplicationParamStandardTypes = 7,

        /// <summary>
        /// The applications lookup type.
        /// </summary>
        Applications = 8,

        /// <summary>
        /// The control actions lookup type.
        /// </summary>
        ControlActions = 9,

        /// <summary>
        /// The correlations lookup type.
        /// </summary>
        Correlations = 10,

        /// <summary>
        /// The correlation types lookup type.
        /// </summary>
        CorrelationTypes = 11,

        /// <summary>
        /// The curve types lookup type.
        /// </summary>
        CurveTypes = 12,

        /// <summary>
        /// The cata types lookup type.
        /// </summary>
        DataTypes = 13,

        /// <summary>
        /// The file types lookup type.
        /// </summary>
        FileTypes = 14,

        /// <summary>
        /// The IPR calculation error types lookup type.
        /// </summary>
        IPRCalculationErrorTypes = 15,

        /// <summary>
        /// The locale phrases lookup type.
        /// </summary>
        LocalePhrases = 16,

        /// <summary>
        /// The meter types lookup type.
        /// </summary>
        MeterTypes = 17,

        /// <summary>
        /// The param standard types lookup type.
        /// </summary>
        ParamStandardTypes = 18,

        /// <summary>
        /// The POC types lookup type.
        /// </summary>
        POCTypes = 19,

        /// <summary>
        /// The protocol types lookup type.
        /// </summary>
        ProtocolTypes = 20,

        /// <summary>
        /// The group status views columns lookup type.
        /// </summary>
        GroupStatusViewsColumns = 21,

        /// <summary>
        /// The motor kind lookup type.
        /// </summary>
        MotorKind = 22,

        /// <summary>
        /// The motor setting lookup type.
        /// </summary>
        MotorSetting = 23,

        /// <summary>
        /// The motor setting lookup type.
        /// </summary>
        MotorSize = 24,

        /// <summary>
        /// The POC Type action lookup type.
        /// </summary>
        POCTypeAction = 25,

        /// <summary>
        /// The POC Type application lookup type.
        /// </summary>
        POCTypeApplication = 26,

        /// <summary>
        /// The pumping unit manufacturer lookup type.
        /// </summary>
        PumpingUnitManufacturer = 27,

        /// <summary>
        /// The pumping unit manufacturer lookup type.
        /// </summary>
        PumpingUnit = 28,

        /// <summary>
        /// The rod grade lookup type.
        /// </summary>
        RodGrade = 29,

        /// <summary>
        /// The rod guide lookup type.
        /// </summary>
        RodGuide = 30,

        /// <summary>
        /// The rod material lookup type.
        /// </summary>
        RodMaterial = 31,

        /// <summary>
        /// The rod size group lookup type.
        /// </summary>
        RodSizeGroup = 34,

        /// <summary>
        /// The rod size  lookup type.
        /// </summary>
        RodSize = 35,

        /// <summary>
        /// The rod size  lookup type.
        /// </summary>
        SAMPumpingUnit = 36,

        /// <summary>
        /// The rod size  lookup type.
        /// </summary>
        SensitivityAnalysisCustomInputType = 37,

        /// <summary>
        /// The sensitivity analysis input options lookup type.
        /// </summary>
        SensitivityAnalysisInputOptions = 38,

        /// <summary>
        /// The sensitivity analysis inputs lookup type.
        /// </summary>
        SensitivityAnalysisInputs = 39,

        /// <summary>
        /// The sensitivity analysis input types lookup type.
        /// </summary>
        SensitivityAnalysisInputTypes = 40,

        /// <summary>
        /// The setpoint groups lookup type.
        /// </summary>
        SetpointGroups = 41,

        /// <summary>
        /// The states lookup type.
        /// </summary>
        States = 42,

        /// <summary>
        /// The status registers lookup type.
        /// </summary>
        StatusRegisters = 43,

        /// <summary>
        /// The tubing sizes lookup type.
        /// </summary>
        TubingSizes = 44,

        /// <summary>
        /// The unit types lookup type.
        /// </summary>
        UnitTypes = 45,

        /// <summary>
        /// The XDiag outputs lookup type.
        /// </summary>
        XDiagOutputs = 46,

        /// <summary>
        /// The analysis input defaults lookup type.
        /// </summary>
        AnalysisInputDefaults = 57,

        /// <summary>
        /// The analysis sources lookup type.
        /// </summary>
        AnalysisSources = 58,

        /// <summary>
        /// The casing sizes lookup type.
        /// </summary>
        CasingSizes = 59,

        /// <summary>
        /// The ESP cables lookup type.
        /// </summary>
        ESPCables = 60,

        /// <summary>
        /// The ESP events types lookup type.
        /// </summary>
        ESPEventTypes = 61,

        /// <summary>
        /// The ESP manufacturer lookup type.
        /// </summary>
        ESPManufacturers = 62,

        /// <summary>
        /// The ESP motor leads lookup type.
        /// </summary>
        ESPMotorLeads = 63,

        /// <summary>
        /// The ESP motors lookup type.
        /// </summary>
        ESPMotors = 64,

        /// <summary>
        /// The ESP pumps lookup type.
        /// </summary>
        ESPPumps = 56,

        /// <summary>
        /// The ESP seals lookup type.
        /// </summary>
        ESPSeals = 47,

        /// <summary>
        /// The failure components lookup type.
        /// </summary>
        FailureComponents = 48,

        /// <summary>
        /// The failure sub components lookup type.
        /// </summary>
        FailureSubcomponents = 49,

        /// <summary>
        /// The GL flow control device states lookup type.
        /// </summary>
        GLFlowControlDeviceStates = 50,

        /// <summary>
        /// The GL manufacturers lookup type.
        /// </summary>
        GLManufacturers = 51,

        /// <summary>
        /// The GL valve configuration options lookup type.
        /// </summary>
        GLValveConfigurationOptions = 52,

        /// <summary>
        /// The GL valves lookup type.
        /// </summary>
        GLValves = 53,

        /// <summary>
        /// The group status columns lookup type.
        /// </summary>
        GroupStatusColumns = 54,

        /// <summary>
        /// The group status tables lookup type.
        /// </summary>
        GroupStatusTables = 55,

        /// <summary>
        /// The cost types lookup type.
        /// </summary>
        CostType = 65,

        /// <summary>
        /// The downhole separator lookup type.
        /// </summary>
        DownholeSeparator = 66,

        /// <summary>
        /// PUCustom lookup type.
        /// </summary>
        PUCustom = 67,

        /// <summary>
        /// Camera Alarm Type lookup type.
        /// </summary>
        CameraAlarmTypes = 68,

        /// <summary>
        /// Facility Tag Group lookup type.
        /// </summary>
        FacilityTagGroups = 69,

        /// <summary>
        /// Mode of Production Mapping lookup type.
        /// </summary>
        MethodOfProductionMapping = 70,

        /// <summary>
        /// Time Series Chart Aggregation lookup type.
        /// </summary>
        TimeSeriesChartAggregation = 71
    }
}
