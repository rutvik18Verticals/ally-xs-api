using Theta.XSPOC.Apex.Api.Core.Common.Interface;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the orifice status class.
    /// </summary>
    public class OrificeStatus : FlowControlDeviceStatus<OrificeState, Orifice>
    {

        #region Properties

        /// <summary>
        /// Gets or sets the state of the orifice
        /// </summary>
        public override OrificeState State { get; set; }

        /// <summary>
        /// Gets or sets the orifice.
        /// </summary>
        public override Orifice FlowControlDevice { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new OrificeStatus with a default ID
        /// </summary>
        public OrificeStatus() : base()
        {
        }

        /// <summary>
        /// Initializes a new OrificeStatus with a specified ID
        /// </summary>
        /// <param name="id">The ID</param>
        public OrificeStatus(object id)
            : base(id)
        {
        }

        #endregion

    }

    /// <summary>
    /// Represents the orifice state class.
    /// </summary>
    public class OrificeState : EnhancedEnumBase, IFlowControlDeviceState
    {

        #region Static Properties

        /// <summary>
        /// Gets the default value
        /// </summary>
        public static OrificeState Default { get; private set; }

        /// <summary>
        /// Specifies that the orifice is open
        /// </summary>
        public static OrificeState Open => Default;

        #endregion

        #region Constructors

        static OrificeState()
        {
            Default = CreateValue(4, "Open");
        }

        /// <summary>
        /// Initializes a new OrificeState with a specified key and name
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="name">The name</param>
        protected OrificeState(int key, Text name)
            : base(key, name)
        {
        }

        #endregion

        #region Static Methods

        private static OrificeState CreateValue(int key, string name)
        {
            var value = new OrificeState(key, new Text(name));

            Register(value);

            return value;
        }

        #endregion

    }

    /// <summary>
    /// Represents the orifice class.
    /// </summary>
    public class Orifice : IFlowControlDevice
    {

        /// <summary>
        /// Gets or sets the orifice manufacturer
        /// </summary>
        public Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the measured depth. Optional.
        /// </summary>
        public float? MeasuredDepth { get; set; } // Length

        /// <summary>
        /// Gets or sets the vertical depth. Required.
        /// </summary>
        public float? VerticalDepth { get; set; } // Length

        /// <summary>
        /// Gets or sets the Port Size. Required
        /// </summary>
        public float? PortSize { get; set; } // Length

        /// <summary>
        /// Gets or sets the true vertical depth from the surface.
        /// </summary>
        public float? TrueVerticalDepth { get; set; } // Length

        /// <summary>
        /// Gets the orifice depth based on if true vertical depth is true.
        /// </summary>
        /// <param name="useTVD">Represents if using true vertical depth is enabled.</param>
        /// <returns>If useTVD is true, returns current true vertical depth. 
        /// If useTVD is false, returns vertical depth unless it is null, then returns measured depth.</returns>
        public float? GetDepth(bool useTVD) // Length
        {
            if (useTVD)
            {
                return TrueVerticalDepth;
            }

            return VerticalDepth ?? MeasuredDepth;
        }

    }
}
