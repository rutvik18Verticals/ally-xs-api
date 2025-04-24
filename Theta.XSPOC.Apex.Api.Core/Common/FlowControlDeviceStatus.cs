using Theta.XSPOC.Apex.Api.Core.Common.Interface;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the class for flow control device status.
    /// </summary>
    /// <typeparam name="T">The <seealso cref="IFlowControlDeviceState"/>.</typeparam>
    /// <typeparam name="U">The <seealso cref="IFlowControlDevice"/>.</typeparam>
    public abstract class FlowControlDeviceStatus<T, U> : IdentityBase
        where T : IFlowControlDeviceState where U : IFlowControlDevice
    {

        #region Properties

        /// <summary>
        /// Gets or sets the state of the device
        /// </summary>
        public abstract T State { get; set; }

        /// <summary>
        /// Gets or sets whether gas is being injected through the device
        /// </summary>
        public bool? IsInjectingGas { get; set; }

        /// <summary>
        /// Gets or sets the gas rate through the device
        /// </summary>
        public float? GasRate { get; set; } // Volume

        /// <summary>
        /// Gets or sets the injection pressure for this device
        /// </summary>
        public float? InjectionPressureAtDepth { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the tubing critical velocity at this flow control device
        /// </summary>
        public float? TubingCriticalVelocityAtDepth { get; set; } // Volume

        /// <summary>
        /// Gets or sets the injection rate needed for calculated tubing critical velocity at this flow control device
        /// </summary>
        public float? InjectionRateForTubingCriticalVelocity { get; set; } // Volume

        /// <summary>
        /// Gets or set the opening pressure at depth for this flow control device. Only applicable to valves.
        /// </summary>
        public float? OpeningPressureAtDepth { get; set; } // Pressure

        /// <summary>
        /// Gets or set the closing pressure at depth for this flow control device. Only applicable to valves.
        /// </summary>
        public float? ClosingPressureAtDepth { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the flow control device ( a valve or an orifice )
        /// </summary>
        public abstract U FlowControlDevice { get; set; }

        /// <summary>
        /// Gets or sets the flow control depth.
        /// </summary>
        public float? Depth { get; set; } // Length

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new FlowControlDeviceStatus with a default ID
        /// </summary>
        public FlowControlDeviceStatus()
        {
        }

        /// <summary>
        /// Initializes a new FlowControlDeviceStatus with a specified ID
        /// </summary>
        /// <param name="id">The ID</param>
        public FlowControlDeviceStatus(object id)
            : base(id)
        {
        }

        #endregion

    }
}
