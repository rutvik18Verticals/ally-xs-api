namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Defines the action for processing.
    /// </summary>
    public enum Action
    {

        /// <summary>
        /// The action is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Process the device scan data.
        /// </summary>
        ProcessDeviceScan = 1,

        /// <summary>
        /// Process the start well data.
        /// </summary>
        StartWell = 2,

        /// <summary>
        /// Process the stop well data.
        /// </summary>
        StopWell = 3,

        /// <summary>
        /// Process the idle well data.
        /// </summary>
        IdleWell = 4,

        /// <summary>
        /// Process the clear alarms data.
        /// </summary>
        ClearAlarms = 5,

        /// <summary>
        /// Process the communication log
        /// </summary>
        CommunicationLog = 18,

        /// <summary>
        /// Process the constant run mode data.
        /// </summary>
        ConstantRunMode = 6,

        /// <summary>
        /// Process the poc mode data.
        /// </summary>
        PocMode = 7,

        /// <summary>
        /// Process the percent timer mode data.
        /// </summary>
        PercentTimerMode = 8,

        /// <summary>
        /// Process the set clock data.
        /// </summary>
        SetClock = 9,

        /// <summary>
        /// Process the update poc data.
        /// </summary>
        UpdatePocData = 10,

        /// <summary>
        /// Process the get card data.
        /// </summary>
        GetCard = 11,

        /// <summary>
        /// Process the download equipment data.
        /// </summary>
        DownloadEquipment = 12,

        /// <summary>
        /// Process the upload equipment data.
        /// </summary>
        UploadEquipment = 13,

        /// <summary>
        /// Process the enable pid data.
        /// </summary>
        EnablePid = 14,

        /// <summary>
        /// Process the disable pid data.
        /// </summary>
        DisablePid = 15,

        /// <summary>
        /// Process the get data data.
        /// </summary>
        GetData = 16,

        /// <summary>
        /// Process get card direct data.
        /// </summary>
        GetCardDirect = 17,

        /// <summary>
        /// Process capture register log data.
        /// </summary>
        CaptureRegisterLogs = 19,

        /// <summary>
        /// Process on off cycles data.
        /// </summary>
        OnOffCycles = 20,

        /// <summary>
        /// Process start port logging data.
        /// </summary>
        StartPortLogging = 21,

        /// <summary>
        /// Process stop port logging data.
        /// </summary>
        StopPortLogging = 22,

    }
}
