using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the valve injection depth estimate result.
    /// </summary>
    public class ValveInjectionDepthEstimateResult
    {

        /// <summary>
        /// Gets or sets the initial valve injection depth estimate.
        /// </summary>
        public double? InitialValveInjectionDepthEstimate { get; set; }

        /// <summary>
        /// Gets or sets the injection depth estimate.
        /// </summary>
        public double? InjectionDepthEstimate { get; set; }

        /// <summary>
        /// Gets or sets the valve number from estimate.
        /// </summary>
        public int ValveNumberFromEstimate { get; set; }

        /// <summary>
        /// Gets or sets the vertical injection depth from valve analysis.
        /// </summary>
        public double? VerticalInjectionDepthFromValveAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the measured injection depth from valve analysis.
        /// </summary>
        public double? MeasuredInjectionDepthFromValveAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the injecting valve number from valve analysis.
        /// </summary>
        public int InjectingValveNumberFromValveAnalysis { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we estimated we're injecting through the orifice.
        /// </summary>
        public bool EstimatedInjectingThroughOrifice { get; set; }

        /// <summary>
        /// Gets or sets the injecting valve status.
        /// </summary>
        public GLValveStatusModel InjectingValveStatus { get; set; }

    }
}
