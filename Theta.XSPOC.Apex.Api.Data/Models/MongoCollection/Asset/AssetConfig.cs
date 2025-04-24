using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset
{
    /// <summary>
    /// A class with configuration properties for assets.
    /// </summary>
    public class AssetConfig
    {

        /// <summary>
        /// Gets or sets the node address for device communication.
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// Gets or sets if the asset is enabled for communication.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the most current communication status.
        /// </summary>
        public string CommunicationStatus { get; set; }

        /// <summary>
        /// Gets or sets the comment on the asset.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the last good scan time.
        /// </summary>
        public DateTime? LastGoodScanTime { get; set; }

        /// <summary>
        /// Gets or sets the last reported run status string.
        /// </summary>
        public string RunStatus { get; set; }

        /// <summary>
        /// Gets or sets the last high priority alarm.
        /// </summary>
        public string HighPriorityAlarm { get; set; }

        /// <summary>
        /// Gets or sets the communication attempts.
        /// </summary>
        public int? CommunicationAttempts { get; set; }

        /// <summary>
        /// Gets or sets the communication successes.
        /// </summary>
        public int? CommunicationSuccesses { get; set; }

        /// <summary>
        /// Gets or sets if the data collection flag is set.
        /// </summary>
        public bool IsDataCollection { get; set; }

        /// <summary>
        /// Gets or sets if the positioner flag is set.
        /// </summary>
        public bool IsPositioner { get; set; }

        /// <summary>
        /// Gets or sets if the paging flag is set.
        /// </summary>
        public bool IsPagingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the port id.
        /// </summary>  
        [BsonRepresentation(BsonType.ObjectId)]
        public string PortId { get; set; }

        /// <summary>
        /// Gets or sets if the runtime 24 flag.
        /// </summary>
        public bool Runtime24Flag { get; set; }

        /// <summary>
        /// Gets or sets if the group shutdown flag is set.
        /// </summary>
        public bool GroupShutDownFlag { get; set; }

        /// <summary>
        /// Gets or sets the other well id, used for system integrations.
        /// </summary>
        public string OtherWellId1 { get; set; }

        /// <summary>
        /// Gets or sets the runtime today.
        /// </summary>
        public double? TodayRuntime { get; set; }

        /// <summary>
        /// Gets or sets the cycles for today.
        /// </summary>
        public int? TodayCycles { get; set; }

        /// <summary>
        /// Gets or sets the current time in state.
        /// </summary>
        public int? TimeInState { get; set; }

        /// <summary>
        /// Gets or sets the last analysis condition.
        /// </summary>
        public string LastAnalysisCondition { get; set; }

        /// <summary>
        /// Gets or sets the well operating type.
        /// </summary>
        public int? WellOperatingType { get; set; }

        /// <summary>
        /// Gets or sets the last reported pump fillage.
        /// </summary>
        public int? PumpFillage { get; set; }

        /// <summary>
        /// Gets or sets the baker version.
        /// </summary>
        public int? BakerVersion { get; set; }

        /// <summary>
        /// Gets or sets the runtime acc.
        /// </summary>
        public int? RuntimeAcc { get; set; }

        /// <summary>
        /// Gets or sets the runtime acc GO.
        /// </summary>
        public int? RuntimeAccGO { get; set; }

        /// <summary>
        /// Gets or sets the runtime acc Go Yesterday.
        /// </summary>
        public int? RuntimeAccGOYesterday { get; set; }

        /// <summary>
        /// Gets or sets the runtime since start.
        /// </summary>
        public int? RuntimeSinceStart { get; set; }

        /// <summary>
        /// Gets or sets the starts acc.
        /// </summary>
        public int? StartsAcc { get; set; }

        /// <summary>
        /// Gets or sets the starts acc GO.
        /// </summary>
        public int? StartsAccGO { get; set; }

        /// <summary>
        /// Gets or sets the starts acc Go yesterday.
        /// </summary>
        public int? StartsAccGOYesterday { get; set; }

        /// <summary>
        /// Gets or sets the kilo watt hours.
        /// </summary>
        public int? KiloWattHours { get; set; }

        /// <summary>
        /// Gets or sets the runtime acc GO YY.
        /// </summary>
        public int? RuntimeAccGOYY { get; set; }

        /// <summary>
        /// Gets or sets the starts acc GO YY.
        /// </summary>
        public int? StartsAccGOYY { get; set; }

        /// <summary>
        /// Gets or sets the energy mode.
        /// </summary>
        public int? EnergyMode { get; set; }

        /// <summary>
        /// Gets or sets the energy group.
        /// </summary>
        public int? EnergyGroup { get; set; }

        /// <summary>
        /// Gets or sets the percent communications yesterday.
        /// </summary>
        public int? PercentCommunicationsYesterday { get; set; }

        /// <summary>
        /// Gets or sets the last good history scan date.
        /// </summary>
        public DateTime? LastGoodHistoryScan { get; set; }

        /// <summary>
        /// Gets or sets the filter id.
        /// </summary>
        public int? FiterId { get; set; }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        public double? FirmwareVersion { get; set; }

        /// <summary>
        /// Gets or sets the inferred production.
        /// </summary>
        public double? InferredProduction { get; set; }

        /// <summary>
        /// Gets or sets the filter id.
        /// </summary>
        public int? FilterId { get; set; }

        /// <summary>
        /// Gets or sets the fast scan flag.
        /// </summary>
        public bool? FastScan { get; set; }

        /// <summary>
        /// Gets or sets the voice node id.
        /// </summary>
        public string VoiceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the yesterday runtime percent.
        /// </summary>
        public double? YesterdayRuntimePercent { get; set; }

        /// <summary>
        /// Gets or sets the today runtime percent.
        /// </summary>
        public double? TodayRuntimePercent { get; set; }

        /// <summary>
        /// Gets or sets the disable code version.
        /// </summary>
        public string DisableCode { get; set; }

        /// <summary>
        /// Gets or sets the production potential.
        /// </summary>
        public int? ProductionPotential { get; set; }

        /// <summary>
        /// Gets or sets the recorded strokes per minute.
        /// </summary>
        public double? RecordedStrokesPerMinute { get; set; }

        /// <summary>
        /// Gets or sets the last alarm date.
        /// </summary>
        public DateTime? LastAlarmDate { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the map id.
        /// </summary>
        public int? MapId { get; set; }

        /// <summary>
        /// Gets or sets the consecutive communication failures.
        /// </summary>
        public int? ConsecutiveCommunicationFailures { get; set; }

        /// <summary>
        /// Gets or sets the alarm action.
        /// </summary>
        public int AlarmAction { get; set; }

        /// <summary>
        /// Gets or sets the register block limit.
        /// </summary>
        public int? RegisterBlockLimit { get; set; }

        /// <summary>
        /// Gets or sets the tech note.
        /// </summary>
        public string TechNote { get; set; }

        /// <summary>
        /// Gets or sets the scan cluster.
        /// </summary>
        public int? ScanCluster { get; set; }

        /// <summary>
        /// Gets or sets the serial number.This appears to only be on hosted servers.
        /// </summary>
        public int? SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the install date.
        /// </summary>
        public DateTime? InstalledDate { get; set; }

        /// <summary>
        /// Gets or sets the allow start lock flag.
        /// </summary>
        public bool? AllowStartLock { get; set; }

        /// <summary>
        /// Gets or sets the time zone offset.
        /// </summary>
        public double TimeZoneOffset { get; set; }

        /// <summary>
        /// Gets or sets the honor day light savings flag.
        /// </summary>
        public bool HonorDayLightSavings { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the last good history collection.
        /// </summary>
        public DateTime? LastGoodHistoryCollection { get; set; }

        /// <summary>
        /// Gets or sets the comment 2.
        /// </summary>
        public string Comment2 { get; set; }

        /// <summary>
        /// Gets or sets the comment 3.
        /// </summary>
        public string Comment3 { get; set; }

        /// <summary>
        /// Gets or sets the Smarten UI Port.
        /// </summary>
        public int? SmartenUIPort { get; set; }

        /// <summary>
        /// Gets or sets the Smarten UI username.
        /// </summary>
        public string SmartenUIUsername { get; set; }

        /// <summary>
        /// Gets or sets the Smarten UI password.
        /// </summary>
        public string SmartenUIPassword { get; set; }

        /// <summary>
        /// Gets or sets the creation date and time.
        /// </summary>
        public DateTime? CreationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the operational score.
        /// </summary>
        public double? OperationalScore { get; set; }

        /// <summary>
        /// Gets or sets the default window id.
        /// </summary>
        public string DefaultWindowId { get; set; }

        /// <summary>
        /// Gets or sets the parent node id.
        /// </summary>
        public string ParentNodeId { get; set; }

        /// <summary>
        /// Gets or sets the time zone id.
        /// </summary>
        public string TimeZoneId { get; set; }

    }
}
