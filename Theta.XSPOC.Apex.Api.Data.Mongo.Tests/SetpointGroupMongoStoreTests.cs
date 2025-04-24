using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using MongoAsset = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using MongoLookup = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class SetpointGroupMongoStoreTests
    {
        private Mock<IMongoDatabase> _mockDatabase;
        private Mock<IThetaLoggerFactory> _mockLoggerFactory;
        private Mock<IDateTimeConverter> _mockDateTimeConverter;
        private SetpointGroupMongoStore _store;

        [TestInitialize]
        public void Setup()
        {
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            _mockDateTimeConverter = new Mock<IDateTimeConverter>();

            var logger = new Mock<IThetaLogger>();
            _mockLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            _store = new SetpointGroupMongoStore(_mockDatabase.Object, _mockLoggerFactory.Object, _mockDateTimeConverter.Object);
        }

        // Scenario : Valid AssetId passed and all corresponding data is present. 
        [TestMethod]
        public void GetSetPointGroupData_ReturnsCorrectData()
        {
            // Arrange
            var assetId = Guid.Parse("FE457B4C-C3F7-40DE-9708-E2F36E78DAFE");
            var correlationId = "test-correlation";

            var node = new MongoAsset.Asset
            {
                Id = "FE45784C-C3F7-40DE-9708-E2F36E78DAFE",
                LegacyId = new Dictionary<string, string> { { "AssetGUID", assetId.ToString() } },
                AssetConfig = new MongoAsset.AssetConfig { FirmwareVersion = 1.2f },
                POCType = new Lookup
                {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCTypesId", "1" },
                    },
                    LookupType = LookupTypes.POCTypes.ToString(),
                    LookupDocument = new MongoLookup.POCTypes()
                    {
                        POCType = 1,
                        Description = "Description",
                    }
                },
            };

            // Mock GetNodeByAsset
            var mockAssetCollection = new Mock<IMongoCollection<MongoAsset.Asset>>();
            _mockDatabase.Setup(m => m.GetCollection<MongoAsset.Asset>("Asset", null)).Returns(mockAssetCollection.Object);

            var assetDataList = new List<MongoAsset.Asset> { node };
            var assetCursor = new Mock<IAsyncCursor<MongoAsset.Asset>>();
            assetCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            assetCursor.Setup(_ => _.Current).Returns(assetDataList);

            mockAssetCollection.Setup(x => x.FindSync(It.IsAny<FilterDefinition<MongoAsset.Asset>>(),
                                                      It.IsAny<FindOptions<MongoAsset.Asset>>(),
                                                      It.IsAny<CancellationToken>())).Returns(assetCursor.Object);

            // Ensure that the node is not null
            Assert.IsNotNull(node, "The node should not be null.");

            // Mock Setpoint Groups
            var mockSetpointGroupCollection = new Mock<IMongoCollection<Lookup>>();
            _mockDatabase.Setup(m => m.GetCollection<Lookup>("Lookup", null)).Returns(mockSetpointGroupCollection.Object);
            var setpointGroupDocs = new List<Lookup>
            {
                new() {
                    LookupDocument = new MongoLookup.SetpointGroups()
                    {
                        SetpointGroupsId = 1,
                        DisplayName = "Group 1",
                        DisplayOrder = 1
                    }
                }
            };
            var setpointGroupCursor = new Mock<IAsyncCursor<Lookup>>();
            setpointGroupCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            setpointGroupCursor.Setup(_ => _.Current).Returns(setpointGroupDocs);

            mockSetpointGroupCollection.Setup(x => x.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                                                              It.IsAny<FindOptions<Lookup>>(),
                                                              It.IsAny<CancellationToken>())).Returns(setpointGroupCursor.Object);

            // Mock Parameters
            var mockParameterCollection = new Mock<IMongoCollection<Parameters>>();
            _mockDatabase.Setup(m => m.GetCollection<Parameters>("MasterVariables", null)).Returns(mockParameterCollection.Object);
            var parameterDocs = new List<Parameters>
            {
                new() {
                    Address = 123,
                    Description = "Parameter 1",
                    ParameterType = "Param",
                    ParameterDocument = new ParameterDetails
                    {
                        SetpointGroup = 1,
                        EarliestSupportedVersion = 1.0f
                    },
                    POCType = new Lookup
                    {
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "1" },
                        },
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LookupDocument = new MongoLookup.POCTypes()
                        {
                            POCType = 1,
                            Description = "Description",
                        }
                    },
                    State = 1
                }
            };
            var parameterCursor = new Mock<IAsyncCursor<Parameters>>();
            parameterCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            parameterCursor.Setup(_ => _.Current).Returns(parameterDocs);
            mockParameterCollection.Setup(x => x.FindSync(It.IsAny<FilterDefinition<Parameters>>(),
                                                          It.IsAny<FindOptions<Parameters>>(),
                                                          It.IsAny<CancellationToken>())).Returns(parameterCursor.Object);

            // Mock SavedParameters Collection
            var mockSavedParameterCollection = new Mock<IMongoCollection<SavedParameters>>();
            _mockDatabase.Setup(m => m.GetCollection<SavedParameters>("SavedParameters", null)).Returns(mockSavedParameterCollection.Object);
            var savedParameterDocs = new List<SavedParameters>
            {
                new () {
                    Address = 3,
                    Value = 200912.0,
                    BackupDate = new DateTime(2024, 6, 26),
                    AssetId = node.Id // Ensure AssetId matches the node.Id
                }
            };
            var savedParameterCursor = new Mock<IAsyncCursor<SavedParameters>>();
            savedParameterCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            savedParameterCursor.Setup(_ => _.Current).Returns(savedParameterDocs);
            mockSavedParameterCollection.Setup(x => x.FindSync(It.IsAny<FilterDefinition<SavedParameters>>(),
                                                              It.IsAny<FindOptions<SavedParameters>>(),
                                                              It.IsAny<CancellationToken>())).Returns(savedParameterCursor.Object);

            var result = _store.GetSetPointGroupData(assetId, correlationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Group 1", result.First().SetpointGroupName);
        }

        // Scenario : AssetId passed is not found in the Asset collection 
        [TestMethod]
        public void GetSetPointGroupData_AssetNotFound_ReturnsEmptyList()
        {
            // Arrange
            var assetId = Guid.Parse("FE457B4C-C3F7-40DE-9708-E2F36E78DAFE");
            var correlationId = "test-correlation";

            // Mock GetNodeByAsset to return no asset (empty result)
            var mockAssetCollection = new Mock<IMongoCollection<MongoAsset.Asset>>();
            _mockDatabase.Setup(m => m.GetCollection<MongoAsset.Asset>("Asset", null)).Returns(mockAssetCollection.Object);

            var emptyAssetCursor = new Mock<IAsyncCursor<MongoAsset.Asset>>();
            emptyAssetCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(false);
            emptyAssetCursor.Setup(_ => _.Current).Returns(new List<MongoAsset.Asset>());

            mockAssetCollection.Setup(x => x.FindSync(It.IsAny<FilterDefinition<MongoAsset.Asset>>(),
                                                      It.IsAny<FindOptions<MongoAsset.Asset>>(),
                                                      It.IsAny<CancellationToken>())).Returns(emptyAssetCursor.Object);

            var result = _store.GetSetPointGroupData(assetId, correlationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);   // Since no asset is found, the result should be an empty list
        }

    }
}
