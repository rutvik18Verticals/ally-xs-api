namespace Theta.XSPOC.Apex.Api.Core.Models
{
    /// <summary>
    /// This enum defines the list of overlay fields.
    /// </summary>
    public enum OverlayFields
    {

        /// <summary>
        /// The field is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The last good scan field.
        /// </summary>
        LastGoodScan = 1,

        /// <summary>
        /// The api designation field.
        /// </summary>
        ApiDesignation = 2,

        /// <summary>
        /// The current communication status field.
        /// </summary>
        CurrentCommunicationStatus = 3,

        /// <summary>
        /// The controller information field.
        /// </summary>
        ControllerInformation = 4,

        /// <summary>
        /// The structural loading field.
        /// </summary>
        StructuralLoading = 5,

        /// <summary>
        /// The run status field.
        /// </summary>
        RunStatus = 6,

        /// <summary>
        /// The time in state field.
        /// </summary>
        TimeInState = 7,

        /// <summary>
        /// The motor type field.
        /// </summary>
        MotorType = 8,

        /// <summary>
        /// The motor loading field.
        /// </summary>
        MotorLoading = 9,

        /// <summary>
        /// The strokes per minute field.
        /// </summary>
        StrokesPerMinute = 10,

        /// <summary>
        /// The stroke length field.
        /// </summary>
        StrokeLength = 11,

        /// <summary>
        /// The cycles today field.
        /// </summary>
        CyclesToday = 12,

        /// <summary>
        /// The cycles yesterday field.
        /// </summary>
        CyclesYesterday = 13,

        /// <summary>
        /// The tubing pressure field.
        /// </summary>
        TubingPressure = 14,

        /// <summary>
        /// The rod loading field.
        /// </summary>
        RodLoading = 15,

        /// <summary>
        /// The pump fillage field.
        /// </summary>
        PumpFillage = 16,

        /// <summary>
        /// The casing pressure field.
        /// </summary>
        CasingPressure = 17,

        /// <summary>
        /// The gearbox loading field.
        /// </summary>
        GearboxLoading = 18,

        /// <summary>
        /// The pump depth field.
        /// </summary>
        PumpDepth = 19,

        /// <summary>
        /// The pump type field.
        /// </summary>
        PumpType = 20,

        /// <summary>
        /// The Flowing Pressure field.
        /// </summary>
        FlowingTemperature = 21,

        /// <summary>
        /// The node address protocol field.
        /// </summary>
        Protocol = 22,

        /// <summary>
        /// The node address hostname field.
        /// </summary>
        HostName = 23,

        /// <summary>
        /// The node address protocol field.
        /// </summary>
        OPCType = 24,

        /// <summary>
        /// The node address port field.
        /// </summary>
        Port = 25,

        /// <summary>
        /// The node address rtu address field.
        /// </summary>
        RTUAddress = 26,

        /// <summary>
        /// The node address offset field.
        /// </summary>
        Offset = 27,

        /// <summary>
        /// The pumping unit field.
        /// </summary>
        PumpingUnit = 28,

        /// <summary>
        /// The firmware version field.
        /// </summary>
        FirmwareVersion = 29,

        /// <summary>
        /// The meter id field.
        /// </summary>
        MeterId = 30,

        /// <summary>
        /// The flowrate field.
        /// </summary>
        FlowRate = 31,

        /// <summary>
        /// The today contract volume field.
        /// </summary>
        TodayContractVolume = 32,

        /// <summary>
        /// The yesterday contract volume field.
        /// </summary>
        YestContractVolume = 33,

        /// <summary>
        /// The accumulated volume field.
        /// </summary>
        AccumulatedVolume = 34,

        /// <summary>
        /// The last calc period volume field.
        /// </summary>
        LastCalcPeriodVol = 35,

        /// <summary>
        /// The diff pressure field.
        /// </summary>
        DiffPress = 36,

        /// <summary>
        /// The static pressure field.
        /// </summary>
        StaticPressure = 37,

        /// <summary>
        /// The todays mass field.
        /// </summary>
        TodaysMass = 38,

        /// <summary>
        /// The yesterdays mass field.
        /// </summary>
        YesterdaysMass = 39,

        /// <summary>
        /// The accumulated mass field.
        /// </summary>
        AccumulatedMass = 40,

        /// <summary>
        /// The pulsecount field.
        /// </summary>
        PulseCount = 41,

        /// <summary>
        /// The battery voltage field.
        /// </summary>
        BatteryVoltage = 42,

        /// <summary>
        /// The solar voltage field.
        /// </summary>
        SolarVoltage = 43,

        /// <summary>
        /// The energy rate field.
        /// </summary>
        EnergyRate = 44,

        /// <summary>
        /// The todays energy field.
        /// </summary>
        TodaysEnergy = 45,

        /// <summary>
        /// The yesterdays energy field.
        /// </summary>
        YesterdaysEnergy = 46,

        /// <summary>
        /// The current mode field.
        /// </summary>
        CurrentMode = 47,

        /// <summary>
        /// The yesterdays flow field.
        /// </summary>
        InjectionPressure = 48,

        /// <summary>
        /// The todays flow field.
        /// </summary>
        TodaysFlow = 49,

        /// <summary>
        /// The production temp field.
        /// </summary>
        ProductionTemp = 50,

        /// <summary>
        /// The control value field.
        /// </summary>
        ControlValue = 51,

        /// <summary>
        /// The flowline pressure field.
        /// </summary>
        FlowlinePressure = 52,

        /// <summary>
        /// The production flow rate field.
        /// </summary>
        ProductionFlowRate = 53,

        /// <summary>
        /// The production pressure field.
        /// </summary>
        ProductionPressure = 54,

        /// <summary>
        /// The injection press field.
        /// </summary>
        InjectionPress = 55,

        /// <summary>
        /// The production today volume field.
        /// </summary>
        ProductionTodayVolume = 56,

        /// <summary>
        /// The production yesterday volume field.
        /// </summary>
        ProductionYesterdayVolume = 57,

        /// <summary>
        /// The gas injection rate field.
        /// </summary>
        GasInjectionRate = 58,

        /// <summary>
        /// The valve percentopen field.
        /// </summary>
        ValvePercentOpen = 59,

        /// <summary>
        /// The comms facility field.
        /// </summary>
        FacilityComms = 60,

        /// <summary>
        /// The facility enabled field.
        /// </summary>
        FacilityEnabled = 61,

        /// <summary>
        /// The facility tag count field.
        /// </summary>
        FacilityTagCount = 62,

        /// <summary>
        /// The facility alarm count field.
        /// </summary>
        FacilityAlarmCount = 63,

        /// <summary>
        /// The facility host alarm field.
        /// </summary>
        FacilityHostAlarm = 64,

        /// <summary>
        /// The valve facility comment field.
        /// </summary>
        FacilityComment = 65,

        /// <summary>
        /// The valve seal field.
        /// </summary>
        Seal = 66,

        /// <summary>
        /// The valve seal series field.
        /// </summary>
        SealSeries = 67,

        /// <summary>
        /// The valve seal model field.
        /// </summary>
        SealModel = 68,

        /// <summary>
        /// The valve cable field.
        /// </summary>
        Cable = 69,

        /// <summary>
        /// The valve cable series field.
        /// </summary>
        CableSeries = 70,

        /// <summary>
        /// The valve cable description field.
        /// </summary>
        CableDescription = 71,

        /// <summary>
        /// The valve cable type field.
        /// </summary>
        CableType = 72,

        /// <summary>
        /// The valve motor series field.
        /// </summary>
        MotorSeries = 73,

        /// <summary>
        /// The valve motor model field.
        /// </summary>
        MotorModel = 74,

        /// <summary>
        /// The valve motor field.
        /// </summary>
        Motor = 75,

        /// <summary>
        /// The valve motor lead field.
        /// </summary>
        MotorLead = 76,

        /// <summary>
        /// The valve motor lead description field.
        /// </summary>
        MotorLeadDescription = 77,

        /// <summary>
        /// The valve motor lead series field.
        /// </summary>
        MotorLeadSeries = 78,

        /// <summary>
        /// The valve motor lead type field.
        /// </summary>
        MotorLeadType = 79,

        /// <summary>
        /// The valve pump field.
        /// </summary>
        Pump = 80,

        /// <summary>
        /// The valve Mamnufacturer field.
        /// </summary>
        Manufacturer = 81,

        /// <summary>
        /// The valve fluid level field.
        /// </summary>
        FluidLevel = 82,

        /// <summary>
        /// The Pump Efficiency Percentage field.
        /// </summary>
        PumpEfficiencyPercentage = 83,

        /// <summary>
        /// The Current Load Factor field.
        /// </summary>
        CurrentLoadFactor = 84,

        /// <summary>
        /// The Intermitten Real Time field.
        /// </summary>
        IntermittenRealTime = 85,

        /// <summary>
        /// The First Head field.
        /// </summary>
        FirstHead = 86,

        /// <summary>
        /// The Second Head field.
        /// </summary>
        SecondHead = 87,

        /// <summary>
        /// The Tube ID field.
        /// </summary>
        TubeID = 88,

        /// <summary>
        /// The Tube Description field.
        /// </summary>
        TubeDescription = 89,

        /// <summary>
        /// The Last Sample field.
        /// </summary>
        LastSample = 90,

        /// <summary>
        /// The Temperature field.
        /// </summary>
        Temperature = 91,

        /// <summary>
        /// The Volume field.
        /// </summary>
        Volume = 92,

        /// <summary>
        /// The Tank Level field.
        /// </summary>
        TankLevel = 93,

        /// <summary>
        /// The Interface Level field.
        /// </summary>
        InterfaceLevel = 94,

        /// <summary>
        /// The Station ID field.
        /// </summary>
        StationID = 95,

        /// <summary>
        /// The Location field.
        /// </summary>
        Location = 96,

        /// <summary>
        /// The Device ID field.
        /// </summary>
        DeviceID = 97,

        /// <summary>
        /// The Meter field.
        /// </summary>
        Meter = 98,

        /// <summary>
        /// The PID Type field.
        /// </summary>
        PIDType = 99,

        /// <summary>
        /// The PID Mode field.
        /// </summary>
        PIDMode = 100,

        /// <summary>
        /// The controller type field.
        /// </summary>
        ControllerType = 101,

        /// <summary>
        /// The controller node field.
        /// </summary>
        ControllerAndNode = 102,

        /// <summary>
        /// The Gas Flow Rate field.
        /// </summary>
        GasRate = 103,

    }
}