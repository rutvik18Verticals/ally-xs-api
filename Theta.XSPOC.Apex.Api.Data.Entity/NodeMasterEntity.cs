using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// THe node master table.
    /// </summary>
    [Table("tblNodeMaster")]
    public class NodeMasterEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID", TypeName = "nvarchar")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(50)]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Node.
        /// </summary>
        [Column("Node")]
        [MaxLength(255)]
        public string Node { get; set; }

        /// <summary>
        /// Gets or sets the Enabled.
        /// </summary>
        [Column("Enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the communication status.
        /// </summary>
        [Column("CommStatus")]
        public string CommStatus { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [Column("Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the last good scan time.
        /// </summary>
        [Column("LastGoodScanTime")]
        public DateTime? LastGoodScanTime { get; set; }

        /// <summary>
        /// Gets or sets the run status.
        /// </summary>
        [Column("RunStatus")]
        [MaxLength(50)]
        public string RunStatus { get; set; }

        /// <summary>
        /// Gets or sets the high priority alarm.
        /// </summary>
        [Column("HighPriAlarm")]
        public string HighPriAlarm { get; set; }

        /// <summary>
        /// Gets or sets the number of communication attempts.
        /// </summary>
        [Column("CommAttempt")]
        public short? CommAttempt { get; set; }

        /// <summary>
        /// Gets or sets the number of communication successes.
        /// </summary>
        [Column("CommSuccess")]
        public short? CommSuccess { get; set; }

        /// <summary>
        /// Gets or sets the data collection flag.
        /// </summary>
        [Column("DataCollection")]
        public bool DataCollection { get; set; }

        /// <summary>
        /// Gets or sets the Positioner flag.
        /// </summary>
        [Column("Positioner")]
        public bool Positioner { get; set; }

        /// <summary>
        /// Gets or sets the string id.
        /// </summary>
        [Column("StringID")]
        public int? StringId { get; set; }

        /// <summary>
        /// Gets or sets the ad hoc group 1.
        /// </summary>
        [Column("AdhocGroup1")]
        public string AdhocGroup1 { get; set; }

        /// <summary>
        /// Gets or sets the ad hoc group 2.
        /// </summary>
        [Column("AdhocGroup2")]
        public string AdhocGroup2 { get; set; }

        /// <summary>
        /// Gets or sets the ad hoc group 3.
        /// </summary>
        [Column("AdhocGroup3")]
        public string AdhocGroup3 { get; set; }

        /// <summary>
        /// Gets or sets the paging enabled flag.
        /// </summary>
        [Column("PagingEnabled")]
        public bool PagingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the port id.
        /// </summary>
        [Column("PortID")]
        public short? PortId { get; set; }

        /// <summary>
        /// Gets or sets the runtime 24 flag.
        /// </summary>
        [Column("Runtime24Flag")]
        public bool Runtime24Flag { get; set; }

        /// <summary>
        /// Gets or sets the poc type.
        /// </summary>
        [Column("POCType")]
        public short PocType { get; set; }

        /// <summary>
        /// Gets or sets the group shutdown flag.
        /// </summary>
        [Column("GroupSdflag")]
        public bool GroupSdflag { get; set; }

        /// <summary>
        /// Gets or sets the other well id.
        /// </summary>
        [Column("OtherWellId1")]
        public string OtherWellId1 { get; set; }

        /// <summary>
        /// Gets or sets the today runtime.
        /// </summary>
        [Column("TodayRuntime")]
        public float? TodayRuntime { get; set; }

        /// <summary>
        /// Gets or sets the today cycles.
        /// </summary>
        [Column("TodayCycles")]
        public int? TodayCycles { get; set; }

        /// <summary>
        /// Gets or sets the time in state.
        /// </summary>
        [Column("TimeInState")]
        public int? TimeInState { get; set; }

        /// <summary>
        /// Gets or sets the last condition of the pump from XDiag.
        /// </summary>
        [Column("LastAnalCondition")]
        public string LastAnalCondition { get; set; }

        /// <summary>
        /// Gets or sets the well operating type.
        /// </summary>
        [Column("WellOpType")]
        public int? WellOperatingType { get; set; }

        /// <summary>
        /// Gets or sets the pump fillage.
        /// </summary>
        [Column("PumpFillage")]
        public int? PumpFillage { get; set; }

        /// <summary>
        /// Gets or sets the baker version.
        /// </summary>
        [Column("BakerVersion")]
        public int? BakerVersion { get; set; }

        /// <summary>
        /// Gets or sets the runtime acc.
        /// </summary>
        [Column("RuntimeAcc")]
        public int? RuntimeAcc { get; set; }

        /// <summary>
        /// Gets or sets the runtime acc GO.
        /// </summary>
        [Column("RuntimeAccGo")]
        public int? RuntimeAccGo { get; set; }

        /// <summary>
        /// Gets or sets the runtime acc Go Yesterday.
        /// </summary>
        [Column("RuntimeAccGoyest")]
        public int? RuntimeAccGoyest { get; set; }

        /// <summary>
        /// Gets or sets the runtime since start.
        /// </summary>
        [Column("RuntimeSinceStart")]
        public int? RuntimeSinceStart { get; set; }

        /// <summary>
        /// Gets or sets the starts acc.
        /// </summary>
        [Column("StartsAcc")]
        public int? StartsAcc { get; set; }

        /// <summary>
        /// Gets or sets the starts acc GO.
        /// </summary>
        [Column("StartsAccGo")]
        public int? StartsAccGo { get; set; }

        /// <summary>
        /// Gets or sets the starts acc GO yesterday.
        /// </summary>
        [Column("StartsAccGoyest")]
        public int? StartsAccGoyest { get; set; }

        /// <summary>
        /// Gets or sets the kilo watt hours.
        /// </summary>
        [Column("Kwh")]
        public int? Kwh { get; set; }

        /// <summary>
        /// Gets or sets the runtime acc GO YY.
        /// </summary>
        [Column("RuntimeAccGoyy")]
        public int? RuntimeAccGoyy { get; set; }

        /// <summary>
        /// Gets or sets the starts acc GO YY.
        /// </summary>
        [Column("StartsAccGoyy")]
        public int? StartsAccGoyy { get; set; }

        /// <summary>
        /// Gets or sets the energy mode.
        /// </summary>
        [Column("EnergyMode")]
        public int? EnergyMode { get; set; }

        /// <summary>
        /// Gets or sets the energy group.
        /// </summary>
        [Column("EnergyGroup")]
        public int? EnergyGroup { get; set; }

        /// <summary>
        /// Gets or sets the percent communications yesterday.
        /// </summary>
        [Column("PctCommYest")]
        public int? PercentCommunicationsYesterday { get; set; }

        /// <summary>
        /// Gets or sets the host alarm.
        /// </summary>
        [Column("HostAlarm")]
        [MaxLength(100)]
        public string HostAlarm { get; set; }

        /// <summary>
        /// Gets or sets the host alarm state.
        /// </summary>
        [Column("HostAlarmState")]
        public short? HostAlarmState { get; set; }

        /// <summary>
        /// Gets or sets the last good scan date.
        /// </summary>
        [Column("LastGoodHistScan")]
        public DateTime? LastGoodHistScan { get; set; }

        /// <summary>
        /// Gets or sets the filter id.
        /// </summary>
        [Column("FiterId")]
        public int? FiterId { get; set; }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        [Column("FirmwareVersion")]
        public float? FirmwareVersion { get; set; }

        /// <summary>
        /// Gets or sets the inferred production.
        /// </summary>
        [Column("InferredProd")]
        public float? InferredProd { get; set; }

        /// <summary>
        /// Gets or sets the filter id.
        /// </summary>
        [Column("FilterId")]
        public int? FilterId { get; set; }

        /// <summary>
        /// Gets or sets the fast scan flag.
        /// </summary>
        [Column("FastScan")]
        public bool? FastScan { get; set; }

        /// <summary>
        /// Gets or sets the voice node id.
        /// </summary>
        [Column("VoiceNodeId")]
        public string VoiceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the yesterday runtime percent.
        /// </summary>
        [Column("YestRuntimePct")]
        public float? YesterdayRuntimePercent { get; set; }

        /// <summary>
        /// Gets or sets the today runtime percent.
        /// </summary>
        [Column("TodayRuntimePct")]
        public float? TodayRuntimePercent { get; set; }

        /// <summary>
        /// Gets or sets the disable code version.
        /// </summary>
        [Column("DisableCode")]
        public string DisableCode { get; set; }

        /// <summary>
        /// Gets or sets the production potential.
        /// </summary>
        [Column("ProdPotential")]
        public int? ProductionPotential { get; set; }

        /// <summary>
        /// Gets or sets the recorded strokes per minute.
        /// </summary>
        [Column("RecSpm")]
        public float? RecSpm { get; set; }

        /// <summary>
        /// Gets or sets the last alarm date.
        /// </summary>
        [Column("LastAlarmDate")]
        public DateTime? LastAlarmDate { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        [Column("Latitude")]
        public float? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        [Column("Longitude")]
        public float? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the map id.
        /// </summary>
        [Column("MapId")]
        public int? MapId { get; set; }

        /// <summary>
        /// Gets or sets the consecutive communication failures.
        /// </summary>
        [Column("CommConsecFails")]
        public int? ConsecutiveCommunicationFailures { get; set; }

        /// <summary>
        /// Gets or sets the alarm action.
        /// </summary>
        [Column("AlarmAction")]
        public int AlarmAction { get; set; }

        /// <summary>
        /// Gets or sets the register block limit.
        /// </summary>
        [Column("RegisterBlockLimit")]
        public int? RegisterBlockLimit { get; set; }

        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        [Column("CompanyId")]
        public int? CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the tech note.
        /// </summary>
        [Column("TechNote")]
        public string TechNote { get; set; }

        /// <summary>
        /// Gets or sets the scan cluster.
        /// </summary>
        [Column("ScanCluster")]
        public int? ScanCluster { get; set; }

        // todo 
        // <summary>
        // Gets or sets the serial number.This appears to only be on hosted servers.
        // </summary>
        //[Column("SerialNumber")]
        //public int? SerialNumber { get; set; }

        // todo  
        // Gets or sets the install date. This appears to only be hosted servers.
        //
        //[Column("InstalledDate")]
        //public DateTime? InstalledDate { get; set; }

        /// <summary>
        /// Gets or sets the allow start lock flag.
        /// </summary>
        [Column("AllowStartLock")]
        public bool? AllowStartLock { get; set; }

        /// <summary>
        /// Gets or sets the time zone offset.
        /// </summary>
        [Column("TZOffset")]
        public float Tzoffset { get; set; }

        /// <summary>
        /// Gets or sets the honor day light savings flag.
        /// </summary>
        [Column("TZDaylight")]
        public bool Tzdaylight { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [Column("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the last good history collection.
        /// </summary>
        [Column("LastGoodHistCollection")]
        public DateTime? LastGoodHistCollection { get; set; }

        /// <summary>
        /// Gets or sets the comment 2.
        /// </summary>
        [Column("Comment2")]
        public string Comment2 { get; set; }

        /// <summary>
        /// Gets or sets the comment 3.
        /// </summary>
        [Column("Comment3")]
        public string Comment3 { get; set; }

        /// <summary>
        /// Gets or sets the Smarten UI Port.
        /// </summary>
        [Column("Apiport")]
        public int? Apiport { get; set; }

        /// <summary>
        /// Gets or sets the Smarten UI username.
        /// </summary>
        [Column("Apiusername")]
        public string Apiusername { get; set; }

        /// <summary>
        /// Gets or sets the Smarten UI password.
        /// </summary>
        [Column("Apipassword")]
        public string Apipassword { get; set; }

        /// <summary>
        /// Gets or sets the creation date and time.
        /// </summary>
        [Column("CreationDateTime")]
        public DateTime? CreationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the operational score.
        /// </summary>
        [Column("OperationalScore")]
        public float? OperationalScore { get; set; }

        /// <summary>
        /// Gets or sets the application id.
        /// </summary>
        [Column("ApplicationId")]
        public int? ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the default wndow id.
        /// </summary>
        [Column("DefaultWindowId")]
        public string DefaultWindowId { get; set; }

        /// <summary>
        /// Gets or sets the parent node id.
        /// </summary>
        [Column("ParentNodeId")]
        public string ParentNodeId { get; set; }

        /// <summary>
        /// Gets or sets the asset guid.
        /// </summary>
        [Column("AssetGuid")]
        public Guid AssetGuid { get; set; }

    }
}
