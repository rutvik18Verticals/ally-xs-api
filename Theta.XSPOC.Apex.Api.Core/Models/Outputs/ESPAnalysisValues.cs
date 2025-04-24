using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for esp analysis data values.
    /// </summary>
    public class ESPAnalysisValues
    {

        /// <summary>
        /// Gets or sets the list of input ValueItem.
        /// </summary>
        public IList<AnalysisInputOutput> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the list of output ValueItem.
        /// </summary>
        public IList<AnalysisInputOutput> Outputs { get; set; }

        /// <summary>
        /// Gets or sets the list of gas handling input ValueItem.
        /// </summary>
        public IList<AnalysisInputOutput> GasHandlingInputs { get; set; }

        /// <summary>
        /// Gets or sets the list of gas handling output ValueItem.
        /// </summary>
        public IList<AnalysisInputOutput> GasHandlingOutputs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gas-handling is enabled.
        /// </summary>
        public bool IsGasHandlingEnabled { get; set; }

    }
}
