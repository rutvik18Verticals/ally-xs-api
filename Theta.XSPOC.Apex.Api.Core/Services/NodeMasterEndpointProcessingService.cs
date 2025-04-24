using System;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Implementation of IDataHistoryProcessingService interface.
    /// </summary>
    public class NodeMasterEndpointProcessingService : INodeMasterEndpointProcessingService
    {

        #region Private Members

        private readonly INodeMaster _nodeMaster;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="DataHistoryProcessingService"/>.
        /// </summary>
        /// <param name="nodeMaster">
        /// The <seealso cref="INodeMaster"/> service.</param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="nodeMaster"/> is null OR
        /// <paramref name="loggerFactory"/> is null OR
        /// </exception>
        public NodeMasterEndpointProcessingService(INodeMaster nodeMaster, IThetaLoggerFactory loggerFactory)
        {
            _nodeMaster = nodeMaster ?? throw new ArgumentNullException(nameof(nodeMaster));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IDataHistoryProcessingService Implementation

        /// <summary>
        /// Processes the Node Master Data Items.
        /// </summary>
        /// <param name="inputData">The <seealso cref="WithCorrelationId{NodeMasterColumnsInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The dictionary of columns.</returns>
        /// <returns>The output response domain object <seealso cref="NodeMasterSelectedColumnsOutput"/>.</returns>
        public NodeMasterSelectedColumnsOutput GetNodeMasterColumnData(WithCorrelationId<NodeMasterColumnsInput> inputData)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMasterEndpointProcessingService)} " +
                $"{nameof(GetNodeMasterColumnData)}", inputData?.CorrelationId);

            NodeMasterSelectedColumnsOutput nodeMasterData = new NodeMasterSelectedColumnsOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (inputData == null)
            {
                var message = $"Correlation is null, cannot get node master data items.";
                logger.Write(Level.Info, message);
                nodeMasterData.Result.Status = false;
                nodeMasterData.Result.Value = message;

                return nodeMasterData;
            }

            if (inputData?.Value == null)
            {
                var message = $"{nameof(inputData)} is null, cannot get node master data items.";
                logger.WriteCId(Level.Info, message, inputData?.CorrelationId);
                nodeMasterData.Result.Status = false;
                nodeMasterData.Result.Value = message;

                return nodeMasterData;
            }

            var correlationId = inputData?.CorrelationId;
            var request = inputData.Value;
            if (request.AssetId == Guid.Empty)
            {
                var message = $"{nameof(request.AssetId)}," +
                    $" should be provided to get node master data items.";
                logger.WriteCId(Level.Info, message, correlationId);
                return nodeMasterData;
            }

            var response = _nodeMaster.GetNodeMasterData(request.AssetId,
                request.Columns, correlationId);

            nodeMasterData = MapToDomainObject(response);
            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMasterEndpointProcessingService)} " +
                $"{nameof(GetNodeMasterColumnData)}", inputData?.CorrelationId);

            return nodeMasterData;
        }

        private NodeMasterSelectedColumnsOutput MapToDomainObject(NodeMasterDictionary nodeMasterDictionary)
        {
            NodeMasterSelectedColumnsOutput nodeMasterData = new NodeMasterSelectedColumnsOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            foreach (var item in nodeMasterDictionary.Data)
            {
                nodeMasterData.Data.TryAdd(item.Key, item.Value);
            }

            return nodeMasterData;
        }

        #endregion

    }
}