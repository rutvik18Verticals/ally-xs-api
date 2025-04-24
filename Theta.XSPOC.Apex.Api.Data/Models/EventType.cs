namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the different event types in the system.
    /// </summary>
    public enum EventType
    {

        /// <summary>
        /// Comment
        /// </summary>
        Comment = 1,

        /// <summary>
        /// Parameter Change
        /// </summary>
        ParamChange = 2,

        /// <summary>
        /// Status Change
        /// </summary>
        StatusChange = 3,

        /// <summary>
        /// RTU ( Remote Terminal Unit ) Alarm
        /// </summary>
        RTUAlarm = 4,

        /// <summary>
        /// Host Alarm
        /// </summary>
        HostAlarm = 5,

        /// <summary>
        /// Dyno ( Dynometer card ) Note
        /// </summary>
        DynoNote = 6,

        /// <summary>
        /// Well Configuration Change
        /// </summary>
        WellConfigChange = 7,

        /// <summary>
        /// Technician Note
        /// </summary>
        TechNote = 8,

        /// <summary>
        /// Production Potential
        /// </summary>
        ProdPotential = 9,

        /// <summary>
        /// Rec SPM
        /// </summary>
        RecSPM = 10,

        /// <summary>
        /// Other Well Id 1
        /// </summary>
        OtherWellID1 = 11,

        /// <summary>
        /// Log File
        /// </summary>
        LogFile = 12,

        /// <summary>
        /// EFM Hourly Log
        /// </summary>
        EFMHourlyLog = 13,

        /// <summary>
        /// EFM Daily Log
        /// </summary>
        EFMDailyLog = 14,

        /// <summary>
        /// EFM Event Log
        /// </summary>
        EFMEventLog = 15,
        /// <summary>
        /// Fillage Reviewed
        /// </summary>
        FillageReviewed = 16,

        /// <summary>
        /// ESP Shutdown Log
        /// </summary>
        ESPShutdownLog = 17,

        /// <summary>
        /// Comment 2
        /// </summary>
        Comment2 = 18,

        /// <summary>
        /// Comment 3
        /// </summary>
        Comment3 = 19,

        /// <summary>
        /// ESP Event Log
        /// </summary>
        ESPEventLog = 20,

        /// <summary>
        /// Enable/Disable
        /// </summary>
        EnableDisable = 21,

        /// <summary>
        /// Facility Tag Alarm
        /// </summary>
        FacilityTagAlarm = 23,

        /// <summary>
        /// Plunger Arrival
        /// </summary>
        PlungerArrival = 24,

        /// <summary>
        /// EventEntry
        /// </summary>
        EventEntry = 25,

        /// <summary>
        /// Parameter Saved Value
        /// </summary>
        ParameterSavedValue = 26,

        /// <summary>
        /// Setpoint Optimization Change
        /// </summary>
        SetpointOptimizationChange = 27,

    }
}
