using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the availble values of esp analysis phrase placeholders.
    /// </summary>
    public class ESPAnalysisPhrasePlaceholder : EnhancedEnumBase
    {

        #region Static Properties

        /// <summary>
        /// Represents Calculated Fluid Level Above Pump
        /// </summary>
        public static ESPAnalysisPhrasePlaceholder CalculatedFluidLevelAbovePump { get; }
            = CreateValue(1, "[CalculatedFluidLevelAbovePump]");

        /// <summary>
        /// Represents Oil Rate
        /// </summary>
        public static ESPAnalysisPhrasePlaceholder OilRate { get; }
            = CreateValue(2, "[OilRate]");

        /// <summary>
        /// Represents Max Potential Production Rate
        /// </summary>
        public static ESPAnalysisPhrasePlaceholder MaxPotentialProductionRate { get; }
            = CreateValue(3, "[MaxPotentialProductionRate]");

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new <see cref="ESPAnalysisPhrasePlaceholder"/> with a specified key and name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        public ESPAnalysisPhrasePlaceholder(int key, Text name) : base(key, name)
        {
        }

        #endregion

        #region Static Methods

        private static ESPAnalysisPhrasePlaceholder CreateValue(int key, string name)
        {
            var value = new ESPAnalysisPhrasePlaceholder(key, new Text(name));

            Register(value);

            return value;
        }

        #endregion

    }
}
