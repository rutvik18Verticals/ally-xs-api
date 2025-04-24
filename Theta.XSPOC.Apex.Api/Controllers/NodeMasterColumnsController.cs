using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Controllers
{
    /// <summary>
    /// The controller that fields requests for Group and Asset
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class NodeMasterColumnsController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.APIService);

        #endregion

        #region Private Fields

        private readonly INodeMasterEndpointProcessingService _service;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="NodeMasterColumnsController"/>.
        /// </summary>
        /// <param name="nodeMasterEndpointProcessingService">The <seealso cref="INodeMasterEndpointProcessingService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null.
        /// or
        /// <paramref name="nodeMasterEndpointProcessingService"/> is null.
        /// </exception>
        public NodeMasterColumnsController(INodeMasterEndpointProcessingService nodeMasterEndpointProcessingService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _service = nodeMasterEndpointProcessingService ?? throw new ArgumentNullException(nameof(nodeMasterEndpointProcessingService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for fetching data history trends data.
        /// </summary>
        /// <param name="columns">The request ViewId.</param>
        /// <param name="assetId">The assetid.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("GetNodeMasterData", Name = "GetNodeMasterData")]
        public IActionResult GetNodeMasterData(string assetId, [FromQuery] string[] columns)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(NodeMasterColumnsController)} {nameof(GetNodeMasterData)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, columns,
                    assetId, QueryParams.CardDate))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(NodeMasterColumnsController)} {nameof(GetNodeMasterData)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    assetId))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(NodeMasterColumnsController)} {nameof(GetNodeMasterData)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var input = new NodeMasterColumnsInput()
            {
                AssetId = parsedAssetId,
                Columns = columns
            };

            var requestWithCorrelationId = new WithCorrelationId<NodeMasterColumnsInput>(
                correlationId, input);

            var serviceResult = _service.GetNodeMasterColumnData(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(NodeMasterColumnsController)} {nameof(GetNodeMasterData)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = NodeMasterColumnMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(NodeMasterColumnsController)} {nameof(GetNodeMasterData)}", correlationId);

            return Ok(response);
        }

        #endregion

    }
}
