namespace Theta.XSPOC.Apex.Api.Common.Communications.Models
{
    /// <summary>
    /// Represents the available actions which can be executed on an xspoc system through a transaction.
    /// </summary>
    public enum ActionType
    {

        /// <summary>
        /// The read action type.
        /// </summary>
        Read = 0,

        /// <summary>
        /// The write action type.
        /// </summary>
        Write = 1,

        /// <summary>
        /// The well control action type.
        /// </summary>
        WellControl = 2,

        /// <summary>
        /// The start port logging action type.
        /// </summary>
        StartPortLogging = 3,

        /// <summary>
        /// The stop port logging action type.
        /// </summary>
        StopPortLogging = 4,

        /// <summary>
        /// The get card action type.
        /// </summary>
        GetCard = 5,

        /// <summary>
        /// The get card direct action type.
        /// </summary>
        GetCardDirect = 6,

        /// <summary>
        /// The email analysis action type.
        /// </summary>
        EmailAnalysis = 7,

        /// <summary>
        /// The node id task action type.
        /// </summary>
        NodeIDTask = 8,

        /// <summary>
        /// The request esp logs action type.
        /// </summary>
        RequestESPLogs = 9,

        /// <summary>
        /// The log it server action type.
        /// </summary>
        LogItServer = 10,

        /// <summary>
        /// The request pcsf trend data action type.
        /// </summary>
        RequestPCSFTrendData = 11,

        /// <summary>
        /// The request efm trend data action type.
        /// </summary>
        RequestEFMTrendData = 12,

        /// <summary>
        /// The request efm custody transfer data action type.
        /// </summary>
        RequestEFMCustodyTransferData = 13,

        /// <summary>
        /// The request lact batch log data action type.
        /// </summary>
        RequestLACTBatchLogData = 14,

        /// <summary>
        /// The request esp analysis result action type.
        /// </summary>
        RequestESPAnalysisResult = 15,

        /// <summary>
        /// The request health monitor log list action type.
        /// </summary>
        RequestHealthMonitorLogList = 16,

        /// <summary>
        /// The request health monitor log content action type.
        /// </summary>
        RequestHealthMonitorLogContent = 17,

        /// <summary>
        /// The request health monitor restart server action type.
        /// </summary>
        RequestHealthMonitorRestartServer = 18,

        /// <summary>
        /// The request health monitor restart scheduler action type.
        /// </summary>
        RequestHealthMonitorRestartScheduler = 19,

        /// <summary>
        /// The send push notification action type.
        /// </summary>
        SendPushNotification = 20,

        /// <summary>
        /// The get card direct with dh action type.
        /// </summary>
        GetCardDirectWithDH = 21,

    }
}
