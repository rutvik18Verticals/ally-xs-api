using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Theta.XSPOC.Apex.Api.Common.Communications;
using Theta.XSPOC.Apex.Api.Common.Communications.Models;
using Theta.XSPOC.Apex.Api.Data;

namespace Theta.XSPOC.Apex.Api.Common.Tests.Communications
{
    [TestClass]
    public class TransactionPayloadCreatorTests
    {

        #region Constructor Tests

        public void ConstructorNullNodeMasterServiceTest()
        {
            _ = new TransactionPayloadCreator(
                null,
                new Mock<ITransaction>().Object,
                new Mock<IParameterDataType>().Object);
        }

        public void ConstructorNullTransactionServiceTest()
        {
            _ = new TransactionPayloadCreator(
                new Mock<INodeMaster>().Object,
                null,
                new Mock<IParameterDataType>().Object);
        }

        public void ConstructorNullParameterDataTypeServiceTest()
        {
            _ = new TransactionPayloadCreator(
                new Mock<INodeMaster>().Object,
                null,
                new Mock<IParameterDataType>().Object);
        }

        #endregion

        #region Test Methods

        #region ReadRegisterPayload Tests

        [TestMethod]
        public void CreateReadRegisterPayloadTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var addresses = new[] { "10001", "10003", "10004" };

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = 32;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(true);

            var result = service.CreateReadRegisterPayload(assetId, addresses, correlationId, out var payload);

            Assert.IsTrue(result.Status);

            // Assert each payload key and data
            Assert.AreEqual(1, payload.Key.Count);
            Assert.AreEqual(9, payload.Data.Count);

            Assert.AreEqual("TransactionID", payload.Key[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Key[0].Value));

            Assert.AreEqual("TransactionID", payload.Data[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[0].Value));

            Assert.AreEqual("DateRequest", payload.Data[1].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[1].Value));

            Assert.AreEqual("PortID", payload.Data[2].Column);
            Assert.AreEqual("32", payload.Data[2].Value);

            Assert.AreEqual("Task", payload.Data[3].Column);
            Assert.AreEqual("GetData", payload.Data[3].Value);

            Assert.AreEqual("Input", payload.Data[4].Column);
            Assert.AreEqual("AAkAdABoAGUAdABhACAAcwBhAG0AAAADAEQcRgAAAAAAAAAAAAAAAAAAAAAATBxGAAAAAAAAAAAAAAAAAAAAAAB" +
                "QHEYAAAAAAAAAAAAAAAAAAAAA", payload.Data[4].Value);

            Assert.AreEqual("NodeID", payload.Data[5].Column);
            Assert.AreEqual("theta sam", payload.Data[5].Value);

            Assert.AreEqual("Priority", payload.Data[6].Column);
            Assert.AreEqual("5", payload.Data[6].Value);

