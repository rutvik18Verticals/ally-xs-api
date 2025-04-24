using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.Communications;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Requests;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Responses;
using Theta.XSPOC.Apex.Api.WellControl.Controllers;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Api.WellControl.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.WellControl.Test.Controllers
{
    [TestClass]
    public class SetpointsControllerTests
    {

        #region Private Fields

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;

        #endregion

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
            var setpointGroupProcessingServiceMock = new Mock<ISetpointGroupProcessingService>();
            var processingServiceMock = new Mock<IProcessingDataUpdatesService>();

            var controller = new SetpointsController(setpointGroupProcessingServiceMock.Object,
                processingServiceMock.Object, _loggerFactory.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "addresses", "'1', '2', '3', '4'"
                },
                {
                    "socketId", "ADA1D0AD-A824-4965-B78D-AB7755E32DD3"
                }
            };
            Guid assetId = Guid.Parse("DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var addresses = ("'1', '2', '3', '4'").Split(',');
            string socketId = "ADA1D0AD-A824-4965-B78D-AB7755E32DD3";

            processingServiceMock.Setup(a => a.SendReadRegisterTransaction(assetId, addresses, socketId))
                .Returns(Task.FromResult(new MethodResult<string>(true, string.Empty)));

            var result = controller.Get(filter);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(AcceptedResult));

            filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "addresses", "1"
                },
                {
                    "socketId", "ADA1D0AD-A824-4965-B78D-AB7755E32DD3"
                }
            };
            addresses = ("1").Split(',');

            processingServiceMock.Setup(a => a.SendReadRegisterTransaction(assetId, addresses, socketId))
                .Returns(Task.FromResult(new MethodResult<string>(true, string.Empty)));

            result = controller.Get(filter);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(AcceptedResult));
        }

        [TestMethod]
        public void GetSetPointsInvalidFiltersTest()
        {
            var setpointGroupProcessingServiceMock = new Mock<ISetpointGroupProcessingService>();
            var processingServiceMock = new Mock<IProcessingDataUpdatesService>();

            var controller = new SetpointsController(
                setpointGroupProcessingServiceMock.Object, processingServiceMock.Object, _loggerFactory.Object);

            var asset = new Dictionary<string, string>
            {
                {
                    "assetIds", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                }
            };

            var result = controller.Get(asset);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetSetPointsByFilterNullTest()
        {
            var setpointGroupProcessingServiceMock = new Mock<ISetpointGroupProcessingService>();
            var processingServiceMock = new Mock<IProcessingDataUpdatesService>();

            var controller = new SetpointsController(setpointGroupProcessingServiceMock.Object,
                processingServiceMock.Object, _loggerFactory.Object);

            IDictionary<string, string> asset = null;

            var result = controller.Get(asset);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateSetpointsByAssetIdTest()
        {
            var setpointGroupProcessingServiceMock = new Mock<ISetpointGroupProcessingService>();
            var processingServiceMock = new Mock<IProcessingDataUpdatesService>();

            var controller = new SetpointsController(setpointGroupProcessingServiceMock.Object,
                processingServiceMock.Object, _loggerFactory.Object);

            var assetId = "DFC1D0AD-A824-4965-B78D-AB7755E32DD3";

            var addressValues = new Dictionary<string, string>
            {
                {
                    "39748", "1"
                },
                {
                    "39749", "`0"
                },
            };
            string socketId = "ADA1D0AD-A824-4965-B78D-AB7755E32DD3";

            PutRequest request = new PutRequest
            {
                AssetId = assetId,
                AddressValues = addressValues,
                SocketId = socketId
            };

            processingServiceMock.Setup(a => a.SendWriteRegisterTransaction(Guid.Parse(assetId), request.AddressValues, socketId))
                .Returns(Task.FromResult(new MethodResult<string>(true, string.Empty)));

            var result = controller.Put(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(AcceptedResult));
        }

        [TestMethod]
        public void UpdateSetPointsByFilterNullTest()
        {
            var setpointGroupProcessingServiceMock = new Mock<ISetpointGroupProcessingService>();
            var processingServiceMock = new Mock<IProcessingDataUpdatesService>();

            var controller = new SetpointsController(setpointGroupProcessingServiceMock.Object,
                processingServiceMock.Object, _loggerFactory.Object);

            var assetId = "DFC1D0AD-A824-4965-B78D-AB7755E32DD3";

            PutRequest request = new PutRequest
            {
                AssetId = assetId,
                AddressValues = null
            };

            var result = controller.Put(request);

            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
        }

        [TestMethod]
        public void UpdateSetPointsByAssetIdNullTest()
        {
            var setpointGroupProcessingServiceMock = new Mock<ISetpointGroupProcessingService>();
            var processingServiceMock = new Mock<IProcessingDataUpdatesService>();

            var controller = new SetpointsController(setpointGroupProcessingServiceMock.Object,
                processingServiceMock.Object, _loggerFactory.Object);

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

            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
        }

        [TestMethod]
        public void GetMockJsonTest()
        {
            var setpointGroupProcessingServiceMock = new Mock<ISetpointGroupProcessingService>();
            var processingServiceMock = new Mock<IProcessingDataUpdatesService>();

            var controller = new SetpointsController(
                setpointGroupProcessingServiceMock.Object, processingServiceMock.Object, _loggerFactory.Object);

            var result = controller.GetMockJson();

            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);

            Assert.IsNotNull(((OkObjectResult)result).Value);

            Assert.IsTrue(((MockSetpointGroup)((OkObjectResult)result).Value).SetpointGroups.Count > 0);
        }

        [TestMethod]
        public void GetSetpointGroupsTest()
        {
            var mockService = new Mock<ISetpointGroupProcessingService>();
            var processingServiceMock = new Mock<IProcessingDataUpdatesService>();

            var setpointGroupData = new List<SetPointGroupsData>()
            {
                new SetPointGroupsData()
                {
                    SetpointGroup = 1,
                    RegisterCount = 1,
                    SetpointGroupName = "POC Control",
                    Setpoints = new List<SetpointData>()
                    {
                        new SetpointData()
                        {
                            Parameter = "42189",
                            Description = "SWT K Factor",
                            BackupDate = DateTime.Now,
                            BackupValue = "1",
                            IsSupported = true,
                            BackUpLookUpValues = null
                        }
                    }
                }
            };

            mockService.Setup(x => x.GetSetpointGroups(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new SetpointsGroupOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = setpointGroupData
                });

            var controller = new SetpointsController(mockService.Object, processingServiceMock.Object,
                _loggerFactory.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                }
            };

            var result = controller.GetSetpointGroups(filter);

            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);

            Assert.IsNotNull(((OkObjectResult)result).Value);

            Assert.IsTrue(((SetpointsGroupDataResponse)((OkObjectResult)result).Value).Values.Count > 0);
        }

        [TestMethod]
        public void GetSetpointGroupsInvalidAssetIdTest()
        {
            var transactionPayloadCreatorMock = new Mock<ITransactionPayloadCreator>();
            var mockService = new Mock<ISetpointGroupProcessingService>();
            var processingServiceMock = new Mock<IProcessingDataUpdatesService>();

            var setpointGroupData = new List<SetPointGroupsData>()
            {
                new SetPointGroupsData()
                {
                    SetpointGroup = 1,
                    RegisterCount = 1,
                    SetpointGroupName = "POC Control",
                    Setpoints = new List<SetpointData>()
                    {
                        new SetpointData()
                        {
                            Parameter = "42189",
                            Description = "SWT K Factor",
                            BackupDate = DateTime.Now,
                            BackupValue = "1",
                            IsSupported = true,
                            BackUpLookUpValues = null
                        }
                    }
                }
            };

            mockService.Setup(x => x.GetSetpointGroups(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new SetpointsGroupOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = setpointGroupData
                });

            var controller = new SetpointsController(mockService.Object, processingServiceMock.Object,
                _loggerFactory.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "assetId", Guid.Empty.ToString()
                }
            };

            var result = controller.GetSetpointGroups(filter);

            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetSetpointGroupsNullResponseTest()
        {
            var mockService = new Mock<ISetpointGroupProcessingService>();
            var processingServiceMock = new Mock<IProcessingDataUpdatesService>();

            var controller = new SetpointsController(mockService.Object, processingServiceMock.Object,
                _loggerFactory.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                }
            };

            var result = controller.GetSetpointGroups(filter);

            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetSetpointGroupsNullBackUpDateTest()
        {
            var mockService = new Mock<ISetpointGroupProcessingService>();
            var processingServiceMock = new Mock<IProcessingDataUpdatesService>();

            var setpointGroupData = new List<SetPointGroupsData>()
            {
                new SetPointGroupsData()
                {
                    SetpointGroup = 1,
                    RegisterCount = 1,
                    SetpointGroupName = "POC Control",
                    Setpoints = new List<SetpointData>()
                    {
                        new SetpointData()
                        {
                            Parameter = "42189",
                            Description = "SWT K Factor",
                            BackupDate = null,
                            BackupValue = "5",
                            IsSupported = true,
                            BackUpLookUpValues = null
                        }
                    }
                }
            };

            mockService.Setup(x => x.GetSetpointGroups(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new SetpointsGroupOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = setpointGroupData
                });

            var controller = new SetpointsController(mockService.Object, processingServiceMock.Object,
                _loggerFactory.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                }
            };

            var result = controller.GetSetpointGroups(filter);

            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);

            Assert.IsNotNull(((OkObjectResult)result).Value);

            Assert.IsTrue(((SetpointsGroupDataResponse)((OkObjectResult)result).Value).Values.Count > 0);
        }

        #endregion

        #region Private Methods

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        #endregion

    }
}
