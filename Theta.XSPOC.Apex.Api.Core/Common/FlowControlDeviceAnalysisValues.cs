namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Class for flow control device analysis values.
    /// </summary>
    public class FlowControlDeviceAnalysisValues
    {

        /// <summary>
        /// Gets or sets the position of the flow control device in top-to-bottom order
        /// </summary>
        public int? Position { get; set; }

        /// <summary>
        /// Gets or sets the flow control device description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the depth of the flow control device in the wellbore
        /// </summary>
        public string Depth { get; set; } // Length

        /// <summary>
        /// Gets or sets the closing pressure at depth
        /// </summary>
        public string ClosingPressureAtDepth { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the opening pressure at depth
        /// </summary>
        public string OpeningPressureAtDepth { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the injection pressure at depth
        /// </summary>
        public string InjectionPressureAtDepth { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the the string describing the tubing critical velocity at the depth of the valve
        /// </summary>
        public string TubingCriticalVelocityAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the status message of the flow control device 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the open state of the flow control device.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Gets or sets the injecting state of the flow control device.
        /// </summary>
        public bool IsInjecting { get; set; }

    }
}
