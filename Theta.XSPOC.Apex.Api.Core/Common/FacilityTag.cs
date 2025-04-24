using System;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the FacilityTag.
    /// </summary>
    public class FacilityTag
    {

        private string _groupNodeId;

        /// <summary>
        /// Gets and sets the NodeID.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets and sets the Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets and sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets and sets the Enabled.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Gets and sets the TrendType.
        /// </summary>
        public int? TrendType { get; set; }

        /// <summary>
        /// Gets and sets the RawLo.
        /// </summary>
        public int? RawLo { get; set; }

        /// <summary>
        /// Gets and sets the RawHi.
        /// </summary>
        public int? RawHi { get; set; }

        /// <summary>
        /// Gets and sets the EngLo.
        /// </summary>
        public float? EngLo { get; set; }

        /// <summary>
        /// Gets and sets the EngHi.
        /// </summary>
        public float? EngHi { get; set; }

        /// <summary>
        /// Gets and sets the LimitLo.
        /// </summary>
        public float? LimitLo { get; set; }

        /// <summary>
        /// Gets and sets the LimitHi.
        /// </summary>
        public float? LimitHi { get; set; }

        /// <summary>
        /// Gets and sets the CurrentValue.
        /// </summary>
        public float? CurrentValue { get; set; }

        /// <summary>
        /// Gets and sets the EngUnits.
        /// </summary>
        public string EngUnits { get; set; }

        /// <summary>
        /// Gets and sets the UpdateDate.
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets and sets the Writeable.
        /// </summary>
        public bool? Writeable { get; set; }

        /// <summary>
        /// Gets and sets the Topic.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Gets and sets the Group Node Id.
        /// </summary>
        public string GroupNodeId
        {
            get => _groupNodeId;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _groupNodeId = NodeId;
                }
                else
                {
                    _groupNodeId = value;
                }
            }
        }

        /// <summary>
        /// Gets and sets the DisplayOrder.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets and sets the AlarmState.
        /// </summary>
        public int? AlarmState { get; set; }

        /// <summary>
        /// Gets and sets the AlarmAction.
        /// </summary>
        public int? AlarmAction { get; set; }

        /// <summary>
        /// Gets and sets the WellGroupName.
        /// </summary>
        public string WellGroupName { get; set; }

        /// <summary>
        /// Gets and sets the PagingGroup.
        /// </summary>
        public string PagingGroup { get; set; }

        /// <summary>
        /// Gets and sets the AlarmArg.
        /// </summary>
        public string AlarmArg { get; set; }

        /// <summary>
        /// Gets and sets the AlarmTextLo.
        /// </summary>
        public string AlarmTextLo { get; set; }

        /// <summary>
        /// Gets and sets the AlarmTextHi.
        /// </summary>
        public string AlarmTextHi { get; set; }

        /// <summary>
        /// Gets and sets the AlarmTextClear.
        /// </summary>
        public string AlarmTextClear { get; set; }

        /// <summary>
        /// Gets and sets the GroupStatusView.
        /// </summary>
        public int? GroupStatusView { get; set; }

        /// <summary>
        /// Gets and sets the Responder List Id.
        /// </summary>
        public int? ResponderListId { get; set; }

        /// <summary>
        /// Gets and sets the VoiceTextLo.
        /// </summary>
        public string VoiceTextLo { get; set; }

        /// <summary>
        /// Gets and sets the VoiceTextHi.
        /// </summary>
        public string VoiceTextHi { get; set; }

        /// <summary>
        /// Gets and sets the VoiceTextClear.
        /// </summary>
        public string VoiceTextClear { get; set; }

        /// <summary>
        /// Gets and sets the DataType.
        /// </summary>
        public int? DataType { get; set; }

        /// <summary>
        /// Gets and sets the Decimals.
        /// </summary>
        public int? Decimals { get; set; }

        /// <summary>
        /// Gets and sets the Voice Node Id.
        /// </summary>
        public string VoiceNodeId { get; set; }

        /// <summary>
        /// Gets and sets the Bit.
        /// </summary>
        public int? Bit { get; set; }

        /// <summary>
        /// Gets and sets the Contact List Id.
        /// </summary>
        public int? ContactListId { get; set; }

        /// <summary>
        /// Gets and sets the Deadband.
        /// </summary>
        public float? Deadband { get; set; }

        /// <summary>
        /// Gets and sets the UnitType.
        /// </summary>
        public short? UnitType { get; set; }

        /// <summary>
        /// Gets and sets the DestinationType.
        /// </summary>
        public int? DestinationType { get; set; }

        /// <summary>
        /// Gets and sets the State Id.
        /// </summary>
        public int? StateId { get; set; }

        /// <summary>
        /// Gets and sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the DisplayName.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets and sets the CaptureType.
        /// </summary>
        public int CaptureType { get; set; }

        /// <summary>
        /// Gets and sets the ParamStandardType.
        /// </summary>
        public int? ParamStandardType { get; set; }

        /// <summary>
        /// Gets and sets the Latitude.
        /// </summary>
        public Decimal? Latitude { get; set; }

        /// <summary>
        /// Gets and sets the Longitude.
        /// </summary>
        public Decimal? Longitude { get; set; }

        /// <summary>
        /// Gets and sets the ArchiveFunction.
        /// </summary>
        public int ArchiveFunction { get; set; }

        /// <summary>
        /// Gets and sets the Tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets and sets the NumConsecAlarmsAllowed.
        /// </summary>
        public int? NumConsecAlarmsAllowed { get; set; }

        /// <summary>
        /// Gets and sets the DetailViewOnly.
        /// </summary>
        public bool? DetailViewOnly { get; set; }

        /// <summary>
        /// Gets and sets the StringValue.
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// Gets and sets the Expression.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets and sets the KeyValue.
        /// </summary>
        public string KeyValue
        {
            get
            {
                int? bit;
                return NodeId + "|" + Address.ToString() + "|" + ((bit = Bit).HasValue
                    ? bit.GetValueOrDefault().ToString()
                    : null);
            }
        }

        /// <summary>
        /// Gets and sets the Facility Tag Group Id.
        /// </summary>
        public int? FacilityTagGroupId { get; set; }

        /// <summary>
        /// Map from Data.Models.FacilityTagGroupID to Core.Common.FacilityTag.
        /// </summary>
        public static FacilityTag Map(FacilityTagsModel entity)
        {
            if (entity == null)
            {
                return null;
            }

            var domain = new FacilityTag();

            domain.NodeId = entity.NodeId;
            domain.Address = entity.Address;
            domain.Bit = entity.Bit;
            domain.Description = entity.Description;
            domain.DataType = entity.DataType;
            domain.GroupNodeId = entity.GroupNodeId;
            domain.StateId = entity.StateId;
            domain.UnitType = entity.UnitType;
            domain.FacilityTagGroupId = entity.FacilityTagGroupId;
            domain.Writeable = entity.Writeable;
            domain.ParamStandardType = entity.ParamStandardType;
            domain.Tag = entity.Tag;

            domain.DisplayName = string.Empty;

            return domain;
        }

    }

}