            Assert.AreEqual("Source", payload.Data[7].Column);
            Assert.AreEqual("username", payload.Data[7].Value);
        }

        [TestMethod]
        public void CreateReadRegisterPayloadWhenNodeIdIsEmptyReturnsFalseTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();
            var addresses = new[] { "10001", "10003", "10004" };

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns(String.Empty);

            var result = service.CreateReadRegisterPayload(assetId, addresses, correlationId, out var payload);

            Assert.IsFalse(result.Status);
        }

        [TestMethod]
        public void CreateReadRegisterPayloadWhenNoPortIdReturnsFalseTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();
            var addresses = new[] { "10001", "10003", "10004" };

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = default;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(false);

            var result = service.CreateReadRegisterPayload(assetId, addresses, correlationId, out var payload);

            Assert.IsFalse(result.Status);
        }

        #endregion

        #region WriteRegisterPayload Tests

        [TestMethod]
        public void CreateWriteRegisterPayloadTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();
            var addressValues = new Dictionary<string, string>()
            {
                { "10046", "1" },
                { "42248", "31" },
                { "42612", "143.5" },
            };

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockParameterDataType
                .Setup(x => x.GetParametersDataTypes(assetId, new List<int>() { 10046, 42248, 42612 }, It.IsAny<string>()))
                    .Returns(new Dictionary<int, short?>()
                    {
                        { 10046, 1},
                        { 42248, 2},
                        { 42612, 4},
                    });

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = 32;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(true);

            var result = service.CreateWriteRegisterPayload(assetId, addressValues, correlationId, out var payload);

            Assert.IsTrue(result.Status);

            // Assert each payload key and data
            Assert.AreEqual(1, payload.Key.Count);
            Assert.AreEqual(9, payload.Data.Count);

            Assert.AreEqual("TransactionID", payload.Key[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Key[0].Value));

            Assert.AreEqual("TransactionID", payload.Data[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[0].Value));

            Assert.AreEqual("DateRequest", payload.Data[1].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[1].Value));

            Assert.AreEqual("PortID", payload.Data[2].Column);
            Assert.AreEqual("32", payload.Data[2].Value);

            Assert.AreEqual("Task", payload.Data[3].Column);
            Assert.AreEqual("GetData", payload.Data[3].Value);

            Assert.AreEqual("Input", payload.Data[4].Column);
            Assert.AreEqual("AAkAdABoAGUAdABhACAAcwBhAG0AAQADAPgcRgAAgD8AAIA/AAAAAAAAAAAACCVHAAAAQAAA+EEAAAAAAAAAAAB" +
                "0JkcAAIBAAIAPQwAAAAAAAAAA", payload.Data[4].Value);

            Assert.AreEqual("NodeID", payload.Data[5].Column);
            Assert.AreEqual("theta sam", payload.Data[5].Value);

            Assert.AreEqual("Priority", payload.Data[6].Column);
            Assert.AreEqual("5", payload.Data[6].Value);

            Assert.AreEqual("Source", payload.Data[7].Column);
            Assert.AreEqual("username", payload.Data[7].Value);
        }

        [TestMethod]
        public void CreateWriteRegisterPayloadWhenNodeIdIsEmptyReturnsFalseTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();
            var addressValues = new Dictionary<string, string>()
            {
                { "10046", "1" },
                { "42248", "31" },
                { "42612", "143.5" },
            };

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns(String.Empty);

            var result = service.CreateWriteRegisterPayload(assetId, addressValues, correlationId, out var payload);

            Assert.IsFalse(result.Status);
        }

        [TestMethod]
        public void CreateWriteRegisterPayloadWhenNoPortIdReturnsFalseTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();
            var addressValues = new Dictionary<string, string>()
            {
                { "10046", "1" },
                { "42248", "31" },
                { "42612", "143.5" },
            };

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockParameterDataType
                .Setup(x => x.GetParametersDataTypes(assetId, new List<int>() { 10046, 42248, 42612 }, It.IsAny<string>()))
                    .Returns(new Dictionary<int, short?>()
                    {
                        { 10046, 1},
                        { 42248, 2},
                        { 42612, 4},
                    });

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = default;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(false);

            var result = service.CreateWriteRegisterPayload(assetId, addressValues, correlationId, out var payload);

            Assert.IsFalse(result.Status);
        }

        #endregion

        #region WellControlPayload Tests

        [TestMethod]
        public void CreateWellControlPayloadStartWellTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = 32;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(true);

            short pocTypeId = 8;
            mockNodeMaster.Setup(x => x.TryGetPocTypeIdByAssetGUID(assetId, out pocTypeId, It.IsAny<string>()))
                .Returns(true);

            var controlType = DeviceControlType.StartWell;

            var result = service.CreateWellControlPayload(out var payload, assetId, controlType, correlationId);

            Assert.IsTrue(result.Status);

            // Assert each payload key and data
            Assert.AreEqual(1, payload.Key.Count);
            Assert.AreEqual(9, payload.Data.Count);

            Assert.AreEqual("TransactionID", payload.Key[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Key[0].Value));

            Assert.AreEqual("TransactionID", payload.Data[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[0].Value));

            Assert.AreEqual("DateRequest", payload.Data[1].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[1].Value));

            Assert.AreEqual("PortID", payload.Data[2].Column);
            Assert.AreEqual("32", payload.Data[2].Value);

            Assert.AreEqual("Task", payload.Data[3].Column);
            Assert.AreEqual("WellControl", payload.Data[3].Value);

            Assert.AreEqual("Input", payload.Data[4].Column);
            Assert.AreEqual("AAkAdABoAGUAdABhACAAcwBhAG0AAQAIAAc=", payload.Data[4].Value);

            Assert.AreEqual("NodeID", payload.Data[5].Column);
            Assert.AreEqual("theta sam", payload.Data[5].Value);

            Assert.AreEqual("Priority", payload.Data[6].Column);
            Assert.AreEqual("5", payload.Data[6].Value);

            Assert.AreEqual("Source", payload.Data[7].Column);
            Assert.AreEqual("username", payload.Data[7].Value);
        }

        [TestMethod]
        public void CreateWellControlPayloadStopWellTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = 32;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(true);

            short pocTypeId = 8;
            mockNodeMaster.Setup(x => x.TryGetPocTypeIdByAssetGUID(assetId, out pocTypeId, It.IsAny<string>()))
                .Returns(true);

            var controlType = DeviceControlType.StopWell;

            var result = service.CreateWellControlPayload(out var payload, assetId, controlType, correlationId);

            Assert.IsTrue(result.Status);

            // Assert each payload key and data
            Assert.AreEqual(1, payload.Key.Count);
            Assert.AreEqual(9, payload.Data.Count);

            Assert.AreEqual("TransactionID", payload.Key[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Key[0].Value));

            Assert.AreEqual("TransactionID", payload.Data[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[0].Value));

            Assert.AreEqual("DateRequest", payload.Data[1].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[1].Value));

            Assert.AreEqual("PortID", payload.Data[2].Column);
            Assert.AreEqual("32", payload.Data[2].Value);

            Assert.AreEqual("Task", payload.Data[3].Column);
            Assert.AreEqual("WellControl", payload.Data[3].Value);

            Assert.AreEqual("Input", payload.Data[4].Column);
            Assert.AreEqual("AAkAdABoAGUAdABhACAAcwBhAG0AAgAIAAc=", payload.Data[4].Value);

            Assert.AreEqual("NodeID", payload.Data[5].Column);
            Assert.AreEqual("theta sam", payload.Data[5].Value);

            Assert.AreEqual("Priority", payload.Data[6].Column);
            Assert.AreEqual("5", payload.Data[6].Value);

            Assert.AreEqual("Source", payload.Data[7].Column);
            Assert.AreEqual("username", payload.Data[7].Value);
        }

        [TestMethod]
        public void CreateWellControlPayloadIdleWellTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = 32;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(true);

            short pocTypeId = 8;
            mockNodeMaster.Setup(x => x.TryGetPocTypeIdByAssetGUID(assetId, out pocTypeId, It.IsAny<string>()))
                .Returns(true);

            var controlType = DeviceControlType.IdleWell;

            var result = service.CreateWellControlPayload(out var payload, assetId, controlType, correlationId);

            Assert.IsTrue(result.Status);

            // Assert each payload key and data
            Assert.AreEqual(1, payload.Key.Count);
            Assert.AreEqual(9, payload.Data.Count);

            Assert.AreEqual("TransactionID", payload.Key[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Key[0].Value));

            Assert.AreEqual("TransactionID", payload.Data[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[0].Value));

            Assert.AreEqual("DateRequest", payload.Data[1].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[1].Value));

            Assert.AreEqual("PortID", payload.Data[2].Column);
            Assert.AreEqual("32", payload.Data[2].Value);

            Assert.AreEqual("Task", payload.Data[3].Column);
            Assert.AreEqual("WellControl", payload.Data[3].Value);

            Assert.AreEqual("Input", payload.Data[4].Column);
            Assert.AreEqual("AAkAdABoAGUAdABhACAAcwBhAG0AAwAIAAc=", payload.Data[4].Value);

            Assert.AreEqual("NodeID", payload.Data[5].Column);
            Assert.AreEqual("theta sam", payload.Data[5].Value);

            Assert.AreEqual("Priority", payload.Data[6].Column);
            Assert.AreEqual("5", payload.Data[6].Value);

            Assert.AreEqual("Source", payload.Data[7].Column);
            Assert.AreEqual("username", payload.Data[7].Value);
        }

        [TestMethod]
        public void CreateWellControlPayloadClearAlarmsTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = 32;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(true);

            short pocTypeId = 8;
            mockNodeMaster.Setup(x => x.TryGetPocTypeIdByAssetGUID(assetId, out pocTypeId, It.IsAny<string>()))
                .Returns(true);

            var controlType = DeviceControlType.ClearAlarms;

            var result = service.CreateWellControlPayload(out var payload, assetId, controlType, correlationId);

            Assert.IsTrue(result.Status);

            // Assert each payload key and data
            Assert.AreEqual(1, payload.Key.Count);
            Assert.AreEqual(9, payload.Data.Count);

            Assert.AreEqual("TransactionID", payload.Key[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Key[0].Value));

            Assert.AreEqual("TransactionID", payload.Data[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[0].Value));

            Assert.AreEqual("DateRequest", payload.Data[1].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[1].Value));

            Assert.AreEqual("PortID", payload.Data[2].Column);
            Assert.AreEqual("32", payload.Data[2].Value);

            Assert.AreEqual("Task", payload.Data[3].Column);
            Assert.AreEqual("WellControl", payload.Data[3].Value);

            Assert.AreEqual("Input", payload.Data[4].Column);
            Assert.AreEqual("AAkAdABoAGUAdABhACAAcwBhAG0ABAAIAAc=", payload.Data[4].Value);

            Assert.AreEqual("NodeID", payload.Data[5].Column);
            Assert.AreEqual("theta sam", payload.Data[5].Value);

            Assert.AreEqual("Priority", payload.Data[6].Column);
            Assert.AreEqual("5", payload.Data[6].Value);

            Assert.AreEqual("Source", payload.Data[7].Column);
            Assert.AreEqual("username", payload.Data[7].Value);
        }

        [TestMethod]
        public void CreateWellControlPayloadConstantRunModeTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = 32;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(true);

            short pocTypeId = 8;
            mockNodeMaster.Setup(x => x.TryGetPocTypeIdByAssetGUID(assetId, out pocTypeId, It.IsAny<string>()))
                .Returns(true);

            var controlType = DeviceControlType.ConstantRunMode;

            var result = service.CreateWellControlPayload(out var payload, assetId, controlType, correlationId);

            Assert.IsTrue(result.Status);

            // Assert each payload key and data
            Assert.AreEqual(1, payload.Key.Count);
            Assert.AreEqual(9, payload.Data.Count);

            Assert.AreEqual("TransactionID", payload.Key[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Key[0].Value));

            Assert.AreEqual("TransactionID", payload.Data[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[0].Value));

            Assert.AreEqual("DateRequest", payload.Data[1].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[1].Value));

            Assert.AreEqual("PortID", payload.Data[2].Column);
            Assert.AreEqual("32", payload.Data[2].Value);

            Assert.AreEqual("Task", payload.Data[3].Column);
            Assert.AreEqual("WellControl", payload.Data[3].Value);

            Assert.AreEqual("Input", payload.Data[4].Column);
            Assert.AreEqual("AAkAdABoAGUAdABhACAAcwBhAG0ABQAIAAc=", payload.Data[4].Value);

            Assert.AreEqual("NodeID", payload.Data[5].Column);
            Assert.AreEqual("theta sam", payload.Data[5].Value);

            Assert.AreEqual("Priority", payload.Data[6].Column);
            Assert.AreEqual("5", payload.Data[6].Value);

            Assert.AreEqual("Source", payload.Data[7].Column);
            Assert.AreEqual("username", payload.Data[7].Value);
        }

        [TestMethod]
        public void CreateWellControlPayloadPocModeTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = 32;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(true);

            short pocTypeId = 8;
            mockNodeMaster.Setup(x => x.TryGetPocTypeIdByAssetGUID(assetId, out pocTypeId, It.IsAny<string>()))
                .Returns(true);

            var controlType = DeviceControlType.PocMode;

            var result = service.CreateWellControlPayload(out var payload, assetId, controlType, correlationId);

            Assert.IsTrue(result.Status);

            // Assert each payload key and data
            Assert.AreEqual(1, payload.Key.Count);
            Assert.AreEqual(9, payload.Data.Count);

            Assert.AreEqual("TransactionID", payload.Key[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Key[0].Value));

            Assert.AreEqual("TransactionID", payload.Data[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[0].Value));

            Assert.AreEqual("DateRequest", payload.Data[1].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[1].Value));

            Assert.AreEqual("PortID", payload.Data[2].Column);
            Assert.AreEqual("32", payload.Data[2].Value);

            Assert.AreEqual("Task", payload.Data[3].Column);
            Assert.AreEqual("WellControl", payload.Data[3].Value);

            Assert.AreEqual("Input", payload.Data[4].Column);
            Assert.AreEqual("AAkAdABoAGUAdABhACAAcwBhAG0ABgAIAAc=", payload.Data[4].Value);

            Assert.AreEqual("NodeID", payload.Data[5].Column);
            Assert.AreEqual("theta sam", payload.Data[5].Value);

            Assert.AreEqual("Priority", payload.Data[6].Column);
            Assert.AreEqual("5", payload.Data[6].Value);

            Assert.AreEqual("Source", payload.Data[7].Column);
            Assert.AreEqual("username", payload.Data[7].Value);
        }

        [TestMethod]
        public void CreateWellControlPayloadPercentTimerModeTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = 32;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(true);

            short pocTypeId = 8;
            mockNodeMaster.Setup(x => x.TryGetPocTypeIdByAssetGUID(assetId, out pocTypeId, It.IsAny<string>()))
                .Returns(true);

            var controlType = DeviceControlType.PercentTimerMode;

            var result = service.CreateWellControlPayload(out var payload, assetId, controlType, correlationId);

            Assert.IsTrue(result.Status);

            // Assert each payload key and data
            Assert.AreEqual(1, payload.Key.Count);
            Assert.AreEqual(9, payload.Data.Count);

            Assert.AreEqual("TransactionID", payload.Key[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Key[0].Value));

            Assert.AreEqual("TransactionID", payload.Data[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[0].Value));

            Assert.AreEqual("DateRequest", payload.Data[1].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[1].Value));

            Assert.AreEqual("PortID", payload.Data[2].Column);
            Assert.AreEqual("32", payload.Data[2].Value);

            Assert.AreEqual("Task", payload.Data[3].Column);
            Assert.AreEqual("WellControl", payload.Data[3].Value);

            Assert.AreEqual("Input", payload.Data[4].Column);
            Assert.AreEqual("AAkAdABoAGUAdABhACAAcwBhAG0ABwAIAAc=", payload.Data[4].Value);

            Assert.AreEqual("NodeID", payload.Data[5].Column);
            Assert.AreEqual("theta sam", payload.Data[5].Value);

            Assert.AreEqual("Priority", payload.Data[6].Column);
            Assert.AreEqual("5", payload.Data[6].Value);

            Assert.AreEqual("Source", payload.Data[7].Column);
            Assert.AreEqual("username", payload.Data[7].Value);
        }

        [TestMethod]
        public void CreateWellControlPayloadScanTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = 32;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(true);

            short pocTypeId = 8;
            mockNodeMaster.Setup(x => x.TryGetPocTypeIdByAssetGUID(assetId, out pocTypeId, It.IsAny<string>()))
                .Returns(true);

            var controlType = DeviceControlType.Scan;

            var result = service.CreateWellControlPayload(out var payload, assetId, controlType, correlationId);

            Assert.IsTrue(result.Status);

            // Assert each payload key and data
            Assert.AreEqual(1, payload.Key.Count);
            Assert.AreEqual(9, payload.Data.Count);

            Assert.AreEqual("TransactionID", payload.Key[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Key[0].Value));

            Assert.AreEqual("TransactionID", payload.Data[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[0].Value));

            Assert.AreEqual("DateRequest", payload.Data[1].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[1].Value));

            Assert.AreEqual("PortID", payload.Data[2].Column);
            Assert.AreEqual("32", payload.Data[2].Value);

            Assert.AreEqual("Task", payload.Data[3].Column);
            Assert.AreEqual("WellControl", payload.Data[3].Value);

            Assert.AreEqual("Input", payload.Data[4].Column);
            Assert.AreEqual("AAkAdABoAGUAdABhACAAcwBhAG0ACAAIAAc=", payload.Data[4].Value);

            Assert.AreEqual("NodeID", payload.Data[5].Column);
            Assert.AreEqual("theta sam", payload.Data[5].Value);

            Assert.AreEqual("Priority", payload.Data[6].Column);
            Assert.AreEqual("5", payload.Data[6].Value);

            Assert.AreEqual("Source", payload.Data[7].Column);
            Assert.AreEqual("username", payload.Data[7].Value);
        }

        [TestMethod]
        public void CreateWellControlPayloadSetClockTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var service = CreateService(
                out var mockNodeMaster, out var mockTransaction, out var mockParameterDataType);

            mockTransaction.Setup(x => x.TransactionIdExists(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            mockNodeMaster.Setup(x => x.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("theta sam");

            short portId = 32;
            mockNodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>()))
                .Returns(true);

            short pocTypeId = 8;
            mockNodeMaster.Setup(x => x.TryGetPocTypeIdByAssetGUID(assetId, out pocTypeId, It.IsAny<string>()))
                .Returns(true);

            var controlType = DeviceControlType.SetClock;

            var result = service.CreateWellControlPayload(out var payload, assetId, controlType, correlationId);

            Assert.IsTrue(result.Status);

            // Assert each payload key and data
            Assert.AreEqual(1, payload.Key.Count);
            Assert.AreEqual(9, payload.Data.Count);

            Assert.AreEqual("TransactionID", payload.Key[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Key[0].Value));

            Assert.AreEqual("TransactionID", payload.Data[0].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[0].Value));

            Assert.AreEqual("DateRequest", payload.Data[1].Column);
            Assert.IsFalse(string.IsNullOrEmpty(payload.Data[1].Value));

            Assert.AreEqual("PortID", payload.Data[2].Column);
            Assert.AreEqual("32", payload.Data[2].Value);

            Assert.AreEqual("Task", payload.Data[3].Column);
            Assert.AreEqual("WellControl", payload.Data[3].Value);

            Assert.AreEqual("Input", payload.Data[4].Column);
            Assert.AreEqual("AAkAdABoAGUAdABhACAAcwBhAG0ACQAIAAc=", payload.Data[4].Value);

            Assert.AreEqual("NodeID", payload.Data[5].Column);
            Assert.AreEqual("theta sam", payload.Data[5].Value);

            Assert.AreEqual("Priority", payload.Data[6].Column);
            Assert.AreEqual("5", payload.Data[6].Value);

            Assert.AreEqual("Source", payload.Data[7].Column);
            Assert.AreEqual("username", payload.Data[7].Value);
        }

        #endregion

        #endregion

        #region Private Methods

        private TransactionPayloadCreator CreateService(
            out Mock<INodeMaster> nodeMaster,
            out Mock<ITransaction> transaction,
            out Mock<IParameterDataType> parameterDataType)
        {
            nodeMaster = new Mock<INodeMaster>();
            transaction = new Mock<ITransaction>();
            parameterDataType = new Mock<IParameterDataType>();

            return new TransactionPayloadCreator(nodeMaster.Object, transaction.Object, parameterDataType.Object);
        }

        #endregion

    }
}
