namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the result of an analyzing a GL well test.
    /// </summary>
    public class GLAnalysisResult : ArtificalLiftAnalysisResultBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the inputs to the analysis.
        /// </summary>
        public GLAnalysisInput Inputs { get; set; }

        /// <summary>
        /// Gets or sets the outputs of the analysis.
        /// </summary>
        public GLAnalysisOutput Outputs { get; set; }

        /// <summary>
        /// Gets or sets the statuses of the flow control devices ( valves, orifice, etc )
        /// </summary>
        public FlowControlDeviceStatuses FlowControlDeviceStatuses { get; set; }

        /// <summary>
        /// Gets or sets the sources associated with the inputs.
        /// </summary>
        public AnalysisSource Sources { get; set; }

        /// <summary>
        /// Gets or sets whether the server has attempted to process this analysis.
        /// </summary>
        public bool IsProcessed { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new AnalysisResult with a default identifier.
        /// </summary>
        public GLAnalysisResult()
        {
        }

        /// <summary>
        /// Initializes a new AnalysisResult with a specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public GLAnalysisResult(object id)
            : base(id)
        {
        }

        #endregion

    }
}
