using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Mappers;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Implementation of INotificationProcessingService.
    /// </summary>
    public class NotificationProcessingService : INotificationProcessingService
    {

        #region Private Dependencies

        private readonly INotification _eventsService;
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IDateTimeConverter _dateTimeConverter;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="NotificationProcessingService"/>.
        /// </summary>
        /// <param name="eventsService">The <seealso cref="INotification"/> service.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <param name="dateTimeConverter">The <seealso cref="IDateTimeConverter"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventsService"/> is null
        /// or
        /// <paramref name="loggerFactory"/> is null
        /// </exception>
        public NotificationProcessingService(INotification eventsService, IThetaLoggerFactory loggerFactory,
           IDateTimeConverter dateTimeConverter)
        {
            _eventsService = eventsService ?? throw new ArgumentNullException(nameof(eventsService));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
        }

        #endregion

        #region INotificationProcessingService Implementation

        /// <summary>
        /// Processes the provided notification request and generates notifications based on that data.
        /// </summary>
        /// <param name="requestWithCorrelationId">The <seealso cref="NotificationsInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="NotificationDataOutput"/>.</returns>
        public NotificationDataOutput GetNotifications(WithCorrelationId<NotificationsInput> requestWithCorrelationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.Notification);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NotificationProcessingService)} {nameof(GetNotifications)}", requestWithCorrelationId?.CorrelationId);

            NotificationDataOutput notificationData = new NotificationDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (requestWithCorrelationId == null)
            {
                var message = $"Correlation Id is null, cannot get notifications.";
                logger.Write(Level.Info, message);
                notificationData.Result.Status = false;
                notificationData.Result.Value = message;

                return notificationData;
            }

            if (requestWithCorrelationId?.Value == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get notifications.";
                logger.WriteCId(Level.Info, message, requestWithCorrelationId?.CorrelationId);
                notificationData.Result = new MethodResult<string>(false, message);
                return notificationData;
            }

            var correlationId = requestWithCorrelationId?.CorrelationId;

            var notificationRequest = requestWithCorrelationId.Value;

            if (string.IsNullOrEmpty(notificationRequest.AssetId) &&
                string.IsNullOrEmpty(notificationRequest.AssetGroupName))
            {
                var message = $"Either {nameof(notificationRequest.AssetId)} or {nameof(notificationRequest.AssetGroupName)}" +
                    $" should be provided to get notifications.";
                logger.WriteCId(Level.Info, message, correlationId);
                notificationData.Result = new MethodResult<string>(false, message);

                return notificationData;
            }

            if (!string.IsNullOrEmpty(notificationRequest.AssetId))
            {
                var resultData = _eventsService.GetEventsByAssetId(notificationRequest.AssetId,
                    notificationRequest.NotificationTypeId, requestWithCorrelationId.CorrelationId);
                notificationData.Values = new List<NotificationData>();

                foreach (var data in resultData)
                {
                    data.Date = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(data.Date, correlationId, LoggingModel.Notification);
                    data.Date = _dateTimeConverter
                                              .GetTimeZoneAdjustedTime(data.TzOffset, data.TzDaylight, data.Date,
                                                                                     correlationId,
                                                                                     LoggingModel.Notification);

                    notificationData.Values.Add(NotificationDataMapper.MapToDomainObject(data));
                }
            }
            else if (!string.IsNullOrEmpty(notificationRequest.AssetGroupName))
            {
                var resultData = _eventsService.GetEventsByAssetGroupName(notificationRequest.AssetGroupName,
                    notificationRequest.NotificationTypeId, requestWithCorrelationId.CorrelationId);
                notificationData.Values = new List<NotificationData>();

                foreach (var data in resultData)
                {
                    data.Date = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(data.Date, correlationId, LoggingModel.Notification);
                    data.Date = _dateTimeConverter
                                              .GetTimeZoneAdjustedTime(data.TzOffset, data.TzDaylight, data.Date,
                                                                                     correlationId,
                                                                                     LoggingModel.Notification);
                    notificationData.Values.Add(NotificationDataMapper.MapToDomainObject(data));
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(NotificationProcessingService)} {nameof(GetNotifications)}", requestWithCorrelationId?.CorrelationId);

            return notificationData;
        }

        /// <summary>
        /// Processes the provided notification types request and generates notifications based on that data.
        /// </summary>
        /// <param name="requestWithCorrelationId">The <seealso cref="NotificationsInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="NotificationTypesDataOutput"/>.</returns>
        public NotificationTypesDataOutput GetNotificationsTypes(WithCorrelationId<NotificationsInput> requestWithCorrelationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.Notification);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NotificationProcessingService)} {nameof(GetNotificationsTypes)}", requestWithCorrelationId?.CorrelationId);

            NotificationTypesDataOutput notificationTypesData = new NotificationTypesDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (requestWithCorrelationId == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get notifications Types.";
                logger.Write(Level.Info, message);
                notificationTypesData.Result.Status = false;
                notificationTypesData.Result.Value = message;

                return notificationTypesData;
            }

            if (requestWithCorrelationId?.Value == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get notifications Types.";
                logger.WriteCId(Level.Info, message, requestWithCorrelationId?.CorrelationId);
                notificationTypesData.Result = new MethodResult<string>(false, message);

                return notificationTypesData;
            }

            var correlationId = requestWithCorrelationId?.CorrelationId;

            var notificationRequest = requestWithCorrelationId.Value;

            if (string.IsNullOrEmpty(notificationRequest.AssetId) &&
                string.IsNullOrEmpty(notificationRequest.AssetGroupName))
            {
                var message = $"Either {nameof(notificationRequest.AssetId)} or {nameof(notificationRequest.AssetGroupName)}" +
                    $" should be provided to get notifications.";
                logger.WriteCId(Level.Info, message, correlationId);
                notificationTypesData.Result = new MethodResult<string>(false, message);

                return notificationTypesData;
            }

            if (!string.IsNullOrEmpty(notificationRequest.AssetId) &&
                string.IsNullOrEmpty(notificationRequest.AssetGroupName))
            {
                var resultData = _eventsService.GetEventsGroupsByAssetId(notificationRequest.AssetId, requestWithCorrelationId.CorrelationId);
                notificationTypesData.Values = new List<NotificationTypesData>();

                foreach (var data in resultData)
                {
                    notificationTypesData.Values.Add(NotificationTypesDataMapper.MapToDomainObject(data));
                }
            }
            else if (string.IsNullOrEmpty(notificationRequest.AssetId) &&
                     !string.IsNullOrEmpty(notificationRequest.AssetGroupName))
            {
                var resultData = _eventsService.GetEventsGroupsByAssetGroupName(notificationRequest.AssetGroupName, requestWithCorrelationId.CorrelationId);
                notificationTypesData.Values = new List<NotificationTypesData>();

                foreach (var data in resultData)
                {
                    notificationTypesData.Values.Add(NotificationTypesDataMapper.MapToDomainObject(data));
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(NotificationProcessingService)} {nameof(GetNotificationsTypes)}", requestWithCorrelationId?.CorrelationId);

            return notificationTypesData;
        }

        #endregion

    }
}
