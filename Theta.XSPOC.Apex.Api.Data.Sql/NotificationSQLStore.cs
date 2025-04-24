using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// This is the implementation that represents the configuration of a notification
    /// on the current XSPOC database.
    /// </summary>
    public class NotificationSQLStore : SQLStoreBase, INotification
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="NotificationSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public NotificationSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        #endregion

        #region INotification Implementation

        /// <summary>
        /// Get the notifications by asset id and notification type id.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="notificationTypeId">The notification type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{NotificationsModel}" />.</returns>
        public IList<NotificationsModel> GetEventsByAssetId(string assetId, int? notificationTypeId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NotificationSQLStore)} {nameof(GetEventsByAssetId)}", correlationId);

            var notifications = new List<NotificationsModel>();
            DateTime current = DateTime.UtcNow;
            DateTime range = DateTime.UtcNow.AddHours(-24);

            using (var context = _contextFactory.GetContext())
            {
                notifications = context.Events.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), l => l.NodeId, r => r.NodeId,
                        (events, nodemaster) => new
                        {
                            events,
                            nodemaster
                        })
                    .Join(context.EventGroups.AsNoTracking(), l => l.events.EventTypeId, r => r.EventTypeId,
                        (events, eventsgroup) => new
                        {
                            events,
                            eventsgroup
                        })
                    .Where(x => x.events.nodemaster.AssetGuid.ToString().ToLower() == assetId.ToLower()
                    && (x.events.events.Date < current && x.events.events.Date > range))
                    .Select(n => new NotificationsModel()
                    {
                        EventId = n.events.events.EventId.ToString(),
                        NodeId = n.events.events.NodeId,
                        Date = n.events.events.Date,
                        EventTypeId = n.events.events.EventTypeId,
                        EventTypeName = n.eventsgroup.Name,
                        Note = n.events.events.Note,
                        Status = n.events.events.Status,
                        TransactionId = n.events.events.TransactionId ?? 0,
                        UserId = n.events.events.UserId,
                        TzOffset = n.events.nodemaster.Tzoffset,
                        TzDaylight = n.events.nodemaster.Tzdaylight
                    }).OrderByDescending(x => x.Date).ToList();

                if (notificationTypeId.HasValue)
                {
                    notifications = notifications.Where(n => n.EventTypeId == notificationTypeId)
                        .OrderByDescending(x => x.Date).ToList();
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NotificationSQLStore)} {nameof(GetEventsByAssetId)}", correlationId);

            return notifications;
        }

        /// <summary>
        /// Get the notifications by asset group name and notification type id.
        /// </summary>
        /// <param name="assetGroupName">The asset group name.</param>
        /// <param name="notificationTypeId">The notification type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{NotificationsModel}"/>.</returns>
        public IList<NotificationsModel> GetEventsByAssetGroupName(string assetGroupName, int? notificationTypeId,
            string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NotificationSQLStore)} {nameof(GetEventsByAssetGroupName)}", correlationId);

            var notifications = new List<NotificationsModel>();
            DateTime current = DateTime.UtcNow;
            DateTime range = DateTime.UtcNow.AddHours(-24);

            using (var context = _contextFactory.GetContext())
            {
                notifications = context.Events.AsNoTracking()
                    .Join(context.EventGroups.AsNoTracking(), l => l.EventTypeId, r => r.EventTypeId,
                        (events, eventsgroup) => new
                        {
                            events,
                            eventsgroup
                        })
                   .Join(context.NodeMasters.AsNoTracking(), l => l.events.NodeId, r => r.NodeId,
                        (events, nodeMaster) => new
                        {
                            events,
                            nodeMaster
                        })
                   .Where(x => context.GroupMembershipCache.AsNoTracking().Any(g => x.events.events.NodeId == g.NodeId
                        && g.GroupName == assetGroupName) && (x.events.events.Date < current && x.events.events.Date > range))
                    .Select(n => new NotificationsModel()
                    {
                        EventId = n.events.events.EventId.ToString(),
                        NodeId = n.events.events.NodeId,
                        Date = n.events.events.Date,
                        EventTypeId = n.events.events.EventTypeId,
                        EventTypeName = n.events.eventsgroup.Name,
                        Note = n.events.events.Note,
                        Status = n.events.events.Status,
                        TransactionId = n.events.events.TransactionId ?? 0,
                        UserId = n.events.events.UserId,
                        TzOffset = n.nodeMaster.Tzoffset,
                        TzDaylight = n.nodeMaster.Tzdaylight
                    }).OrderByDescending(x => x.Date).ToList();

                if (notificationTypeId.HasValue)
                {
                    notifications = notifications.Where(n => n.EventTypeId == notificationTypeId)
                        .OrderByDescending(x => x.Date).ToList();
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NotificationSQLStore)} {nameof(GetEventsByAssetGroupName)}", correlationId);

            return notifications;
        }

        /// <summary>
        /// Get the notifications by asset id and notification type id.
        /// </summary>
        /// <param name="assetId">The asset id/node id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{NotificationsTypesModel}"/>.</returns>
        public IList<NotificationsTypesModel> GetEventsGroupsByAssetId(string assetId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NotificationSQLStore)} {nameof(GetEventsGroupsByAssetId)}", correlationId);

            var notifications = new List<NotificationsTypesModel>();
            Guid guid = new Guid(assetId);
            using (var context = _contextFactory.GetContext())
            {
                notifications = (from eg in context.EventGroups.AsNoTracking()
                                 join e in context.Events.AsNoTracking() on eg.EventTypeId equals e.EventTypeId
                                 join nm in context.NodeMasters.AsNoTracking() on e.NodeId equals nm.NodeId
                                 where nm.AssetGuid == guid
                                 select new NotificationsTypesModel()
                                 {
                                     Id = eg.EventTypeId,
                                     Name = eg.Name,
                                 }).AsEnumerable()
                    .DistinctBy(notification => notification.Id)
                    .ToList();
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NotificationSQLStore)} {nameof(GetEventsGroupsByAssetId)}", correlationId);

            return notifications;
        }

        /// <summary>
        /// Get the notifications by asset group name and notification type id.
        /// </summary>
        /// <param name="assetGroupName">The asset group name.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{NotificationsTypesModel}"/>.</returns>
        public IList<NotificationsTypesModel> GetEventsGroupsByAssetGroupName(string assetGroupName, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NotificationSQLStore)} {nameof(GetEventsGroupsByAssetGroupName)}", correlationId);

            var notifications = new List<NotificationsTypesModel>();

            using (var context = _contextFactory.GetContext())
            {
                var nodeId = context.GroupMembershipCache.AsNoTracking().Where(a => a.GroupName == assetGroupName).Select(a => a.NodeId)
                    .ToList();

                var eventTypeId = context.Events.AsNoTracking().Where(x => nodeId.Contains(x.NodeId))
                    .Select(x => x.EventTypeId).Distinct().ToList();

                notifications = context.EventGroups.AsNoTracking().Where(a => eventTypeId.Contains(a.EventTypeId))
                    .Select(x => new NotificationsTypesModel
                    {
                        Id = x.EventTypeId,
                        Name = x.Name,
                    }).AsEnumerable()
                    .DistinctBy(notification => notification.Id)
                    .ToList();
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NotificationSQLStore)} {nameof(GetEventsGroupsByAssetGroupName)}", correlationId);

            return notifications;
        }

        /// <summary>
        /// Get the event type name by event type id.
        /// </summary>
        /// <param name="eventTypeId">The event type id to get the event name by.</param>
        /// <param name="correlationId"></param>
        /// <returns>The event type name as a string.</returns>
        public string GetEventTypeName(int eventTypeId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NotificationSQLStore)} {nameof(GetEventTypeName)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.EventGroups.AsNoTracking().Where(x => x.EventTypeId == eventTypeId).Select(x => x.Name).FirstOrDefault();

                logger.WriteCId(Level.Trace, $"Finished {nameof(NotificationSQLStore)} {nameof(GetEventTypeName)}", correlationId);

                return result;
            }
        }

        #endregion

    }
}
