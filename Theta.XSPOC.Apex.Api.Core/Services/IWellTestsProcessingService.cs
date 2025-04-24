using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Interface for processing Well Tests Processing Service.
    /// </summary>
    public interface IWellTestsProcessingService
    {

        /// <summary>
        /// Processes the provided Well Tests Processing Service request and generates response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="WellTestInput"/> to act on, annotated
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="WellTestDataOutput"/>.</returns>
        WellTestDataOutput GetESPAnalysisWellTestData(WithCorrelationId<WellTestInput> data);

        /// <summary>
        /// Processes the provided Well Tests GL Analysis Processing Service request and generates response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="WellTestInput"/> to act on, annotated
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GLAnalysisWellTestDataOutput"/></returns>
        GLAnalysisWellTestDataOutput GetGLAnalysisWellTestData(WithCorrelationId<WellTestInput> data);

    }
}
