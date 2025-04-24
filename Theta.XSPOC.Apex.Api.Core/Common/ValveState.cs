using Theta.XSPOC.Apex.Api.Core.Common.Interface;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a valve state.
    /// </summary>
    public class ValveState : EnhancedEnumBase, IFlowControlDeviceState
    {

        #region Static Properties

        /// <summary>
        /// Gets the default value
        /// </summary>
        public static ValveState Default { get; private set; }

        /// <summary>
        /// Specifies that the valve is open
        /// </summary>
        public static ValveState Open { get; private set; }

        /// <summary>
        /// Specifies that the valve is closed
        /// </summary>
        public static ValveState Closed => Default;

        /// <summary>
        /// Specifies that the valve is unstable
        /// </summary>
        public static ValveState Unstable { get; private set; }

        /// <summary>
        /// Specifies that the valve status is unknown
        /// </summary>
        public static ValveState Unknown { get; private set; }

        #endregion

        #region Constructors

        static ValveState()
        {
            Open = CreateValue(1, "Open");
            Default = CreateValue(2, "Closed");
            Unstable = CreateValue(3, "Unstable");
            Unknown = CreateValue(5, "Unknown");
        }

        /// <summary>
        /// Initializes a new ValveState with a specified key and name
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="name">The name</param>
        protected ValveState(int key, Text name)
            : base(key, name)
        {
        }

        #endregion

        #region Static Methods

        private static ValveState CreateValue(int key, string name)
        {
            var value = new ValveState(key, new Text(name));

            Register(value);

            return value;
        }

        #endregion

    }
}
