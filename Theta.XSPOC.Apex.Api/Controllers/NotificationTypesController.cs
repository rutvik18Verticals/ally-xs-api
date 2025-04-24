using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    /// The controller that fields requests for NotificationTypes.
    /// </summary>
    [ApiController]
    [Route("Notifications/[controller]")]
    [Authorize]
    public class NotificationTypesController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.Notification);

        #endregion

        #region Private Fields

        private readonly INotificationProcessingService _service;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="NotificationTypesController"/>.
        /// </summary>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <param name="notificationProcessingService">The <seealso cref="INotificationProcessingService"/> to call 
        /// for handling requests.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null.
        /// or
        /// <paramref name="notificationProcessingService"/> is null.
        /// </exception>
        public NotificationTypesController(IThetaLoggerFactory loggerFactory,
            INotificationProcessingService notificationProcessingService) : base(loggerFactory)
        {
            _service = notificationProcessingService ??
                throw new ArgumentNullException(nameof(notificationProcessingService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for notification types.
        /// </summary>
        /// <param name="filters">The filters contains the id and type in the dictionary.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetNotificationTypes")]
        public IActionResult Get([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(NotificationTypesController)} {nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.Id, QueryParams.Type))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(NotificationTypesController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetType(correlationId, out defaultInvalidStatusCodeResult,
                    out var type,
                    filters?[QueryParams.Type]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(NotificationTypesController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var id = filters[QueryParams.Id];

            var notificationInput = new NotificationsInput()
            {
                AssetId = (type == "asset") ? id : string.Empty,
                AssetGroupName = (type == "group") ? id : string.Empty,
            };
            var requestWithCorrelationId = new WithCorrelationId<NotificationsInput>(
                correlationId, notificationInput);

            var serviceResult = _service.GetNotificationsTypes(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(NotificationTypesController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = NotificationTypesDataMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(NotificationTypesController)} {nameof(Get)}", correlationId);

            return Ok(response);
        }

        #endregion

    }
}
