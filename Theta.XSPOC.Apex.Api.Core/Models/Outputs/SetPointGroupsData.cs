using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    ///  Represent the class for setpoint group data.
    /// </summary>
    public class SetPointGroupsData
    {

        /// <summary>
        /// Gets or sets the setpoint group id.
        /// </summary>
        public int SetpointGroup { get; set; }

        /// <summary>
        /// Gets or sets the setpoint group name.
        /// </summary>
        public string SetpointGroupName { get; set; }

        /// <summary>
        /// Gets or sets the count of registers.
        /// </summary>
        public int RegisterCount { get; set; }

        /// <summary>
        /// Gets or sets the list of setpoint registers.
        /// </summary>
        public List<SetpointData> Setpoints { get; set; }

    }
}