namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the valve class.
    /// </summary>
    public class Valve : IdentityBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the diameter of the valve
        /// </summary>
        public float? Diameter { get; set; } // Length

        /// <summary>
        /// Gets or sets the valve description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the area of the bellows
        /// </summary>
        public float BellowsArea { get; set; }

        /// <summary>
        /// Gets or sets the size of the port
        /// </summary>
        public float? PortSize { get; set; } // Length

        /// <summary>
        /// Gets or sets the area of the port
        /// </summary>
        public float PortArea { get; set; }

        /// <summary>
        /// Gets or sets the port-to-bellows-area ratio
        /// </summary>
        public float PortToBellowsAreaRatio { get; set; }

        /// <summary>
        /// Gets or sets the production pressure effect factor (PPEF)
        /// </summary>
        public float ProductionPressureEffectFactor { get; set; }

        /// <summary>
        /// Gets or sets the 1 - Ap / Ab [ aka 1 - PortToBellowsAreaRatio ] value
        /// </summary>
        public float? OneMinusR { get; set; }

        /// <summary>
        /// Gets or sets the valve manufacturer
        /// </summary>
        public Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the valve was created by the user.
        /// </summary>
        public bool IsCustom { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new Valve with a default ID
        /// </summary>
        public Valve()
        {
        }

        /// <summary>
        /// Initializes a new Valve with a specified ID
        /// </summary>
        /// <param name="id">The ID</param>
        public Valve(object id)
            : base(id)
        {
        }

        #endregion

    }
}
