using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{

    /// <summary>
    /// Represents a setpoint group data model.
    /// </summary>
    public class SetpointGroupModel
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
        /// Gets or sets the display order.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the list of setpoint registers.
        /// </summary>
        public List<SetpointModel> Setpoints { get; set; }

    }

    /// <summary>
    /// Represents a mock setpoint data.
    /// </summary>
    public class MockSetpointGroup
    {

        /// <summary>
        /// Gets or sets the setpoint groups.
        /// </summary>
        public List<SetpointGroupModel> SetpointGroups { get; set; }

    }
}
