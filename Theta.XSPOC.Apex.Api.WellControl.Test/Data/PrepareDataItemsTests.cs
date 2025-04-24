using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.WorkflowModels;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.V2;
using Theta.XSPOC.Apex.Api.WellControl.Data.Services;
using Theta.XSPOC.Apex.Api.WellControl.Data.Services.DbStoreManagers;
using Theta.XSPOC.Apex.Api.WellControl.Data.Services.Factories;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Data.Updates.Models;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Test.Data
{
    [TestClass]
    public class PrepareDataItemsTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            _ = new PrepareDataItems(null, new Mock<IPersistenceFactory>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullPersistenceFactoryTest()
        {
            _ = new PrepareDataItems(new Mock<IThetaLoggerFactory>().Object, null);
        }

        [TestMethod]
        public void PrepareDataItemsUpdateAsyncTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockPersistantFactory = new Mock<IPersistenceFactory>();

            var mockDbStoreManager = new Mock<IDbStoreManager>();

            mockPersistantFactory.Setup(x => x.Create("tblTransactions"))
                .Returns(mockDbStoreManager.Object);

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
            var dbStoreResult = new DbStoreResult
            {
                KindOfError = ErrorType.None,
                Message = string.Empty
            };

            mockDbStoreManager.Setup(x => x.UpdateAsync(JsonConvert.SerializeObject(payload), "correlationId1")).Returns(Task.FromResult(dbStoreResult));

            var prepareDataItemsService = new PrepareDataItems(mockThetaLoggerFactory.Object, mockPersistantFactory.Object);

            var message = new WithCorrelationId<DataUpdateEvent>(
                "correlationId1",
                inputData);

            var result = prepareDataItemsService.PrepareDataItemsAsync(message);

            Assert.IsNotNull(result);
            Assert.AreEqual(ConsumerBaseAction.Success, result.Result);
            mockDbStoreManager.Verify(x => x.UpdateAsync(JsonConvert.SerializeObject(payload), "correlationId1"), Times.Once);
        }

        [TestMethod]
        public void PrepareDataItemsUpdateAsyncNullMesageTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var dbStoreResult = new DbStoreResult
            {
                KindOfError = ErrorType.NotRecoverable,
                Message = string.Empty
            };

            var mockPersistantFactory = new Mock<IPersistenceFactory>();
            var mockDbStoreManager = new Mock<IDbStoreManager>();
            mockDbStoreManager.Setup(x => x.UpdateAsync(null, "correlationId1"))
                .Returns(Task.FromResult(dbStoreResult));

            mockPersistantFactory.Setup(x => x.Create("tblTransactions"))
                .Returns(mockDbStoreManager.Object);

            var prepareDataItemsService = new PrepareDataItems(mockThetaLoggerFactory.Object, mockPersistantFactory.Object);

            var result = prepareDataItemsService.PrepareDataItemsAsync(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(ConsumerBaseAction.Reject, result.Result);
            mockDbStoreManager.Verify(x => x.UpdateAsync(It.IsAny<string>(), "correlationId1"), Times.Never);
            logger.Verify(x => x.Write(Level.Error,
                It.Is<string>(x => x.Contains("Prepare data item failed because messageWithCorrelationId is missing."))),
                Times.Once);
        }

        [TestMethod]
        public void PrepareDataItemsInvalidDbStoreTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockPersistantFactory = new Mock<IPersistenceFactory>();

            var mockDbStoreManager = new Mock<IDbStoreManager>();

            mockPersistantFactory.Setup(x => x.Create("tblTransactions"))
                .Returns(mockDbStoreManager.Object);

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
                PayloadType = "tblNodeMaster",
                ResponseMetadata = ""
            };
            var data = new WithCorrelationId<DataUpdateEvent>("correlationId1", inputData);
            var dbStoreResult = new DbStoreResult
            {
                KindOfError = ErrorType.None,
                Message = string.Empty
            };

            mockDbStoreManager.Setup(x => x.UpdateAsync(JsonConvert.SerializeObject(payload), "correlationId1")).Returns(Task.FromResult(dbStoreResult));

            var prepareDataItemsService = new PrepareDataItems(mockThetaLoggerFactory.Object, mockPersistantFactory.Object);

            var message = new WithCorrelationId<DataUpdateEvent>(
                "correlationId1",
                inputData);

            var result = prepareDataItemsService.PrepareDataItemsAsync(message);

            Assert.IsNotNull(result);
            Assert.AreEqual(ConsumerBaseAction.Reject, result.Result);
            mockDbStoreManager.Verify(x => x.UpdateAsync(JsonConvert.SerializeObject(payload), "correlationId1"), Times.Never);
            logger.Verify(x => x.WriteCId(Level.Error,
                It.Is<string>(x => x.Contains("Failed to obtain a Db Store Manager.")), It.IsAny<Exception>(), "correlationId1"),
                Times.Once);
        }

        #endregion

    }
}
