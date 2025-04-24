using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents analysis types.
    /// </summary>
    public class AnalysisType : EnhancedEnumBase
    {

        #region Static Properties

        /// <summary>
        /// Specifies that the analysis type is for well tests.
        /// </summary>
        public static AnalysisType WellTest { get; } = Create<AnalysisType>(1, Text.FromString("WellTest"));

        /// <summary>
        /// Specifies that the analysis type is sensitivity.
        /// </summary>
        public static AnalysisType Sensitivity { get; } = Create<AnalysisType>(2, Text.FromString("Sensitivity"));

        /// <summary>
        /// Specifies that the analysis type is XDiag Result.
        /// </summary>
        public static AnalysisType XDiagResult { get; } = Create<AnalysisType>(3, Text.FromString("XDiagResult"));

        #endregion

        /// <summary>
        /// Initializes a new AnalysisType with a specified key and name
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="name">The name</param>
        public AnalysisType(int key, Text name)
            : base(key, name)
        {
        }

    }
}
