using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{

    /// <summary>
    /// Defines the facility tag group data model.
    /// </summary>
    public class FacilityTagGroupModel
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

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
        public string DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the tag count.
        /// </summary>
        public string TagCount { get; set; }

        /// <summary>
        /// Gets or sets the alarm count.
        /// </summary>
        public string AlarmCount { get; set; }

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
        public List<FacilityTagsModel> FacilityTags { get; set; } = new List<FacilityTagsModel>();

    }
}
