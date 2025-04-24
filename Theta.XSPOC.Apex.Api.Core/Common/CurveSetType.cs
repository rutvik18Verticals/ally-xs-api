using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the available curve set names
    /// </summary>
    public class CurveSetType : EnhancedEnumBase
    {

        #region Properties

        /// <summary>
        /// Gets the tornado curve set type.
        /// </summary>
        /// <value>
        /// The tornado curve.
        /// </value>
        public static CurveSetType Tornado { get; private set; }

        /// <summary>
        /// Gets the FBHP curve set type.
        /// </summary>
        /// <value>
        /// The FBHP curve.
        /// </value>
        public static CurveSetType FBHP { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="CurveSetType"/> class.
        /// </summary>
        static CurveSetType()
        {
            Tornado = CreateValue(0, "Tornado");
            FBHP = CreateValue(1, "FBHP");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveSetType"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        protected CurveSetType(int key, Text name) : base(key, name)
        {
        }

        #endregion

        #region Private Methods

        private static CurveSetType CreateValue(int key, string name)
        {
            var value = new CurveSetType(key, new Text(name));

            Register(value);

            return value;
        }

        #endregion

    }
}
