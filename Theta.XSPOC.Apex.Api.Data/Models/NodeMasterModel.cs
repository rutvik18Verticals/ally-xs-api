using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the node master data model.
    /// </summary>
    public class NodeMasterModel
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Node.
        /// </summary>
        public string Node { get; set; }

        /// <summary>
        /// Gets or sets the communication status.
        /// </summary>
        public string CommStatus { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the last good scan time.
        /// </summary>
        public DateTime? LastGoodScanTime { get; set; }

        /// <summary>
        /// Gets or sets the run status.
        /// </summary>
        public string RunStatus { get; set; }

        /// <summary>
        /// Gets or sets the high priority alarm.
        /// </summary>
        public string HighPriAlarm { get; set; }

        /// <summary>
        /// Gets or sets the number of communication attempts.
        /// </summary>
        public short? CommAttempt { get; set; }

        /// <summary>
        /// Gets or sets the number of communication successes.
        /// </summary>
        public short? CommSuccess { get; set; }

        /// <summary>
        /// Gets or sets the data collection flag.
        /// </summary>
        public bool DataCollection { get; set; }

        /// <summary>
        /// Gets or sets the Positioner flag.
        /// </summary>
        public bool Positioner { get; set; }

        /// <summary>
        /// Gets or sets the string id.
        /// </summary>
        public int? StringId { get; set; }

        /// <summary>
        /// Gets or sets the ad hoc group 1.
        /// </summary>
        public string AdhocGroup1 { get; set; }

        /// <summary>
        /// Gets or sets the ad hoc group 2.
        /// </summary>
        public string AdhocGroup2 { get; set; }

        /// <summary>
        /// Gets or sets the ad hoc group 3.
        /// </summary>
        public string AdhocGroup3 { get; set; }

        /// <summary>
        /// Gets or sets the port id.
        /// </summary>
        public short? PortId { get; set; }

        /// <summary>
        /// Gets or sets the poc type.
        /// </summary>
        public short PocType { get; set; }

        /// <summary>
        /// Gets or sets the group shutdown flag.
        /// </summary>
        public bool GroupSdflag { get; set; }

        /// <summary>
        /// Gets or sets the other well id.
        /// </summary>
        public string OtherWellId1 { get; set; }

        /// <summary>
        /// Gets or sets the today runtime.
        /// </summary>
        public float? TodayRuntime { get; set; }

        /// <summary>
        /// Gets or sets the today cycles.
        /// </summary>
        public int? TodayCycles { get; set; }

        /// <summary>
        /// Gets or sets the time in state.
        /// </summary>
        public int? TimeInState { get; set; }

        /// <summary>
        /// Gets or sets the last condition of the pump from XDiag.
        /// </summary>
        public string LastAnalCondition { get; set; }

        /// <summary>
        /// Gets or sets the runtime acc GO.
        /// </summary>
        public int? RuntimeAccGo { get; set; }

        /// <summary>
        /// Gets or sets the runtime acc Go Yesterday.
        /// </summary>
        public int? RuntimeAccGoyest { get; set; }

        /// <summary>
        /// Gets or sets the starts acc GO.
        /// </summary>
        public int? StartsAccGo { get; set; }

        /// <summary>
        /// Gets or sets the starts acc GO yesterday.
        /// </summary>
        public int? StartsAccGoyest { get; set; }

        /// <summary>
        /// Gets or sets the kilo watt hours.
        /// </summary>
        public int? Kwh { get; set; }

        /// <summary>
        /// Gets or sets the runtime acc GO YY.
        /// </summary>
        public int? RuntimeAccGoyy { get; set; }

        /// <summary>
        /// Gets or sets the starts acc GO YY.
        /// </summary>
        public int? StartsAccGoyy { get; set; }

        /// <summary>
        /// Gets or sets the host alarm.
        /// </summary>
        public string HostAlarm { get; set; }

        /// <summary>
        /// Gets or sets the last good scan date.
        /// </summary>
        public DateTime? LastGoodHistScan { get; set; }

        /// <summary>
        /// Gets or sets the filter id.
        /// </summary>
        public int? FiterId { get; set; }

        /// <summary>
        /// Gets or sets the inferred production.
        /// </summary>
        public float? InferredProd { get; set; }

        /// <summary>
        /// Gets or sets the filter id.
        /// </summary>
        public int? FilterId { get; set; }

        /// <summary>
        /// Gets or sets the voice node id.
        /// </summary>
        public string VoiceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the disable code version.
        /// </summary>
        public string DisableCode { get; set; }

        /// <summary>
        /// Gets or sets the recorded strokes per minute.
        /// </summary>
        public float? RecSpm { get; set; }

        /// <summary>
        /// Gets or sets the last alarm date.
        /// </summary>
        public DateTime? LastAlarmDate { get; set; }

        /// <summary>
        /// Gets or sets the map id.
        /// </summary>
        public int? MapId { get; set; }

        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the tech note.
        /// </summary>
        public string TechNote { get; set; }

        /// <summary>
        /// Gets or sets the allow start lock flag.
        /// </summary>
        public bool? AllowStartLock { get; set; }

        /// <summary>
        /// Gets or sets the time zone offset.
        /// </summary>
        public float Tzoffset { get; set; }

        /// <summary>
        /// Gets or sets the honor day light savings flag.
        /// </summary>
        public bool Tzdaylight { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the last good history collection.
        /// </summary>
        public DateTime? LastGoodHistCollection { get; set; }

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
        public int? Apiport { get; set; }

        /// <summary>
        /// Gets or sets the Smarten UI username.
        /// </summary>
        public string Apiusername { get; set; }

        /// <summary>
        /// Gets or sets the Smarten UI password.
        /// </summary>
        public string Apipassword { get; set; }

        /// <summary>
        /// Gets or sets the creation date and time.
        /// </summary>
        public DateTime? CreationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the application id.
        /// </summary>
        public int? ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the default wndow id.
        /// </summary>
        public string DefaultWindowId { get; set; }

        /// <summary>
        /// Gets or sets the parent node id.
        /// </summary>
        public string ParentNodeId { get; set; }

        /// <summary>
        /// Gets or sets the asset guid.
        /// </summary>
        public Guid AssetGuid { get; set; }

        /// <summary>
        /// Gets or sets the asset enabled or not.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the company guid.
        /// </summary>
        public Guid? CompanyGuid { get; set; }

        /// <summary>
        /// Gets or sets the CustomerId.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer is valid or not.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the UserAccount.
        /// </summary>
        public string UserAccount { get; set; }

    }
}
