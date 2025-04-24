using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Service which handles processing of NodeMasterEndpoint.
    /// </summary>
    public interface INodeMasterEndpointProcessingService
    {

        /// <summary>
        /// Processes the Node Master Data Items.
        /// </summary>
        /// <param name="inputData">The <seealso cref="WithCorrelationId{NodeMasterColumnsInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The output response domain object <seealso cref="NodeMasterSelectedColumnsOutput"/>.</returns>
        NodeMasterSelectedColumnsOutput GetNodeMasterColumnData(WithCorrelationId<NodeMasterColumnsInput> inputData);

    }
}
