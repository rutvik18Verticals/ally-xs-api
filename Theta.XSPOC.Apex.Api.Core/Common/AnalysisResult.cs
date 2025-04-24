namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// The result common to all lift analysis.
    /// </summary>
    public class AnalysisResult : ArtificalLiftAnalysisResultBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the inputs to the analysis.
        /// </summary>
        public ESPAnalysisInput Inputs { get; set; }

        /// <summary>
        /// Gets or sets the outputs of the analysis.
        /// </summary>
        public AnalysisOutput Outputs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gas-handling is enabled.
        /// </summary>
        public bool IsGasHandlingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the sources associated with the inputs.
        /// </summary>
        public AnalysisSource Sources { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new AnalysisResult with a default identifier.
        /// </summary>
        public AnalysisResult()
        {
        }

        /// <summary>
        /// Initializes a new AnalysisResult with a specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public AnalysisResult(object id)
            : base(id)
        {
        }

        #endregion

    }
}
