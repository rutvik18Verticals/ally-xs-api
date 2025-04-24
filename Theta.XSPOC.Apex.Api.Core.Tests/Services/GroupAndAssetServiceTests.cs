using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class GroupAndAssetServiceTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullUserDefaultServiceTest()
        {
            var mockService = new Mock<IGroupAndAsset>();
            var mockThetaLoggingFactory = new Mock<IThetaLoggerFactory>();
            var mockNodeMasterService = new Mock<INodeMaster>();

            _ = new GroupAndAssetService(mockService.Object, null, mockThetaLoggingFactory.Object,
                mockNodeMasterService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerServiceTest()
        {
            var mockService = new Mock<IGroupAndAsset>();
            var mockUserDefaultStore = new Mock<IUserDefaultStore>();
            var mockNodeMasterService = new Mock<INodeMaster>();

            _ = new GroupAndAssetService(mockService.Object, mockUserDefaultStore.Object, null,
                mockNodeMasterService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullGroupAndAssetServiceTest()
        {
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var mockUserDefaultStore = new Mock<IUserDefaultStore>();
            var mockNodeMasterService = new Mock<INodeMaster>();

            _ = new GroupAndAssetService(null, mockUserDefaultStore.Object, mockThetaLoggerFactory.Object,
                mockNodeMasterService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullNodeMasterServiceTest()
        {
            var mockService = new Mock<IGroupAndAsset>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var mockUserDefaultStore = new Mock<IUserDefaultStore>();

            _ = new GroupAndAssetService(mockService.Object, mockUserDefaultStore.Object, mockThetaLoggerFactory.Object,
                null);
        }

        [TestMethod]
        public void GetGroupAndAssetTest()
        {
            var userId = "TestUser";
            var group = "TestGroup";
            var property = "TestProperty";
            var correlationId = new Guid(Guid.NewGuid().ToString()).ToString();
            var isNewArchtecture = false;

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockNodeMasterService = new Mock<INodeMaster>();

            var mockService = new Mock<IGroupAndAsset>();
            mockService.Setup(m => m.GetGroupAssetAndRelationshipData(correlationId, "")).Returns(new GroupAndAssetModel());

            var mockUserDefaultStore = new Mock<IUserDefaultStore>();
            mockUserDefaultStore.Setup(m => m.GetItem(userId, property, group, correlationId)).Returns("A Value");
            var service = new GroupAndAssetService(mockService.Object, mockUserDefaultStore.Object,
                mockThetaLoggerFactory.Object, mockNodeMasterService.Object);

            var result = service.GetGroupAndAssetData(userId, correlationId, isNewArchtecture);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GroupAndAssetDataOutput));
            mockService.Verify(x => x.GetGroupAssetAndRelationshipData(correlationId, ""), Times.Once);
        }

        [TestMethod]
        public void GetGroupAndAssetWithNewArhtectureTest()
        {
            var userId = "TestUser";
            var group = "TestGroup";
            var property = "TestProperty";
            var correlationId = new Guid(Guid.NewGuid().ToString()).ToString();
            var isNewArchtecture = true;

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockNodeMasterService = new Mock<INodeMaster>();
            mockNodeMasterService.Setup(m => m.GetNewArchitectureWells(correlationId))
                .Returns(new List<Guid>());

            var mockService = new Mock<IGroupAndAsset>();
            mockService.Setup(m => m.GetGroupAssetAndRelationshipData(correlationId, "")).Returns(new GroupAndAssetModel());

            var mockUserDefaultStore = new Mock<IUserDefaultStore>();
            mockUserDefaultStore.Setup(m => m.GetItem(userId, property, group, correlationId)).Returns("A Value");
            var service = new GroupAndAssetService(mockService.Object, mockUserDefaultStore.Object,
                mockThetaLoggerFactory.Object, mockNodeMasterService.Object);

            var result = service.GetGroupAndAssetData(userId, correlationId, isNewArchtecture);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GroupAndAssetDataOutput));
            mockService.Verify(x => x.GetGroupAssetAndRelationshipData(correlationId, ""), Times.Once);
        }

        #endregion

    }
}
