using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.WellControl.Integration.Models;
using Theta.XSPOC.Apex.Kernel.Data.Updates.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Data.Services.Contracts.Mappers
{
    /// <summary>
    /// Maps a <seealso cref="UpdatePayload"/> to a <seealso cref="EventModel"/>.
    /// </summary>
    public class EventsPayloadMapper
    {

        /// <summary>
        /// Maps the <seealso cref="UpdatePayload"/> that came from the integration layer to the
        /// <seealso cref="EventModel"/> that will be handled by the persistence layer.
        /// </summary>
        /// <param name="input">The input to map.</param>
        /// <returns>
        /// A new <seealso cref="EventModel"/> for the provided <paramref name="input"/>.
        /// <c>null</c> is returned if any of the following are missing:
        /// <paramref name="input"/>
        /// <paramref name="input.Data"/>.
        /// </returns>
        public static EventModel Map(UpdatePayload input)
        {
            if (input == null)
            {
                return null;
            }
            var eventData = input.Data as List<UpdateColumnValuePair>;

            var eventModel = new EventModel();

            foreach (var data in eventData)
            {
                if (data.Column == null)
                {
                    continue;
                }

                switch (data.Column.ToLower())
                {
                    case "nodeid":
                        eventModel.NodeId = data.Value;
                        break;
                    case "enabled":
                        eventModel.EventTypeId = (int)EventType.EnableDisable;
                        eventModel.Note = int.Parse(data.Value) == 1 ? "Enabled for Scanning" : "Disabled";
                        eventModel.Date = DateTime.UtcNow;
                        break;
                    case "userid":
                        eventModel.UserId = data.Value;
                        break;
                    default:
                        break;
                }// switch (data.Column)
            }// foreach (var data in assetData)

            return eventModel;
        }

        /// <summary>
        /// Maps the <seealso cref="UpdatePayload"/> that came from the integration layer to the
        /// <seealso cref="LogEventData"/> that will be handled by the persistence layer.
        /// </summary>
        /// <param name="input">The input to map.</param>
        /// <returns>
        /// A new <seealso cref="LogEventData"/> for the provided <paramref name="input"/>.
        /// <c>null</c> is returned if any of the following are missing:
        /// <paramref name="input"/>
        /// <paramref name="input.Data"/>.
        /// </returns>
        public static LogEventData MapEventLogs(UpdatePayload input)
        {
            if (input == null)
            {
                return null;
            }
            var eventData = input.Data as List<UpdateColumnValuePair>;

            var eventModel = new LogEventData();

            foreach (var data in eventData)
            {
                if (data.Column == null)
                {
                    continue;
                }

                switch (data.Column.ToLower())
                {
                    case "nodeid":
                        eventModel.NodeId = data.Value;
                        break;
                    case "enabled":
                        eventModel.EventType = EventType.EnableDisable;
                        eventModel.Note = int.Parse(data.Value) == 1 ? "Enabled for Scanning" : "Disabled";
                        eventModel.Date = DateTime.UtcNow;
                        break;
                    case "userid":
                        eventModel.Source = data.Value;
                        break;
                    default:
                        break;
                }// switch (data.Column)
            }// foreach (var data in assetData)

            return eventModel;
        }
    }
}