using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.WorkflowModels;
using Theta.XSPOC.Apex.Api.WellControl.Data.Services.DbStoreManagers;
using Theta.XSPOC.Apex.Kernel.Logging;
using Level = Theta.XSPOC.Apex.Kernel.Logging.Models.Level;
using SystemSerializer = System.Text.Json;

namespace Theta.XSPOC.Apex.Api.WellControl.Test.Services.DbStoreManagers
{
    [TestClass]
    public class DbStoreManagerBaseTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorGuardsAgainstNullLoggingFactoryTest()
        {
            _ = new DbStoreManagerBaseTestStub(null, new Mock<ITestDbStore>().Object, new Mock<IConfiguration>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorGuardsAgainstNullConfigurationTest()
        {
            _ = new DbStoreManagerBaseTestStub(new Mock<IThetaLoggerFactory>().Object, new Mock<ITestDbStore>().Object, null);
        }

        [TestMethod]
        public async Task UpdateAsyncTest()
        {
            SetupUtility.CreateMockLoggerFactory(out var logger, out var loggerFactory);

            var mockDbStore = new Mock<ITestDbStore>();

            var mockConfiguration = SetupUtility.GetMockMongoDbConfiguration();

            var dbStoreManager = new DbStoreManagerBaseTestStub(loggerFactory.Object, mockDbStore.Object, mockConfiguration);

            var testInput = new TestPayload()
            {
                Field1 = "Value 1",
                Field2 = 2,
            };

            var result = await dbStoreManager.UpdateAsyncTestStub(SystemSerializer.JsonSerializer.Serialize(testInput),
                "correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorType.None, result.KindOfError);

            logger.Verify(x => x.WriteCId(Level.Info, "Updating TestPayload.", "correlationId"), Times.Once);

            logger.Verify(x => x.WriteCId(Level.Debug, "Deserializing to TestPayload.", "correlationId"), Times.Once);
            logger.Verify(x => x.WriteCId(Level.Trace,
                It.Is<string>(s => s.Contains("Serialized payload received")), "correlationId"), Times.Once);
            logger.Verify(x => x.WriteCId(Level.Debug, "Successfully deserialized to TestPayload.", "correlationId"),
                Times.Once);

            logger.Verify(x => x.WriteCId(Level.Debug, "Mapping to TestDocument.", "correlationId"), Times.Once);
            logger.Verify(x => x.WriteCId(Level.Debug, "Successfully mapped to TestDocument.", "correlationId"),
                Times.Once);

            logger.Verify(x => x.WriteCId(Level.Debug, "Making database call to persist TestDocument.", "correlationId"),
                Times.Once);
            logger.Verify(x => x.WriteCId(Level.Debug, "TestDocument persisted.", "correlationId"), Times.Once);

            logger.Verify(x => x.WriteCId(Level.Info, "Successfully updated TestPayload.", "correlationId"), Times.Once);
        }

        [TestMethod]
        public async Task FailedDeserializationTest()
        {
            SetupUtility.CreateMockLoggerFactory(out var logger, out var loggerFactory);

            var mockConfiguration = SetupUtility.GetMockMongoDbConfiguration();

            var dbStoreManager = new DbStoreManagerBaseTestStub(loggerFactory.Object, new Mock<ITestDbStore>().Object, mockConfiguration);

            var result = await dbStoreManager.UpdateAsyncTestStub("incorrect payload", "correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorType.NotRecoverable, result.KindOfError);

            logger.Verify(x => x.WriteCId(Level.Debug, "Deserializing to TestPayload.", "correlationId"), Times.Once);
            logger.Verify(x => x.WriteCId(Level.Error, "Deserialization failed.", It.IsAny<JsonReaderException>(),
                "correlationId"));
            logger.Verify(x => x.WriteCId(Level.Info, "Successfully updated TestPayload.", "correlationId"), Times.Never);
        }

        [TestMethod]
        public async Task MapNotImplementedTest()
        {
            SetupUtility.CreateMockLoggerFactory(out var logger, out var loggerFactory);

            var mockConfiguration = SetupUtility.GetMockMongoDbConfiguration();

            var testInput = new TestPayload()
            {
                Field1 = "Value 1",
                Field2 = 2,
            };

            var dbStoreManager = new DbStoreManagerBaseMapNotImplementedTestStub(loggerFactory.Object, mockConfiguration);

            var result = await dbStoreManager.UpdateAsyncTestStub(SystemSerializer.JsonSerializer.Serialize(testInput),
                "correlationId");

            Assert.IsNotNull(result);

            Assert.AreEqual(ErrorType.NotRecoverable, result.KindOfError);
            Assert.AreEqual("Map must be implemented in order to use UpdateAsync", result.Message);

            logger.Verify(x => x.WriteCId(Level.Error, "Mapping failed.",
                It.Is<NotImplementedException>(e => e.Message == "Map must be implemented in order to use UpdateAsync"),
                "correlationId"));

            logger.Verify(x => x.WriteCId(Level.Debug, "Mapping to TestDocument.", "correlationId"), Times.Once);
            logger.Verify(x => x.WriteCId(Level.Debug, "Successfully mapped to TestDocument.", "correlationId"),
                Times.Never);
            logger.Verify(x => x.WriteCId(Level.Info, "Successfully updated TestPayload.", "correlationId"), Times.Never);
        }

        [TestMethod]
        public async Task MapReturnsNullTest()
        {
            SetupUtility.CreateMockLoggerFactory(out var logger, out var loggerFactory);

            var mockConfiguration = SetupUtility.GetMockMongoDbConfiguration();

            var testInput = new TestPayload()
            {
                Field1 = "Value 1",
                Field2 = 2,
            };

            var dbStoreManager = new DbStoreManagerBaseMapReturnsNullTestStub(loggerFactory.Object, mockConfiguration);

            var result = await dbStoreManager.UpdateAsyncTestStub(SystemSerializer.JsonSerializer.Serialize(testInput),
                "correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual("At least one required identifier was missing from the input. Mapping failed.",
                result.Message);

            logger.Verify(x => x.WriteCId(Level.Debug, "Mapping to TestDocument.", "correlationId"), Times.Once);

            Assert.AreEqual(ErrorType.NotRecoverable, result.KindOfError);

            logger.Verify(x => x.WriteCId(Level.Error, "Mapping failed: null returned from mapper.", "correlationId"));
            logger.Verify(x => x.WriteCId(Level.Debug, "Successfully mapped to TestDocument.", "correlationId"),
                Times.Once);
            logger.Verify(x => x.WriteCId(Level.Info, "Successfully updated TestPayload.", "correlationId"), Times.Never);
        }

        [TestMethod]
        public async Task UpdateDbStoreNotImplementedTest()
        {
            SetupUtility.CreateMockLoggerFactory(out var logger, out var loggerFactory);

            var mockConfiguration = SetupUtility.GetMockMongoDbConfiguration();

            var testInput = new TestPayload()
            {
                Field1 = "Value 1",
                Field2 = 2,
            };

            var dbStoreManager = new DbStoreManagerBaseUpdateDbStoreNotImplementedTestStub(loggerFactory.Object, mockConfiguration);

            var result = await dbStoreManager.UpdateAsyncTestStub(SystemSerializer.JsonSerializer.Serialize(testInput),
                "correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorType.NotRecoverable, result.KindOfError);

            logger.Verify(x => x.WriteCId(Level.Debug, "Making database call to persist TestDocument.", "correlationId"),
                Times.Once);
            logger.Verify(
                x => x.WriteCId(
                It.IsAny<Level>(),
                It.IsAny<string>(),
                It.IsAny<NotImplementedException>(),
                It.IsAny<string>()),
                Times.AtLeastOnce);

            logger.Verify(x => x.WriteCId(Level.Debug, "TestDocument persisted.", "correlationId"), Times.Never);
            logger.Verify(x => x.WriteCId(Level.Info, "Successfully updated TestPayload.", "correlationId"), Times.Never);
        }

        #endregion

        #region Test Stub Classes

        // Because this is a base class under test, several helper classes are needed to stub it out. These classes
        // have no utility outside this test class, so they are internal here.

        /// <summary>
        /// Test stub for full functionality.
        /// </summary>
        internal class DbStoreManagerBaseTestStub : DbStoreManagerBase
        {

            private readonly ITestDbStore _store;

            /// <summary>
            /// Constructs a test stub for full functionality.
            /// </summary>
            /// <param name="loggerFactory">The logger factory.</param>
            /// <param name="store">The db store.</param>
            public DbStoreManagerBaseTestStub(IThetaLoggerFactory loggerFactory, ITestDbStore store,
                IConfiguration configuration) : base(loggerFactory, configuration)
            {
                _store = store;
            }

            /// <summary>
            /// Test method, set up to test full functionality.
            /// </summary>
            /// <param name="payload">The payload to test with.</param>
            /// <param name="correlationId">The correlation id to test with.</param>
            /// <returns>A <seealso cref="DbStoreResult"/> describing the outcome of the operation.</returns>
            public async Task<DbStoreResult> UpdateAsyncTestStub(string payload, string correlationId)
            {
                return await UpdateAsync<TestPayload, TestDocument>(payload, correlationId);
            }

            /// <summary>
            /// Test method, set up to test full functionality.
            /// </summary>
            /// <param name="document">The document to test with.</param>
            /// <returns>The return value of the operation.</returns>
            protected override async Task<bool> UpdateDbStore<TDocument>(TDocument document)
            {
                var convertedDocument = document as TestDocument;

                return await _store.UpdateAsync(convertedDocument);
            }

            /// <summary>
            /// Test method, set up for testing full functionality.
            /// </summary>
            /// <param name="payload">The payload to test with.</param>
            /// <returns>
            /// A mapped object that can be used for testing.
            /// </returns>
            protected override TDocument Map<TPayload, TDocument>(TPayload payload) where TDocument : class
            {
                var input = payload as TestPayload;

                var result = new TestDocument()
                {
                    FirstField = input.Field1,
                    SecondField = input.Field2,
                };

                return result as TDocument;
            }

        }

        /// <summary>
        /// Test stub for testing missing mapping functionality.
        /// </summary>
        internal class DbStoreManagerBaseMapNotImplementedTestStub : DbStoreManagerBase
        {

            /// <summary>
            /// Constructs a test stub for testing missing mapping functionality.
            /// </summary>
            /// <param name="loggerFactory">The logger factory.</param>
            public DbStoreManagerBaseMapNotImplementedTestStub(IThetaLoggerFactory loggerFactory,
                IConfiguration configuration)
                : base(loggerFactory, configuration)
            {

            }

            /// <summary>
            /// Test method, set up for testing missing mapping functionality.
            /// </summary>
            /// <param name="payload">The payload to test with.</param>
            /// <param name="correlationId">The correlation id to test with.</param>
            /// <returns>A <seealso cref="DbStoreResult"/> describing the outcome of the operation.</returns>
            public async Task<DbStoreResult> UpdateAsyncTestStub(string payload, string correlationId)
            {
                return await UpdateAsync<TestPayload, TestDocument>(payload, correlationId);
            }

            protected override TDocument Map<TPayload, TDocument>(TPayload payload)
            {
                throw new NotImplementedException("Map must be implemented in order to use UpdateAsync");
            }

            protected override Task<bool> UpdateDbStore<TDocument>(TDocument document)
            {
                throw new NotImplementedException("UpdateDbStore must be implemented in order to use UpdateAsync");
            }
        }

        /// <summary>
        /// Test stub for testing behavior when returning a null from the mapper.
        /// </summary>
        internal class DbStoreManagerBaseMapReturnsNullTestStub : DbStoreManagerBase
        {

            /// <summary>
            /// Constructs a test stub for testing behavior when returning a null from the mapper.
            /// </summary>
            /// <param name="loggerFactory">The logger factory.</param>
            public DbStoreManagerBaseMapReturnsNullTestStub(IThetaLoggerFactory loggerFactory,
                IConfiguration configuration)
                : base(loggerFactory, configuration)
            {

            }

            /// <summary>
            /// Test method, set up for testing behavior when returning a null from the mapper.
            /// </summary>
            /// <param name="payload">The payload to test with.</param>
            /// <returns>
            /// A mapped object that can be used for testing.
            /// </returns>
            protected override TDocument Map<TPayload, TDocument>(TPayload payload) where TDocument : class
            {
                return null;
            }

            /// <summary>
            /// Test method, for testing behavior when returning a null from the mapper.
            /// </summary>
            /// <param name="payload">The payload to test with.</param>
            /// <param name="correlationId">The correlation id to test with.</param>
            /// <returns>A <seealso cref="DbStoreResult"/> describing the outcome of the operation.</returns>
            public async Task<DbStoreResult> UpdateAsyncTestStub(string payload, string correlationId)
            {
                return await UpdateAsync<TestPayload, TestDocument>(payload, correlationId);
            }

            protected override Task<bool> UpdateDbStore<TDocument>(TDocument document)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Test stub for testing when the db store is not implemented.
        /// </summary>
        internal class DbStoreManagerBaseUpdateDbStoreNotImplementedTestStub : DbStoreManagerBase
        {

            /// <summary>
            /// Constructs a test stub for testing when the db store is not implemented.
            /// </summary>
            /// <param name="loggerFactory">The logger factory.</param>
            public DbStoreManagerBaseUpdateDbStoreNotImplementedTestStub(IThetaLoggerFactory loggerFactory,
                IConfiguration configuration)
                : base(loggerFactory, configuration)
            {

            }

            /// <summary>
            /// Test method, set up for testing when the db store is not implemented.
            /// </summary>
            /// <param name="payload">The payload to test with.</param>
            /// <returns>
            /// A mapped object that can be used for testing.
            /// </returns>
            protected override TDocument Map<TPayload, TDocument>(TPayload payload) where TDocument : class
            {
                var input = payload as TestPayload;

                var result = new TestDocument()
                {
                    FirstField = input.Field1,
                    SecondField = input.Field2,
                };

                return result as TDocument;
            }

            /// <summary>
            /// Test method, set up for testing when the db store is not implemented.
            /// </summary>
            /// <param name="payload">The payload to test with.</param>
            /// <param name="correlationId">The correlation id to test with.</param>
            /// <returns>A <seealso cref="DbStoreResult"/> describing the outcome of the operation.</returns>
            public async Task<DbStoreResult> UpdateAsyncTestStub(string payload, string correlationId)
            {
                return await UpdateAsync<TestPayload, TestDocument>(payload, correlationId);
            }

            protected override Task<bool> UpdateDbStore<TDocument>(TDocument document)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Internal class to use with the stub classes above.
        /// </summary>
        internal class TestPayload
        {

            /// <summary>
            /// A test field to use with the stub classes above.
            /// </summary>
            public string Field1 { get; set; }

            /// <summary>
            /// A test field to use with the stub classes above.
            /// </summary>
            public int Field2 { get; set; }

        }

        /// <summary>
        /// Moq only works on public interfaces, so this has to be public.
        /// </summary>
        public class TestDocument
        {

            /// <summary>
            /// A test field to use with the stub classes above.
            /// </summary>
            public string FirstField { get; set; }

            /// <summary>
            /// A test field to use with the stub classes above.
            /// </summary>
            public int SecondField { get; set; }

        }

        /// <summary>
        /// A stub interface to use with the stub classes above.
        /// </summary>
        /// <remarks>
        /// Moq only works on public interfaces, so this has to be public.
        /// </remarks>
        public interface ITestDbStore
        {

            /// <summary>
            /// A test field to use with the stub classes above.
            /// </summary>
            Task<bool> UpdateAsync(TestDocument document);

        }

        #endregion

    }
}