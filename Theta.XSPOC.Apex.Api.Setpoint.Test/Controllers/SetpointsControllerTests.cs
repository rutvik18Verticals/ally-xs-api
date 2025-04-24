using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common.Communications;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Setpoint.Contracts.Requests;
using Theta.XSPOC.Apex.Api.Setpoint.Controllers;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Setpoint.Test.Controllers
{
    [TestClass]
    public class SetpointsControllerTests
    {

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();

            SetupThetaLoggerFactory();
        }

        #region Test Methods

        [TestMethod]
        public void GetSetPointsTest()
        {
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();

            var controller = new SetpointsController(transactionPayloadCreatorMock.Object, _loggerFactory.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "addresses", "'1', '2', '3', '4'"
                }
            };

            var result = controller.Get(filter);

            Assert.IsNotNull(result);
            Assert.AreEqual(202, ((AcceptedResult)result).StatusCode);

            filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "addresses", "1"
                }
            };

            result = controller.Get(filter);

            Assert.IsNotNull(result);
            Assert.AreEqual(202, ((AcceptedResult)result).StatusCode);
        }

        [TestMethod]
        public void GetSetPointsInvalidFiltersTest()
        {
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();
            var controller = new SetpointsController(transactionPayloadCreatorMock.Object, _loggerFactory.Object);

            var asset = new Dictionary<string, string>
            {
                {
                    "assetIds", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                }
            };

            var result = controller.Get(asset);

            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((BadRequestResult)result).StatusCode);
        }

        [TestMethod]
        public void GetSetPointsByFilterNullTest()
        {
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();
            var controller = new SetpointsController(transactionPayloadCreatorMock.Object, _loggerFactory.Object);

            IDictionary<string, string> asset = null;

            var result = controller.Get(asset);

            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((BadRequestResult)result).StatusCode);
        }

        [TestMethod]
        public void UpdateSetpointsByAssetIdTest()
        {
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();
            var controller = new SetpointsController(transactionPayloadCreatorMock.Object, _loggerFactory.Object);

            var assetId = "DFC1D0AD-A824-4965-B78D-AB7755E32DD3";

            var addressValues = new Dictionary<string, string>
            {
                {
                    "addressValue1", "39748"
                },
                {
                    "addressValue2", "39749"
                },

            };
            PutRequest request = new PutRequest
            {
                AssetId = assetId,
                AddressValues = addressValues
            };

            var result = controller.Put(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(202, ((AcceptedResult)result).StatusCode);
        }

        [TestMethod]
        public void UpdateSetPointsByFilterNullTest()
        {
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();
            var controller = new SetpointsController(transactionPayloadCreatorMock.Object, _loggerFactory.Object);

            var assetId = "DFC1D0AD-A824-4965-B78D-AB7755E32DD3";

            PutRequest request = new PutRequest
            {
                AssetId = assetId,
                AddressValues = null
            };

            var result = controller.Put(request);

            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void UpdateSetPointsByAssetIdNullTest()
        {
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();
            var controller = new SetpointsController(transactionPayloadCreatorMock.Object, _loggerFactory.Object);

            var assetId = string.Empty;

            var addressValues = new Dictionary<string, string>
            {
                {
                    "addressValue1", "39748"
                },
                {
                    "addressValue2", "39749"
                },

            };
            PutRequest request = new PutRequest
            {
                AssetId = assetId,
                AddressValues = addressValues
            };

            var result = controller.Put(request);

            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
        }

        #endregion

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

    }
}
