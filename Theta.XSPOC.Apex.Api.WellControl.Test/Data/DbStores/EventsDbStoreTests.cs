using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Mongo;

namespace Theta.XSPOC.Apex.Api.WellControl.Test.Data.DbStores
{
    [TestClass]
    public class EventsDbStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullCollectionTest()
        {
            _ = new EventsDbStore(null);
        }

        public async Task UpdateAsyncEventModelIsNullTest()
        {
            var store = new EventsDbStore(new Mock<IMongoCollection<EventModel>>().Object);

            var result = await store.UpdateAsync(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UpdateAsyncEventsWithNodeIdNullTest()
        {
            var store = new EventsDbStore(new Mock<IMongoCollection<EventModel>>().Object);
            var eventData = new EventModel()
            {
                EventId = 1,
                NodeId = null,
                Date = DateTime.UtcNow,
                Description = null,
                EventTypeId = 21,
                Note = "Enabled for Scanning",
            };

            var result = await store.UpdateAsync(eventData);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UpdateAsyncEventsInsertTest()
        {
            var eventData = new EventModel()
            {
                EventId = 1,
                NodeId = "Well1",
                Date = DateTime.UtcNow,
                Description = null,
                EventTypeId = 21,
                Note = "Enabled for Scanning",
            };

            var mockCollection = new Mock<IMongoCollection<EventModel>>();
            mockCollection
                .SetupSequence(m => m.FindAsync(It.IsAny<FilterDefinition<EventModel>>(),
                                               It.IsAny<FindOptions<EventModel>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetNotFoundResult()))
                .Returns(Task.FromResult(GetNotFoundResult()));

            var store = new EventsDbStore(mockCollection.Object);

            var input = new EventModel()
            {
                EventId = 1,
                NodeId = "Well1",
                Date = DateTime.UtcNow,
                Description = null,
                EventTypeId = 21,
                Note = "Enabled for Scanning",
            };

            var result = await store.UpdateAsync(input);

            Assert.IsTrue(result);
            mockCollection.Verify(
                m => m.InsertOneAsync(It.IsAny<EventModel>(), It.IsAny<InsertOneOptions>(),
                                      It.IsAny<CancellationToken>()), Times.Once());

            mockCollection.Verify(
                m => m.ReplaceOneAsync(It.IsAny<FilterDefinition<EventModel>>(),
                                       It.Is<EventModel>(x => x.EventId == 1), It.IsAny<ReplaceOptions>(),
                                       It.IsAny<CancellationToken>()), Times.Never());
        }

        [TestMethod]
        public async Task UpdateAsyncEventsUpdateTest()
        {
            var eventData = new EventModel()
            {
                EventId = 1,
                NodeId = "Well1",
                Date = DateTime.UtcNow,
                Description = null,
                EventTypeId = 21,
                Note = "Enabled for Scanning",
            };

            var mockCollection = new Mock<IMongoCollection<EventModel>>();
            mockCollection
                .SetupSequence(m => m.FindAsync(It.IsAny<FilterDefinition<EventModel>>(),
                                               It.IsAny<FindOptions<EventModel>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetFoundResult(eventData)))
                .Returns(Task.FromResult(GetFoundResult(eventData)));

            var store = new EventsDbStore(mockCollection.Object);

            var input = new EventModel()
            {
                EventId = 1,
                NodeId = "Well1",
                Date = DateTime.UtcNow,
                Description = null,
                EventTypeId = 21,
                Note = "Enabled for Scanning",
            };

            var result = await store.UpdateAsync(input);

            Assert.IsTrue(result);
            mockCollection.Verify(
                m => m.InsertOneAsync(It.IsAny<EventModel>(), It.IsAny<InsertOneOptions>(),
                                      It.IsAny<CancellationToken>()), Times.Once());

            mockCollection.Verify(
                m => m.ReplaceOneAsync(It.IsAny<FilterDefinition<EventModel>>(),
                                       It.Is<EventModel>(x => x.EventId == 1), It.IsAny<ReplaceOptions>(),
                                       It.IsAny<CancellationToken>()), Times.Never());
        }

        #endregion

        #region Private Methods

        private static IAsyncCursor<EventModel> GetFoundResult(EventModel events)
        {
            var result = new Mock<IAsyncCursor<EventModel>>();
            result.Setup(m => m.Current).Returns(new List<EventModel>() { events });
            result.Setup(m => m.MoveNext(It.IsAny<CancellationToken>())).Returns(true);

            return result.Object;
        }

        private static IAsyncCursor<EventModel> GetNotFoundResult()
        {
            var result = new Mock<IAsyncCursor<EventModel>>();
            result.Setup(m => m.Current).Returns(new List<EventModel>());
            result.Setup(m => m.MoveNext(It.IsAny<CancellationToken>())).Returns(false);

            return result.Object;
        }

        #endregion

    }
}
