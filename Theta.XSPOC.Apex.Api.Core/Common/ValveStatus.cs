namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the valvestatus class.
    /// </summary>
    public class ValveStatus : FlowControlDeviceStatus<ValveState, WellValve>
    {

        #region Properties

        /// <summary>
        /// Gets or sets the state of the valve
        /// </summary>
        public override ValveState State { get; set; }

        /// <summary>
        /// Gets or sets the valve.
        /// </summary>
        public override WellValve FlowControlDevice { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new ValveStatus with a default ID
        /// </summary>
        public ValveStatus() : base()
        {
        }

        /// <summary>
        /// Initializes a new ValveStatus with a specified ID
        /// </summary>
        /// <param name="id">The ID</param>
        public ValveStatus(object id)
            : base(id)
        {
        }

        #endregion

    }
}
