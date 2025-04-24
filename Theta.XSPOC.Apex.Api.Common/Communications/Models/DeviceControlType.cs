namespace Theta.XSPOC.Apex.Api.Common.Communications.Models
{
    /// <summary>
    /// Describes the available control types able to be performed on a device.
    /// </summary>
    public enum DeviceControlType
    {

        /// <summary>
        /// The start well device control type.
        /// </summary>
        StartWell = 1,

        /// <summary>
        /// The stop well device control type.
        /// </summary>
        StopWell = 2,

        /// <summary>
        /// The idle well device control type.
        /// </summary>
        IdleWell = 3,

        /// <summary>
        /// The clear alarms device control type.
        /// </summary>
        ClearAlarms = 4,

        /// <summary>
        /// The constant run mode device control type.
        /// </summary>
        ConstantRunMode = 5,

        /// <summary>
        /// The poc mode
        /// </summary>
        PocMode = 6,

        /// <summary>
        /// The percent timer mode device control type.
        /// </summary>
        PercentTimerMode = 7,

        /// <summary>
        /// The scan device control type.
        /// </summary>
        Scan = 8,

        /// <summary>
        /// The set clock device control type.
        /// </summary>
        SetClock = 9,

        /// <summary>
        /// The power shutdown device control type.
        /// </summary>
        PowerShutdown = 10,

        /// <summary>
        /// The update poc data device control type.
        /// </summary>
        UpdatePOCData = 11,

        /// <summary>
        /// The fast scan device control type.
        /// </summary>
        FastScan = 12,

        /// <summary>
        /// The surface mode device control type.
        /// </summary>
        SurfaceMode = 13,

        /// <summary>
        /// The vfd mode device control type.
        /// </summary>
        VFDMode = 14,

        /// <summary>
        /// The pip control device control type.
        /// </summary>
        PIPControl = 15,

        /// <summary>
        /// The download device control type.
        /// </summary>
        Download = 16,

        /// <summary>
        /// The upload device control type.
        /// </summary>
        Upload = 17,

        /// <summary>
        /// The plunger lift 5 min device control type.
        /// </summary>
        PlungerLift5Min = 18,

        /// <summary>
        /// The schedule mode device control type.
        /// </summary>
        ScheduleMode = 19,

        /// <summary>
        /// The scan master and slaves device control type.
        /// </summary>
        ScanMasterAndSlaves = 20,

        /// <summary>
        /// The enable pid device control type.
        /// </summary>
        EnablePID = 25,

        /// <summary>
        /// The disable pid device control type.
        /// </summary>
        DisablePID = 26,

        /// <summary>
        /// The fixed speed manual mode device control type.
        /// </summary>
        FixedSpeedManualMode = 27,

        /// <summary>
        /// The fixed speed timer mode device control type.
        /// </summary>
        FixedSpeedTimerMode = 28,

        /// <summary>
        /// The fixed speed pump off mode device control type.
        /// </summary>
        FixedSpeedPumpOffMode = 29,

        /// <summary>
        /// The variable speed pump fillage mode device control type.
        /// </summary>
        VariableSpeedPumpFillageMode = 30,

        /// <summary>
        /// The variable speed manual mode device control type.
        /// </summary>
        VariableSpeedManualMode = 31,

        /// <summary>
        /// The variable speed pump off mode device control type.
        /// </summary>
        VariableSpeedPumpOffMode = 32,

        /// <summary>
        /// The fixed speed pump fillage mode device control type.
        /// </summary>
        FixedSpeedPumpFillageMode = 33,

    }
}
