using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Represent the class for facility tag groups data..
    /// </summary>
    public class FacilityTagGroupDataOutput
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the tag group nodeid.
        /// </summary>
        public string TagGroupNodeId { get; set; }

        /// <summary>
        /// Gets or sets the tag group name.
        /// </summary>
        public string TagGroupName { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the tag count.
        /// </summary>
        public int TagCount { get; set; }

        /// <summary>
        /// Gets or sets the alarm count.
        /// </summary>
        public int AlarmCount { get; set; }

        /// <summary>
        /// Gets or sets the group host alarm.
        /// </summary>
        public string GroupHostAlarm { get; set; }

        /// <summary>
        /// Gets or sets the host alarm back color.
        /// </summary>
        public string HostAlarmBackColor { get; set; }

        /// <summary>
        /// Gets or sets the host alarm fore color.
        /// </summary>
        public string HostAlarmForeColor { get; set; }

        /// <summary>
        /// Facility Tag in Group
        /// </summary>
        public List<FacilityTagsDataOutput> FacilityTags { get; set; } = new List<FacilityTagsDataOutput>();

    }
}
