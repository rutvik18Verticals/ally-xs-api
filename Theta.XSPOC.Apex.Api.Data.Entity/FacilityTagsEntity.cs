using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The System FacilityTags table.
    /// </summary>
    [Table("tblFacilityTags")]
    public class FacilityTagsEntity
    {

        /// <summary>
        /// Get and set the NodeId.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(100)]
        public string NodeId { get; set; }

        /// <summary>
        /// Get and set the Address.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Get and set the Description.
        /// </summary>
        [MaxLength(1024)]
        public string Description { get; set; }

        /// <summary>
        /// Get and set the Enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Get and set the TrendType.
        /// </summary>
        public int? TrendType { get; set; }

        /// <summary>
        /// Get and set the RawLo.
        /// </summary>
        public int? RawLo { get; set; }

        /// <summary>
        /// Get and set the RawHi.
        /// </summary>
        public int? RawHi { get; set; }

        /// <summary>
        /// Get and set the EngLo.
        /// </summary>
        public float? EngLo { get; set; }

        /// <summary>
        /// Get and set the EngHi.
        /// </summary>
        public float? EngHi { get; set; }

        /// <summary>
        /// Get and set the LimitLo.
        /// </summary>
        public float? LimitLo { get; set; }

        /// <summary>
        /// Get and set the LimitHi.
        /// </summary>
        public float? LimitHi { get; set; }

        /// <summary>
        /// Get and set the CurrentValue.
        /// </summary>
        public float? CurrentValue { get; set; }

        /// <summary>
        /// Get and set the EngUnits.
        /// </summary>
        [MaxLength(50)]
        [Unicode(false)]
        public string EngUnits { get; set; }

        /// <summary>
        /// Get and set the UpdateDate.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Get and set the Writeable.
        /// </summary>
        public bool Writeable { get; set; }

        /// <summary>
        /// Get and set the Topic.
        /// </summary>
        [MaxLength(50)]
        [Unicode(false)]
        public string Topic { get; set; }

        /// <summary>
        /// Get and set the GroupNodeId.
        /// </summary>
        [Required]
        [Column("GroupNodeID")]
        [MaxLength(50)]
        public string GroupNodeId { get; set; }

        /// <summary>
        /// Get and set the DisplayOrder.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Get and set the AlarmState.
        /// </summary>
        public int AlarmState { get; set; }

        /// <summary>
        /// Get and set the AlarmAction.
        /// </summary>
        public int AlarmAction { get; set; }

        /// <summary>
        /// Get and set the WellGroupName.
        /// </summary>
        [MaxLength(50)]
        [Unicode(false)]
        public string WellGroupName { get; set; }

        /// <summary>
        /// Get and set the PagingGroup.
        /// </summary>
        [MaxLength(50)]
        [Unicode(false)]
        public string PagingGroup { get; set; }

        /// <summary>
        /// Get and set the AlarmArg.
        /// </summary>
        [MaxLength(50)]
        [Unicode(false)]
        public string AlarmArg { get; set; }

        /// <summary>
        /// Get and set the AlarmTextLo.
        /// </summary>
        [MaxLength(255)]
        public string AlarmTextLo { get; set; }

        /// <summary>
        /// Get and set the AlarmTextHi.
        /// </summary>
        [MaxLength(255)]
        public string AlarmTextHi { get; set; }

        /// <summary>
        /// Get and set the AlarmTextClear.
        /// </summary>
        [MaxLength(255)]
        public string AlarmTextClear { get; set; }

        /// <summary>
        /// Get and set the GroupStatusView.
        /// </summary>
        public int? GroupStatusView { get; set; }

        /// <summary>
        /// Get and set the ResponderListId.
        /// </summary>
        [Column("ResponderListID")]
        public int? ResponderListId { get; set; }

        /// <summary>
        /// Get and set the VoiceTextLo.
        /// </summary>
        [MaxLength(255)]
        public string VoiceTextLo { get; set; }

        /// <summary>
        /// Get and set the VoiceTextHi.
        /// </summary>
        [MaxLength(255)]
        public string VoiceTextHi { get; set; }

        /// <summary>
        /// Get and set the VoiceTextClear.
        /// </summary>
        [MaxLength(255)]
        public string VoiceTextClear { get; set; }

        /// <summary>
        /// Get and set the DataType.
        /// </summary>
        public int? DataType { get; set; }

        /// <summary>
        /// Get and set the Decimals.
        /// </summary>
        public int? Decimals { get; set; }

        /// <summary>
        /// Get and set the VoiceNodeId.
        /// </summary>
        [Column("VoiceNodeID")]
        [MaxLength(50)]
        public string VoiceNodeId { get; set; }

        /// <summary>
        /// Get and set the ContactListID.
        /// </summary>
        [Column("ContactListID")]
        public int? ContactListId { get; set; }

        /// <summary>
        /// Get and set the Bit.
        /// </summary>
        public int Bit { get; set; }

        /// <summary>
        /// Get and set the Deadband.
        /// </summary>
        public float Deadband { get; set; }

        /// <summary>
        /// Get and set the DestinationType.
        /// </summary>
        public int? DestinationType { get; set; }

        /// <summary>
        /// Get and set the StateId.
        /// </summary>
        [Column("StateID")]
        public int? StateId { get; set; }

        /// <summary>
        /// Get and set the CaptureType.
        /// </summary>
        public int CaptureType { get; set; }

        /// <summary>
        /// Get and set the LastCaptureDate.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? LastCaptureDate { get; set; }

        /// <summary>
        /// Get and set the NumConsecAlarmsAllowed.
        /// </summary>
        public int? NumConsecAlarmsAllowed { get; set; }

        /// <summary>
        /// Get and set the NumConsecAlarms.
        /// </summary>
        public int? NumConsecAlarms { get; set; }

        /// <summary>
        /// Get and set the UnitType.
        /// </summary>
        public short UnitType { get; set; }

        /// <summary>
        /// Get and set the Name.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Get and set the FacilityTagGroupId.
        /// </summary>
        [Column("FacilityTagGroupID")]
        public int? FacilityTagGroupId { get; set; }

        /// <summary>
        /// Get and set the ParamStandardType.
        /// </summary>
        public int? ParamStandardType { get; set; }

        /// <summary>
        /// Get and set the Latitude.
        /// </summary>
        [Column(TypeName = "decimal(8, 6)")]
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Get and set the Longitude.
        /// </summary>
        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Longitude { get; set; }

        /// <summary>
        /// Get and set the ArchiveFunction.
        /// </summary>
        public int ArchiveFunction { get; set; }

        /// <summary>
        /// Get and set the Tag.
        /// </summary>
        [MaxLength(50)]
        [Unicode(false)]
        public string Tag { get; set; }

        /// <summary>
        /// Get and set the DetailViewOnly.
        /// </summary>
        public bool? DetailViewOnly { get; set; }

        /// <summary>
        /// Get and set the Expression.
        /// </summary>
        [MaxLength(200)]
        public string Expression { get; set; }

        /// <summary>
        /// Get and set the StringValue.
        /// </summary>
        public string StringValue { get; set; }

    }
}
