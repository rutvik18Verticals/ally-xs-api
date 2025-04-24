using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the class for phrase placeholder for analysis.
    /// </summary>
    public class GLAnalysisPhrasePlaceholder : EnhancedEnumBase
    {

        #region Static Properties

        /// <summary>
        /// Represents Vertical Injection Depth From Valve Analysis
        /// </summary>
        public static GLAnalysisPhrasePlaceholder VerticalInjectionDepthFromValveAnalysis { get; }
            = CreateValue(1, "[VerticalInjectionDepthFromValveAnalysis]");

        /// <summary>
        /// Represents Optimum Liquid Rate
        /// </summary>
        public static GLAnalysisPhrasePlaceholder OptimumLiquidRate { get; }
            = CreateValue(2, "[OptimumLiquidRate]");

        /// <summary>
        /// Represents Injection Rate For Optimum Liquid Rate
        /// </summary>
        public static GLAnalysisPhrasePlaceholder InjectionRateForOptimumLiquidRate { get; }
            = CreateValue(3, "[InjectionRateForOptimumLiquidRate]");

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new <see cref="GLAnalysisPhrasePlaceholder"/> with a specified key and name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        public GLAnalysisPhrasePlaceholder(int key, Text name) : base(key, name)
        {
        }

        #endregion

        #region Static Methods

        private static GLAnalysisPhrasePlaceholder CreateValue(int key, string name)
        {
            var value = new GLAnalysisPhrasePlaceholder(key, new Text(name));

            Register(value);

            return value;
        }

        #endregion

    }

}
