using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the valve configuration options enumerator.
    /// </summary>
    public class ValveConfigurationOption : EnhancedEnumBase
    {

        #region Properties

        /// <summary>
        /// Gets the basic input valve configuration.
        /// </summary>
        /// <value>
        /// The basic input.
        /// </value>
        public static ValveConfigurationOption BasicInput { get; private set; }

        /// <summary>
        /// Gets the use design data valve configuration.
        /// </summary>
        /// <value>
        /// The use design data.
        /// </value>
        public static ValveConfigurationOption UseDesignData { get; private set; }

        /// <summary>
        /// Gets the quick input valve configuration.
        /// </summary>
        /// <value>
        /// The quick input.
        /// </value>
        public static ValveConfigurationOption QuickInput { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="ValveConfigurationOption"/> class.
        /// </summary>
        static ValveConfigurationOption()
        {
            BasicInput = CreateValue(0, "Basic Input");
            UseDesignData = CreateValue(1, "Use Design Data");
            QuickInput = CreateValue(2, "Quick Input");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValveConfigurationOption"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        protected ValveConfigurationOption(int key, Text name) : base(key, name)
        {
        }

        #endregion

        #region Private Methods

        private static ValveConfigurationOption CreateValue(int key, string name)
        {
            var value = new ValveConfigurationOption(key, new Text(name));

            Register(value);

            return value;
        }

        #endregion

    }
}
