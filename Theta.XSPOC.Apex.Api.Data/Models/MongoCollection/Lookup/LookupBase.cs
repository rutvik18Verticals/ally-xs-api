using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This is a base class for the lookup sub document.
    /// </summary>
    [BsonKnownTypes(typeof(POCTypes), typeof(ParamStandardTypes), typeof(UnitTypes), typeof(DataTypes), typeof(AnalysisCorrelations), typeof(AnalysisCurveSetTypes),
        typeof(AnalysisInputDefaults), typeof(AnalysisSources), typeof(AnalysisTypes), typeof(Correlations), typeof(CorrelationTypes),
        typeof(GLFlowControlDeviceStates), typeof(GLValveConfigurationOptions), typeof(Applications), typeof(CurveTypes), typeof(PumpingUnitManufacturer),
        typeof(PUCustom), typeof(PumpingUnit), typeof(GLManufacturers), typeof(LocalePhrases), typeof(ControlActions), typeof(POCTypeAction), typeof(States),
        typeof(CasingSizes), typeof(TubingSizes), typeof(DownholeSeparator), typeof(MotorKind), typeof(MotorSetting), typeof(MotorSize),
        typeof(RodGrade), typeof(RodGuide), typeof(RodMaterial), typeof(RodSize), typeof(RodSizeGroup), typeof(GLValves), typeof(ESPPumps),
        typeof(SAMPumpingUnit), typeof(AlarmEventTypes), typeof(AlarmTypes), typeof(ApplicationParamStandardTypes),
        typeof(CostType), typeof(ESPCables), typeof(ESPEventTypes), typeof(ESPManufacturers), typeof(ESPMotorLeads), typeof(ESPMotors),
        typeof(ESPSeals), typeof(FailureComponents), typeof(FailureSubcomponents), typeof(FileTypes), typeof(GroupStatusColumns), typeof(GroupStatusTables),
        typeof(GroupStatusViewsColumns), typeof(IPRCalculationErrorTypes), typeof(MeterTypes), typeof(POCTypeApplication),
        typeof(ProtocolTypes), typeof(SensitivityAnalysisCustomInputType), typeof(SensitivityAnalysisInputTypes), typeof(SensitivityAnalysisInputOptions),
        typeof(SensitivityAnalysisInputs), typeof(SetpointGroups), typeof(StatusRegisters), typeof(XDiagOutputs), typeof(FacilityTagGroups), typeof(CameraTypes),
        typeof(CameraConfigurations), typeof(CameraAlarmTypes))]
    public class LookupBase
    {

    }
}