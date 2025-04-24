using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the analysis result sources.
    /// </summary>
    public class AnalysisResultSource : EnhancedEnumBase
    {

        #region Properties

        /// <summary>
        /// Gets the gas lift analysis result source.
        /// </summary>
        /// <value>
        /// The gas lift.
        /// </value>
        public static AnalysisResultSource GasLift { get; private set; }

        /// <summary>
        /// Gets the ESP analysis result source
        /// </summary>
        /// <value>
        /// The ESP.
        /// </value>
        public static AnalysisResultSource ESP { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="AnalysisResultSource"/> class.
        /// </summary>
        static AnalysisResultSource()
        {
            GasLift = CreateValue(0, "Gas Lift");
            ESP = CreateValue(1, "ESP");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisResultSource"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        protected AnalysisResultSource(int key, Text name) : base(key, name)
        {
        }

        #endregion

        #region Private Methods

        private static AnalysisResultSource CreateValue(int key, string name)
        {
            var value = new AnalysisResultSource(key, new Text(name));

            Register(value);

            return value;
        }

        #endregion

    }
}
