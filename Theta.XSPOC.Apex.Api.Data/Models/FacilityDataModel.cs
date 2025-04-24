using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Defines the facility data model.
    /// </summary>
    public class FacilityDataModel
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the facility.
        /// </summary>
        public string Facility { get; set; }

        /// <summary>
        /// Gets or sets the comm status.
        /// </summary>
        public string CommStatus { get; set; }

        /// <summary>
        /// Gets or sets the indicator.
        /// </summary>
        public string Indicator { get; set; }

        /// <summary>
        /// Gets or sets the bool value if facility enabled/not.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the node host alarm.
        /// </summary>
        public string NodeHostAlarm { get; set; }

        /// <summary>
        /// Gets or sets the node host alarm state.
        /// </summary>
        public int NodeHostAlarmState { get; set; }

        /// <summary>
        /// Gets or sets the host alarm back color.
        /// </summary>
        public string HostAlarmBackColor { get; set; }

        /// <summary>
        /// Gets or sets the host alarm fore color.
        /// </summary>
        public string HostAlarmForeColor { get; set; }

        /// <summary>
        /// Gets or sets the tag count.
        /// </summary>
        public int TagCount { get; set; }

        /// <summary>
        /// Gets or sets the alarm count.
        /// </summary>
        public int AlarmCount { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or Sets the List of Facility tag Groups
        /// </summary>
        public List<FacilityTagGroupModel> TagGroups { get; set; } = new List<FacilityTagGroupModel>();

    }
}