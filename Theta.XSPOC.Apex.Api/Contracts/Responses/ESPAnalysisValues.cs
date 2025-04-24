using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Represent the class for ESPAnalysis data values.
    /// </summary>
    public class ESPAnalysisValues
    {

        /// <summary>
        /// Gets or sets the list of input ValueItem.
        /// </summary>
        public IList<ValueItem> Input { get; set; }

        /// <summary>
        /// Gets or sets the list of output ValueItem.
        /// </summary>
        public IList<ValueItem> Output { get; set; }

        /// <summary>
        /// Gets or sets the list of gas handling input ValueItem.
        /// </summary>
        public IList<ValueItem> GasHandlingInputs { get; set; }

        /// <summary>
        /// Gets or sets the list of gas handling output ValueItem.
        /// </summary>
        public IList<ValueItem> GasHandlingOutputs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating  if the gas handling is enabled.
        /// </summary>
        public bool IsGasHandlingEnabled { get; set; }

    }
}
