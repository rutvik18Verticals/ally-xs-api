namespace Theta.XSPOC.Apex.Api.Core.Models
{

    /// <summary>
    /// The controller mode enum.
    /// </summary>
    public enum ControllerMode
    {

        /// <summary>
        /// Manual control mode.
        /// </summary>
        Manual = 0,

        /// <summary>
        /// Step open control mode.
        /// </summary>
        StepOpen = 1,

        /// <summary>
        /// Step closed control mode.
        /// </summary>
        StepClosed = 2,

        /// <summary>
        /// Ramp open control mode.
        /// </summary>
        RampOpen = 3,

        /// <summary>
        /// Ramp closed control mode.
        /// </summary>
        RampClosed = 4,

        /// <summary>
        /// Auto DP control mode.
        /// </summary>
        AutoDP = 5,

        /// <summary>
        /// Auto flow rate control mode.
        /// </summary>
        AutoFlowRate = 6,

        /// <summary>
        /// Auto SP control mode.
        /// </summary>
        AutoSP = 7,

        /// <summary>
        /// Auto nominations control mode.
        /// </summary>
        AutoNominations = 8,

        /// <summary>
        /// Auto DP with shut in control mode.
        /// </summary>
        AutoDPWithShutIn = 9,

        /// <summary>
        /// Auto flow rate with shut in control mode.
        /// </summary>
        AutoFlowRateWithShutIn = 10,

        /// <summary>
        /// Auto SP with shut in control mode.
        /// </summary>
        AutoSPWithShutIn = 11,

        /// <summary>
        /// Auto nominations with shut in control mode.
        /// </summary>
        AutoNominationsWithShutIn = 12,

        /// <summary>
        /// Invalid control mode.
        /// </summary>
        InvalidControlMode = 255

    }
}
