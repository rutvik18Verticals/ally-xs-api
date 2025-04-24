using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Manages access to the <seealso cref="EventModel"/> collection.
    /// </summary>
    public class EventsDbStore : IEventsDbStore
    {

        #region Private Fields

        private readonly IMongoCollection<EventModel> _eventsCollection;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of the <seealso cref="EventsDbStore"/>.
        /// </summary>
        /// <param name="eventDataCollection">
        /// The <seealso cref="IMongoCollection{TDocument}"/> that this store is responsible for.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the eventDataCollection is null.
        /// </exception>
        public EventsDbStore(IMongoCollection<EventModel> eventDataCollection)
        {
            _eventsCollection = eventDataCollection ?? throw new ArgumentNullException(nameof(eventDataCollection));
        }

        #endregion

        #region IAssetDbStore Implementation

        /// <summary>
        /// Persists the given <param name="eventData"/> in the data store. Event data will be added.
        /// </summary>
        /// <returns><c>true</c> if the persistence succeeded, false otherwise.</returns>
        public async Task<bool> UpdateAsync(EventModel eventData)
        {
            if (eventData == null)
            {
                return false;
            }

            if (eventData.NodeId == null)
            {
                return false;
            }

            await _eventsCollection.InsertOneAsync(eventData);

            return true;
        }

        #endregion

    }
}
