using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
    public class AssetsController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.GroupAndAsset);

        #endregion

        #region Private Fields

        private readonly IGroupAndAssetService _service;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="AssetsController"/>.
        /// </summary>
        /// <param name="groupAndAssetService">The <seealso cref="IGroupAndAssetService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="groupAndAssetService"/> is null.
        /// or
        /// <paramref name="loggerFactory"/>is null.
        /// </exception>
        public AssetsController(IGroupAndAssetService groupAndAssetService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _service = groupAndAssetService ?? throw new ArgumentNullException(nameof(groupAndAssetService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for group and asset.
        /// </summary>
        /// <param name="groupBy">The groupBy to the Get Group And Asset only if it contains  the AssetGroupName.</param>
        /// <param name="isNewArchitecture">The boolean value to check if only the new architecture wells to be fetched.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet(Name = "GetGroupAndAsset")]
        public IActionResult Get([FromQuery] string[] groupBy, bool isNewArchitecture = false)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(AssetsController)} " +
                $"{nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, groupBy,
                QueryParams.GroupName))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(AssetsController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateGroupName(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedGroupName,
                    groupBy.SingleOrDefault()))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(AssetsController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateUserAuthorized(correlationId, out defaultInvalidStatusCodeResult, out var user))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(AssetsController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var assetsInput = new AssetsInput()
            {
                AssetGroup = parsedGroupName,
                IsNewArchitecture = isNewArchitecture
            };

            var requestWithCorrelationId = new WithCorrelationId<AssetsInput>
                (correlationId, assetsInput);

            if (requestWithCorrelationId.Value.AssetGroup == "AssetGroupName")
            {
                var message = $"{nameof(requestWithCorrelationId.Value.AssetGroup)} , Process Get Group And Asset Data In.";
                ControllerLogger.WriteCId(Level.Info, message, requestWithCorrelationId?.CorrelationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(AssetsController)} {nameof(Get)}", correlationId);

                return ProcessGetGroupAndAssetData(requestWithCorrelationId, user);
            }
            else
            {
                var message = $"{nameof(requestWithCorrelationId.Value.AssetGroup)} , Not Found.";
                ControllerLogger.WriteCId(Level.Info, message, requestWithCorrelationId?.CorrelationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(AssetsController)} {nameof(Get)}", correlationId);

                return BadRequest();
            }
        }

        #endregion

        #region Private Method

        private IActionResult ProcessGetGroupAndAssetData(WithCorrelationId<AssetsInput> requestWithCorrelationId, string userName)
        {
            var serviceResult = _service.GetGroupAndAssetData(userName, requestWithCorrelationId.CorrelationId,
                requestWithCorrelationId.Value.IsNewArchitecture);

            if (!ValidateServiceResult(requestWithCorrelationId.CorrelationId, out var defaultInvalidStatusCodeResult, serviceResult))
            {
                return defaultInvalidStatusCodeResult;
            }

            var response = GroupAndAssetDataMapper.Map(serviceResult, requestWithCorrelationId.CorrelationId);

            return Ok(response);
        }

        #endregion

    }
}
