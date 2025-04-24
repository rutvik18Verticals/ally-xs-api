using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.WellControl.Contracts.Responses
{
    /// <summary>
    /// The set point group response data contract.
    /// </summary>
    public class SetpointsGroupsData
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