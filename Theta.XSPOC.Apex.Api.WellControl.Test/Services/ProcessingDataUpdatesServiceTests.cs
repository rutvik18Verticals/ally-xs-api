using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.Communications;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.WellControl.Contracts;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.V2;
using Theta.XSPOC.Apex.Api.WellControl.Data.Services;
using Theta.XSPOC.Apex.Api.WellControl.Integration.Models;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Api.WellControl.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Data.Updates.Models;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Test.Services
{
    [TestClass]
    public class ProcessingDataUpdatesServiceTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();

            _ = new ProcessingDataUpdatesService(null, mockPublisher.Object, mockMongoDataStore.Object,
                mockSqlStore.Object, transactionPayloadCreatorMock.Object, portConfigurationStoreMock.Object
                , notificationMock.Object, dateTimeConverterMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullPublisherTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();

            _ = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, null, mockMongoDataStore.Object,
                mockSqlStore.Object, transactionPayloadCreatorMock.Object, portConfigurationStoreMock.Object
                , notificationMock.Object, dateTimeConverterMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullDataStoreTest()
        {
            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();

            _ = new ProcessingDataUpdatesService(null, mockPublisher.Object, mockMongoDataStore.Object,
                mockSqlStore.Object, transactionPayloadCreatorMock.Object, portConfigurationStoreMock.Object
                , notificationMock.Object, dateTimeConverterMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullSQLDataStoreTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<TestIPublishMessage>()
            {
                new TestIPublishMessage(),
            };
            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();

            _ = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, null, transactionPayloadCreatorMock.Object, portConfigurationStoreMock.Object
                , notificationMock.Object, dateTimeConverterMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullPortConfigurationStoreTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<TestIPublishMessage>()
            {
                new TestIPublishMessage(),
            };
            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());
            var mockSqlStore = new Mock<INodeMaster>();

            _ = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, mockSqlStore.Object, transactionPayloadCreatorMock.Object, null
                , notificationMock.Object, dateTimeConverterMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullTransactionPayloadCreatorTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<TestIPublishMessage>()
            {
                new TestIPublishMessage(),
            };
            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());
            var mockSqlStore = new Mock<INodeMaster>();

            _ = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, mockSqlStore.Object, null, portConfigurationStoreMock.Object
                , notificationMock.Object, dateTimeConverterMock.Object);
        }

        [TestMethod]
        public void ProcessingDataUpdatesServiceTest()
        {
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            portConfigurationStoreMock.Setup(x => x.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestIPublishMessage(),
                new TestIPublishMessage2(),
                new TestIPublishMessage3(),
            };

            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());

            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();
            var assetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = assetGuid,
                    Enabled = true
                }
            }.AsQueryable();

            mockSqlStore.Setup(x => x.GetNode(assetGuid, It.IsAny<string>()))
                .Returns(nodeData.FirstOrDefault());
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();

            var service = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, mockSqlStore.Object, transactionPayloadCreatorMock.Object,
                portConfigurationStoreMock.Object, notificationMock.Object, dateTimeConverterMock.Object);

            var payload = new UpdatePayload
            {
                Key = new List<UpdateColumnValuePair>()
                {
                    new UpdateColumnValuePair()
                    {
                        Column = "TransactionID",
                        Value = "579961220",
                    },
                },
                Data = new List<UpdateColumnValuePair>()
                {
                    new UpdateColumnValuePair()
                    {
                        Column = "TransactionID",
                        Value = "579961220",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "DateRequest",
                        Value = "11/8/2023 1:46:50 PM",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "PortID",
                        Value = "0",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Task",
                        Value = "WellControl",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Priority",
                        Value = "5",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "NodeID",
                        Value = "Theta Smarten",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Input",
                        Value = "AA0AVABoAGUAdABhACAAUwBtAGEAcgB0AGUAbgAIABEABw==",
                    },
                },
            };
            var inputData = new DataUpdateEvent
            {
                Action = "Insert",
                Payload = JsonConvert.SerializeObject(payload),
                PayloadType = "tblTransactions",
                ResponseMetadata = ""
            };
            var data = new WithCorrelationId<DataUpdateEvent>("correlationId1", inputData);
            mockMongoDataStore.Setup(x => x.PrepareDataItemsAsync(data)).Returns(Task.FromResult(ConsumerBaseAction.Success));

            var result = service.ProcessDataUpdatesAsync(data, assetGuid);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, ConsumerBaseAction.Success);
            logger.Verify(x => x.WriteCNId(Level.Info,
                It.Is<string>(x => x.Contains("Finished ProcessTransactionAsync for transaction id 579961220.")), "correlationId1", "Theta Smarten"),
                Times.Once);
        }

        [TestMethod]
        public void ProcessingDataUpdatesServiceFailTest()
        {
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            portConfigurationStoreMock.Setup(x => x.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestIPublishMessage(),
                new TestIPublishMessage2(),
                new TestIPublishMessage3(),
            };
            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();

            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();
            var assetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = assetGuid,
                    Enabled = true
                }
            }.AsQueryable();

            mockSqlStore.Setup(x => x.GetNode(assetGuid, It.IsAny<string>()))
                .Returns(nodeData.FirstOrDefault());

            var service = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, mockSqlStore.Object, transactionPayloadCreatorMock.Object,
                portConfigurationStoreMock.Object, notificationMock.Object, dateTimeConverterMock.Object);

            var payload = new UpdatePayload
            {
                Key = new List<UpdateColumnValuePair>()
                {
                    new UpdateColumnValuePair()
                    {
                        Column = "TransactionID",
                        Value = "579961220",
                    },
                },
                Data = new List<UpdateColumnValuePair>()
                {
                    new UpdateColumnValuePair()
                    {
                        Column = "DateRequest",
                        Value = "11/8/2023 1:46:50 PM",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "PortID",
                        Value = "0",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Task",
                        Value = "WellControl",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Priority",
                        Value = "5",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "NodeID",
                        Value = "Theta Smarten",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Input",
                        Value = "AA0AVABoAGUAdABhACAAUwBtAGEAcgB0AGUAbgAIABEABw==",
                    },
                },
            };
            var inputData = new DataUpdateEvent
            {
                Action = "Insert",
                Payload = JsonConvert.SerializeObject(payload),
                PayloadType = "tblTransactions",
                ResponseMetadata = ""
            };
            var data = new WithCorrelationId<DataUpdateEvent>("correlationId1", inputData);
            var result = service.ProcessDataUpdatesAsync(data, assetGuid);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, ConsumerBaseAction.Reject);
            logger.Verify(x => x.WriteCNId(Level.Info,
                It.Is<string>(x => x.Contains("Finished ProcessTransactionAsync for transaction id 579961220.")), "correlationId1", "Theta Smarten"),
                Times.Never);
        }

        [TestMethod]
        public void ProcessingDataUpdatesServiceNullPayloadTest()
        {
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            portConfigurationStoreMock.Setup(x => x.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestIPublishMessage(),
                new TestIPublishMessage2(),
                new TestIPublishMessage3(),
            };

            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());

            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();

            var service = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, mockSqlStore.Object, transactionPayloadCreatorMock.Object,
                portConfigurationStoreMock.Object, notificationMock.Object, dateTimeConverterMock.Object);

            var inputData = new DataUpdateEvent
            {
                Action = "Insert",
                Payload = null,
                PayloadType = "tblTransactions",
                ResponseMetadata = ""
            };

            var data = new WithCorrelationId<DataUpdateEvent>("correlationId1", inputData);
            var result = service.ProcessDataUpdatesAsync(data, new Guid());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, ConsumerBaseAction.Reject);
            logger.Verify(x => x.WriteCId(Level.Error,
                It.Is<string>(x => x.Contains("Could not extract transaction from message: data incomplete.")), "correlationId1"),
                Times.Once);
        }

        [TestMethod]
        public void ProcessingDataUpdatesServiceNullPayloadTypeTest()
        {
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            portConfigurationStoreMock.Setup(x => x.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestIPublishMessage(),
                new TestIPublishMessage2(),
                new TestIPublishMessage3(),
            };

            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());

            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();

            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();

            var service = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, mockSqlStore.Object, transactionPayloadCreatorMock.Object,
                portConfigurationStoreMock.Object, notificationMock.Object, dateTimeConverterMock.Object);

            var payload = "test payload";

            var inputData = new DataUpdateEvent
            {
                Action = "Insert",
                Payload = JsonConvert.SerializeObject(payload),
                PayloadType = null,
                ResponseMetadata = ""
            };

            var data = new WithCorrelationId<DataUpdateEvent>("correlationId1", inputData);
            var result = service.ProcessDataUpdatesAsync(data, new Guid());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, ConsumerBaseAction.Reject);
            logger.Verify(x => x.WriteCId(Level.Error,
                It.Is<string>(x => x.Contains("Could not extract transaction from message: data incomplete.")), "correlationId1"),
                Times.Once);
        }

        [TestMethod]
        public void ProcessingDataUpdatesServiceInvalidPayloadTypeTest()
        {
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            portConfigurationStoreMock.Setup(x => x.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestIPublishMessage(),
                new TestIPublishMessage2(),
                new TestIPublishMessage3(),
            };

            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());

            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();

            var service = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, mockSqlStore.Object, transactionPayloadCreatorMock.Object,
                portConfigurationStoreMock.Object, notificationMock.Object, dateTimeConverterMock.Object);

            var payload = "test payload";

            var inputData = new DataUpdateEvent
            {
                Action = "Insert",
                Payload = JsonConvert.SerializeObject(payload),
                PayloadType = "tblNodeMaster",
                ResponseMetadata = ""
            };

            var data = new WithCorrelationId<DataUpdateEvent>("correlationId1", inputData);
            var result = service.ProcessDataUpdatesAsync(data, new Guid());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, ConsumerBaseAction.Reject);
            logger.Verify(x => x.WriteCId(Level.Error,
                It.Is<string>(x => x.Contains("tblNodeMaster is not supported.")), "correlationId1"),
                Times.Once);
        }

        [TestMethod]
        public void ProcessingDataUpdatesServiceInvalidActionTest()
        {
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            portConfigurationStoreMock.Setup(x => x.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestIPublishMessage(),
                new TestIPublishMessage2(),
                new TestIPublishMessage3(),
            };
            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());

            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();

            var service = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, mockSqlStore.Object, transactionPayloadCreatorMock.Object,
                portConfigurationStoreMock.Object, notificationMock.Object, dateTimeConverterMock.Object);

            var payload = "test payload";

            var inputData = new DataUpdateEvent
            {
                Action = "Delete",
                Payload = JsonConvert.SerializeObject(payload),
                PayloadType = "tblTransactions",
                ResponseMetadata = ""
            };

            var data = new WithCorrelationId<DataUpdateEvent>("correlationId1", inputData);
            var result = service.ProcessDataUpdatesAsync(data, new Guid());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, ConsumerBaseAction.Reject);
            logger.Verify(x => x.WriteCId(Level.Error,
                It.Is<string>(x => x.Contains("Received invalid action Delete.")), "correlationId1"),
                Times.Once);
        }

        [TestMethod]
        public void ProcessingDataUpdatesServiceInvalidPayloadTest()
        {
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            portConfigurationStoreMock.Setup(x => x.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestIPublishMessage(),
                new TestIPublishMessage2(),
                new TestIPublishMessage3(),
            };

            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());

            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();
            var assetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = assetGuid,
                    Enabled = true
                }
            }.AsQueryable();

            mockSqlStore.Setup(x => x.GetNode(assetGuid, It.IsAny<string>()))
                .Returns(nodeData.FirstOrDefault());
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();

            var service = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, mockSqlStore.Object, transactionPayloadCreatorMock.Object,
                portConfigurationStoreMock.Object, notificationMock.Object, dateTimeConverterMock.Object);

            var payload = "test payload";

            var inputData = new DataUpdateEvent
            {
                Action = "Insert",
                Payload = JsonConvert.SerializeObject(payload),
                PayloadType = "tblTransactions",
                ResponseMetadata = ""
            };

            var data = new WithCorrelationId<DataUpdateEvent>("correlationId1", inputData);
            var result = service.ProcessDataUpdatesAsync(data, assetGuid);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, ConsumerBaseAction.Reject);
            logger.Verify(x => x.WriteCId(Level.Error,
                It.Is<string>(x => x.Contains("Error processing Insert for tblTransactions.")), It.IsAny<Exception>(), "correlationId1"),
                Times.Once);
        }

        [TestMethod]
        public void ProcessingWellControlActionOnDisabledTest()
        {
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            portConfigurationStoreMock.Setup(x => x.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestIPublishMessage(),
                new TestIPublishMessage2(),
                new TestIPublishMessage3(),
            };

            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());

            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();
            var assetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "Theta Smarten",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = assetGuid,
                    Enabled = false
                }
            }.AsQueryable();

            mockSqlStore.Setup(x => x.GetNode(assetGuid, It.IsAny<string>()))
                .Returns(nodeData.FirstOrDefault());
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();

            var service = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, mockSqlStore.Object, transactionPayloadCreatorMock.Object,
                portConfigurationStoreMock.Object, notificationMock.Object, dateTimeConverterMock.Object);

            var payload = new UpdatePayload
            {
                Key = new List<UpdateColumnValuePair>()
                {
                    new UpdateColumnValuePair()
                    {
                        Column = "TransactionID",
                        Value = "579961220",
                    },
                },
                Data = new List<UpdateColumnValuePair>()
                {
                    new UpdateColumnValuePair()
                    {
                        Column = "DateRequest",
                        Value = "11/8/2023 1:46:50 PM",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "PortID",
                        Value = "0",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Task",
                        Value = "WellControl",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Priority",
                        Value = "5",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "NodeID",
                        Value = "Theta Smarten",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Input",
                        Value = "AA0AVABoAGUAdABhACAAUwBtAGEAcgB0AGUAbgAIABEABw==",
                    },
                },
            };
            var inputData = new DataUpdateEvent
            {
                Action = "Insert",
                Payload = JsonConvert.SerializeObject(payload),
                PayloadType = "tblTransactions",
                ResponseMetadata = ""
            };
            var data = new WithCorrelationId<DataUpdateEvent>("correlationId1", inputData);
            var result = service.ProcessDataUpdatesAsync(data, assetGuid);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, ConsumerBaseAction.Reject);
            logger.Verify(x => x.WriteCNId(Level.Error,
                It.Is<string>(x => x.Contains("Cannot perform action on a disabled asset.")),
                "correlationId1", "Theta Smarten"), Times.Once);
        }

        [TestMethod]
        public void ProcessingDataUpdatesServiceLegacyTest()
        {
            var notificationMock = new Mock<INotification>();
            var dateTimeConverterMock = new Mock<IDateTimeConverter>();
            var portConfigurationStoreMock = new Mock<IPortConfigurationStore>();
            portConfigurationStoreMock.Setup(x => x.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var testPublish1 = new Mock<IPublishMessage<ProcessDataUpdateContract, Responsibility>>();
            testPublish1.Setup(x => x.Responsibility).Returns(Responsibility.PublishTransationDataToMicroservices);
            var testPublish2 = new Mock<IPublishMessage<ProcessDataUpdateContract, Responsibility>>();
            testPublish2.Setup(x => x.Responsibility).Returns(Responsibility.PublishTransationIdToListener);
            var testPublish3 = new Mock<IPublishMessage<ProcessDataUpdateContract, Responsibility>>();
            testPublish3.Setup(x => x.Responsibility).Returns(Responsibility.PublishStoreUpdateDataToLegacyDBStore);

            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                testPublish1.Object,
                testPublish2.Object,
                testPublish3.Object
            };

            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());

            var mockMongoDataStore = new Mock<IPrepareDataItems>();
            var mockSqlStore = new Mock<INodeMaster>();
            var assetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = assetGuid,
                    Enabled = true
                }
            }.AsQueryable();

            mockSqlStore.Setup(x => x.GetNode(assetGuid, It.IsAny<string>()))
                .Returns(nodeData.FirstOrDefault());
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();

            var service = new ProcessingDataUpdatesService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                mockMongoDataStore.Object, mockSqlStore.Object, transactionPayloadCreatorMock.Object,
                portConfigurationStoreMock.Object, notificationMock.Object, dateTimeConverterMock.Object);

            var payload = new UpdatePayload
            {
                Key = new List<UpdateColumnValuePair>()
                {
                    new UpdateColumnValuePair()
                    {
                        Column = "TransactionID",
                        Value = "579961220",
                    },
                },
                Data = new List<UpdateColumnValuePair>()
                {
                    new UpdateColumnValuePair()
                    {
                        Column = "TransactionID",
                        Value = "579961220",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "DateRequest",
                        Value = "11/8/2023 1:46:50 PM",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "PortID",
                        Value = "0",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Task",
                        Value = "WellControl",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Priority",
                        Value = "5",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "NodeID",
                        Value = "Theta Smarten",
                    },
                    new UpdateColumnValuePair()
                    {
                        Column = "Input",
                        Value = "AA0AVABoAGUAdABhACAAUwBtAGEAcgB0AGUAbgAIABEABw==",
                    },
                },
            };
            var inputData = new DataUpdateEvent
            {
                Action = "Insert",
                Payload = JsonConvert.SerializeObject(payload),
                PayloadType = "tblTransactions",
                ResponseMetadata = ""
            };
            var data = new WithCorrelationId<DataUpdateEvent>("correlationId1", inputData);
            mockMongoDataStore.Setup(x => x.PrepareDataItemsAsync(data)).Returns(Task.FromResult(ConsumerBaseAction.Success));

            var result = service.ProcessDataUpdatesAsync(data, assetGuid);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, ConsumerBaseAction.Success);
            logger.Verify(x => x.WriteCNId(Level.Info,
                It.Is<string>(x => x.Contains("Finished ProcessTransactionAsync for transaction id 579961220.")), "correlationId1", "Theta Smarten"),
                Times.Once);
            testPublish1.Verify(x => x.PublishMessageAsync(It.IsAny<WithCorrelationId<ProcessDataUpdateContract>>(),
                It.IsAny<string>()), Times.Never);
            testPublish2.Verify(x => x.PublishMessageAsync(It.IsAny<WithCorrelationId<ProcessDataUpdateContract>>(),
                It.IsAny<string>()), Times.Once);
            testPublish3.Verify(x => x.PublishMessageAsync(It.IsAny<WithCorrelationId<ProcessDataUpdateContract>>(),
                It.IsAny<string>()), Times.Once);
        }

        #endregion

        #region Utility Class

        private class TestIPublishMessage : IPublishMessage<ProcessDataUpdateContract, Responsibility>
        {

            #region Implementation of IDisposable

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region Implementation of IPublishMessage<OnDemandTransaction>

            public Task PublishMessageAsync(string data, string correlationId, string topic = null)
            {
                return Task.CompletedTask;
            }

            public Task PublishMessageAsync(WithCorrelationId<ProcessDataUpdateContract> data, string topic = null)
            {
                return Task.CompletedTask;
            }

            #endregion

            #region Implementation of IPublishMessage<OnDemandTransaction,out Responsibility>

            public Responsibility Responsibility => Responsibility.PublishTransationDataToMicroservices;

            #endregion

        }

        private class TestIPublishMessage2 : IPublishMessage<ProcessDataUpdateContract, Responsibility>
        {

            #region Implementation of IDisposable

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region Implementation of IPublishMessage<OnDemandTransaction>

            public Task PublishMessageAsync(string data, string correlationId, string topic = null)
            {
                return Task.CompletedTask;
            }

            public Task PublishMessageAsync(WithCorrelationId<ProcessDataUpdateContract> data, string topic = null)
            {
                return Task.CompletedTask;
            }

            #endregion

            #region Implementation of IPublishMessage<OnDemandTransaction,out Responsibility>

            public Responsibility Responsibility => Responsibility.PublishTransationIdToListener;

            #endregion

        }

        private class TestIPublishMessage3 : IPublishMessage<ProcessDataUpdateContract, Responsibility>
        {

            #region Implementation of IDisposable

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region Implementation of IPublishMessage<OnDemandTransaction>

            public Task PublishMessageAsync(string data, string correlationId, string topic = null)
            {
                return Task.CompletedTask;
            }

            public Task PublishMessageAsync(WithCorrelationId<ProcessDataUpdateContract> data, string topic = null)
            {
                return Task.CompletedTask;
            }

            #endregion

            #region Implementation of IPublishMessage<OnDemandTransaction,out Responsibility>

            public Responsibility Responsibility => Responsibility.PublishStoreUpdateDataToLegacyDBStore;

            #endregion

        }

        #endregion

    }
}
