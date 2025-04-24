using InfluxDB.Client.Core.Flux.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Alarms;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Camera;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class AlarmMongoStoreTest
    {
        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new AlarmMongoStore(null, null, null);
        }

        [TestMethod]
        public void GetAlarmConfigurationAsyncTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockHostAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockHostAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("RTUAlarm"));

            var mockParameterCollection = new Mock<IMongoCollection<Parameters>>();
            mockParameterCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Parameters>>(),
                               It.IsAny<FindOptions<Parameters>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Parameters>("MasterVariables"));

            var mockLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.States));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<AlarmConfiguration>("AlarmConfiguration", null))
                .Returns(mockHostAlarmCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Parameters>("MasterVariables", null))
                .Returns(mockParameterCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockLookupCollection.Object);

            // Influx Setup
            var mocConfig = new Mock<IConfiguration>();

            Mock<IConfigurationSection> mockBucketName = new Mock<IConfigurationSection>();
            mockBucketName.Setup(x => x.Value).Returns("XSPOC");

            Mock<IConfigurationSection> mockOrg = new Mock<IConfigurationSection>();
            mockOrg.Setup(x => x.Value).Returns("XSPOC");

            Mock<IConfigurationSection> mockMeasurement = new Mock<IConfigurationSection>();
            mockOrg.Setup(x => x.Value).Returns("XSPOCData");

            mocConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "AppSettings:BucketName")))
                .Returns(mockBucketName.Object);
            mocConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "AppSettings:Org")))
                .Returns(mockOrg.Object);
            mocConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "AppSettings:MeasurementName")))
               .Returns(mockMeasurement.Object);

            TestUtilityInflux.WriteMockData("XSPOC", "XSPOC");

            var mocInfluxClient = new Mock<IDataHistoryTrendData>();
            var tables = new List<FluxTable>();

            var assetStore = new AlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object, mocInfluxClient.Object);

            var request = new WithCorrelationId<DataHistoryTrendInput>("correlationId1", new DataHistoryTrendInput
            {
                AssetId = new Guid("61e72096-72d4-4878-afb7-f042e0a30118"),
                CustomerId = new Guid("9db1a9ea-e23e-4ee8-824a-887bb09e541e"),
                POCType = "8",
                //Address = "2022",
                StartDate = "2024-01-11",
                EndDate = "2024-04-11"
            });
            var addresses = new List<string>();
            addresses.Add(request.Value.Address);

            var paramStdType = new List<string>();
            paramStdType.Add(request.Value.Address);

            var result = assetStore.GetAlarmConfigurationAsync(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), new Guid("9db1a9ea-e23e-4ee8-824a-887bb09e541e"));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAlarmConfigurationAsyncNotFoundTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                .Eq("LegacyId.NodeId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            mockCollection.Setup(m => m.FindSync(filter,
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockHostAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockHostAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("RTUAlarm"));

            var mockParameterCollection = new Mock<IMongoCollection<Parameters>>();
            mockParameterCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Parameters>>(),
                               It.IsAny<FindOptions<Parameters>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Parameters>("MasterVariables"));

            var mockLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.States));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<AlarmConfiguration>("AlarmConfiguration", null))
                .Returns(mockHostAlarmCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Parameters>("MasterVariables", null))
                .Returns(mockParameterCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockLookupCollection.Object);

            // Influx Setup
            var mocConfig = new Mock<IConfiguration>();

            Mock<IConfigurationSection> mockBucketName = new Mock<IConfigurationSection>();
            mockBucketName.Setup(x => x.Value).Returns("XSPOC");

            Mock<IConfigurationSection> mockOrg = new Mock<IConfigurationSection>();
            mockOrg.Setup(x => x.Value).Returns("XSPOC");

            Mock<IConfigurationSection> mockMeasurement = new Mock<IConfigurationSection>();
            mockOrg.Setup(x => x.Value).Returns("XSPOCData");

            mocConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "AppSettings:BucketName")))
                .Returns(mockBucketName.Object);
            mocConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "AppSettings:Org")))
                .Returns(mockOrg.Object);
            mocConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "AppSettings:MeasurementName")))
               .Returns(mockMeasurement.Object);

            TestUtilityInflux.WriteMockData("XSPOC", "XSPOC");

            var mocInfluxClient = new Mock<IDataHistoryTrendData>();
            var tables = new List<FluxTable>();

            var assetStore = new AlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object, mocInfluxClient.Object);

            var result = assetStore.GetAlarmConfigurationAsync(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), new Guid("9db1a9ea-e23e-4ee8-824a-887bb09e541e"));
            Assert.AreEqual(0, result.Result.Count);
            logger.Verify(m => m.Write(Level.Info, "Missing node"), Times.Once);
        }

        [TestMethod]
        public void GetHostAlarmsAsyncTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockHostAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockHostAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("HostAlarm"));

            var mockFacilityTagAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockFacilityTagAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("FacilityTagAlarm"));

            var mockParameterCollection = new Mock<IMongoCollection<Parameters>>();
            mockParameterCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Parameters>>(),
                               It.IsAny<FindOptions<Parameters>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Parameters>("MasterVariables"));

            var mockLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.LocalePhrases));

            var mockxDiagLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockxDiagLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.XDiagOutputs));

            var mockNotificationCollection = new Mock<IMongoCollection<Notification>>();
            mockNotificationCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Notification>>(),
                               It.IsAny<FindOptions<Notification>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Notification>("Notification"));

            // setup mock database
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<AlarmConfiguration>("AlarmConfiguration", null))
                .Returns(mockHostAlarmCollection.Object)
                .Returns(mockFacilityTagAlarmCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Parameters>("MasterVariables", null))
                .Returns(mockParameterCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockLookupCollection.Object)
                .Returns(mockxDiagLookupCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Notification>("Notification", null))
                .Returns(mockNotificationCollection.Object);

            var mocInfluxClient = new Mock<IDataHistoryTrendData>();
            var tables = new List<FluxTable>();

            var assetStore = new AlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object, mocInfluxClient.Object);

            var result = assetStore.GetHostAlarmsAsync(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), new Guid("9db1a9ea-e23e-4ee8-824a-887bb09e541e")).Result;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetHostAlarmsAsyncNotFoundTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Eq("LegacyId.NodeId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            mockCollection.Setup(m => m.FindSync(filter,
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockHostAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockHostAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("HostAlarm"));

            var mockFacilityTagAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockFacilityTagAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("FacilityTagAlarm"));

            var mockParameterCollection = new Mock<IMongoCollection<Parameters>>();
            mockParameterCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Parameters>>(),
                               It.IsAny<FindOptions<Parameters>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Parameters>("MasterVariables"));

            var mockLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.LocalePhrases));

            var mockxDiagLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockxDiagLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.XDiagOutputs));

            var mockNotificationCollection = new Mock<IMongoCollection<Notification>>();
            mockNotificationCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Notification>>(),
                               It.IsAny<FindOptions<Notification>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Notification>("Notification"));

            // setup mock database
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<AlarmConfiguration>("AlarmConfiguration", null))
                .Returns(mockHostAlarmCollection.Object)
                .Returns(mockFacilityTagAlarmCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Parameters>("MasterVariables", null))
                .Returns(mockParameterCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockLookupCollection.Object)
                .Returns(mockxDiagLookupCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Notification>("Notification", null))
                .Returns(mockNotificationCollection.Object);

            var mocInfluxClient = new Mock<IDataHistoryTrendData>();
            var tables = new List<FluxTable>();

            var assetStore = new AlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object, mocInfluxClient.Object);

            var result = assetStore.GetHostAlarmsAsync(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), new Guid("9db1a9ea-e23e-4ee8-824a-887bb09e541e")).Result;

            Assert.AreEqual(0, result.Count);
            logger.Verify(m => m.Write(Level.Info, "Missing node"), Times.Once);
        }

        [TestMethod]
        public void GetFacilityTagAlarmsAsyncTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockFacilityTagAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockFacilityTagAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("FacilityTagAlarm"));

            var mockParameterCollection = new Mock<IMongoCollection<Parameters>>();
            mockParameterCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Parameters>>(),
                               It.IsAny<FindOptions<Parameters>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Parameters>("MasterVariables"));

            // setup mock database
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<AlarmConfiguration>("AlarmConfiguration", null))
                .Returns(mockFacilityTagAlarmCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Parameters>("MasterVariables", null))
                .Returns(mockParameterCollection.Object);

            var mocInfluxClient = new Mock<IDataHistoryTrendData>();
            var tables = new List<FluxTable>();

            var assetStore = new AlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object, mocInfluxClient.Object);

            var result = assetStore.GetFacilityTagAlarmsAsync(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), new Guid("9db1a9ea-e23e-4ee8-824a-887bb09e541e")).Result;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetFacilityTagAlarmsAsyncNotFoundTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Eq("LegacyId.NodeId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            mockCollection.Setup(m => m.FindSync(filter,
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockFacilityTagAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockFacilityTagAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("FacilityTagAlarm"));

            var mockParameterCollection = new Mock<IMongoCollection<Parameters>>();
            mockParameterCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Parameters>>(),
                               It.IsAny<FindOptions<Parameters>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Parameters>("MasterVariables"));

            // setup mock database
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<AlarmConfiguration>("AlarmConfiguration", null))
                .Returns(mockFacilityTagAlarmCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Parameters>("MasterVariables", null))
                .Returns(mockParameterCollection.Object);

            var mocInfluxClient = new Mock<IDataHistoryTrendData>();
            var tables = new List<FluxTable>();

            var assetStore = new AlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object, mocInfluxClient.Object);

            var result = assetStore.GetFacilityTagAlarmsAsync(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), new Guid("9db1a9ea-e23e-4ee8-824a-887bb09e541e")).Result;

            Assert.AreEqual(0, result.Count);
            // logger.Verify(m => m.WriteCId(Level.Info, "Missing node", "correlationId"), Times.Once);
        }

        [TestMethod]
        public void GetCameraAlarmsAsyncTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockCameraCollection = new Mock<IMongoCollection<Camera>>();
            mockCameraCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Camera>>(),
                It.IsAny<FindOptions<Camera>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Camera>("Cameras"));

            var mockCameraAlarmsCollection = new Mock<IMongoCollection<CameraAlarms>>();
            mockCameraAlarmsCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<CameraAlarms>>(),
                               It.IsAny<FindOptions<CameraAlarms>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<CameraAlarms>("CameraAlarms"));

            var mockAlarmEventsCollection = new Mock<IMongoCollection<AlarmEvents>>();
            mockAlarmEventsCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmEvents>>(),
                               It.IsAny<FindOptions<AlarmEvents>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmEvents>("AlarmEvents"));

            var mockxDiagLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockxDiagLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.CameraAlarmTypes));

            var mockLocalPhraseLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockLocalPhraseLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.LocalePhrases));

            // setup mock database
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Camera>("Cameras", null))
                .Returns(mockCameraCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<CameraAlarms>("CameraAlarms", null))
                .Returns(mockCameraAlarmsCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<AlarmEvents>("AlarmEvents", null))
                .Returns(mockAlarmEventsCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockxDiagLookupCollection.Object)
                .Returns(mockLocalPhraseLookupCollection.Object);

            var mocInfluxClient = new Mock<IDataHistoryTrendData>();
            var tables = new List<FluxTable>();

            var assetStore = new AlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object, mocInfluxClient.Object);

            var result = assetStore.GetCameraAlarmsAsync(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), new Guid("9db1a9ea-e23e-4ee8-824a-887bb09e541e")).Result;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetCameraAlarmsAsyncNotFoundTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Eq("LegacyId.NodeId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            mockCollection.Setup(m => m.FindSync(filter,
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockCameraCollection = new Mock<IMongoCollection<Camera>>();
            mockCameraCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Camera>>(),
                It.IsAny<FindOptions<Camera>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Camera>("Cameras"));

            var mockCameraAlarmsCollection = new Mock<IMongoCollection<CameraAlarms>>();
            mockCameraAlarmsCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<CameraAlarms>>(),
                               It.IsAny<FindOptions<CameraAlarms>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<CameraAlarms>("CameraAlarms"));

            var mockAlarmEventsCollection = new Mock<IMongoCollection<AlarmEvents>>();
            mockAlarmEventsCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmEvents>>(),
                               It.IsAny<FindOptions<AlarmEvents>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmEvents>("AlarmEvents"));

            var mockxDiagLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockxDiagLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.CameraAlarmTypes));

            var mockLocalPhraseLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockLocalPhraseLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.LocalePhrases));

            // setup mock database
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Camera>("Cameras", null))
                .Returns(mockCameraCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<CameraAlarms>("CameraAlarms", null))
                .Returns(mockCameraAlarmsCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<AlarmEvents>("AlarmEvents", null))
                .Returns(mockAlarmEventsCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockxDiagLookupCollection.Object)
                .Returns(mockLocalPhraseLookupCollection.Object);

            var mocInfluxClient = new Mock<IDataHistoryTrendData>();
            var tables = new List<FluxTable>();

            var assetStore = new AlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object, mocInfluxClient.Object);

            var result = assetStore.GetCameraAlarmsAsync(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), new Guid("9db1a9ea-e23e-4ee8-824a-887bb09e541e")).Result;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetFacilityHeaderAndDetailsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockCustomerCollection = new Mock<IMongoCollection<Customer>>();
            mockCustomerCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<FindOptions<Customer>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Customer>("Customers"));

            var mockFacilityTagAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockFacilityTagAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("FacilityTagAlarm"));

            var mockHostAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockHostAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("HostAlarm"));

            var mockParameterCollection = new Mock<IMongoCollection<Parameters>>();
            mockParameterCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Parameters>>(),
                               It.IsAny<FindOptions<Parameters>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Parameters>("MasterVariables"));

            var mockFacilityTagLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockFacilityTagLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.FacilityTagGroups));

            var mockParamStandardTypesLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockParamStandardTypesLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.ParamStandardTypes));

            var mockStatesLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockStatesLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.States));

            // setup mock database
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Customer>("Customers", null))
                .Returns(mockCustomerCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<AlarmConfiguration>("AlarmConfiguration", null))
                .Returns(mockFacilityTagAlarmCollection.Object)
                .Returns(mockHostAlarmCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Parameters>("MasterVariables", null))
                .Returns(mockParameterCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockFacilityTagLookupCollection.Object)
                .Returns(mockParamStandardTypesLookupCollection.Object)
                .Returns(mockStatesLookupCollection.Object);

            var mocInfluxClient = new Mock<IDataHistoryTrendData>();
            var tables = new List<FluxTable>();

            var assetStore = new AlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object, mocInfluxClient.Object);

            var result = assetStore.GetFacilityHeaderAndDetails(new Guid("61e72096-72d4-4878-afb7-f042e0a30118")).Result;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetFacilityHeaderAndDetailsNotFoundTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Eq("LegacyId.NodeId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            mockCollection.Setup(m => m.FindSync(filter,
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockFacilityTagAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockFacilityTagAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("FacilityTagAlarm"));

            var mockHostAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockHostAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("HostAlarm"));

            var mockParameterCollection = new Mock<IMongoCollection<Parameters>>();
            mockParameterCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Parameters>>(),
                               It.IsAny<FindOptions<Parameters>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Parameters>("MasterVariables"));

            var mockFacilityTagLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockFacilityTagLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.FacilityTagGroups));

            var mockParamStandardTypesLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockParamStandardTypesLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.ParamStandardTypes));

            var mockStatesLookupCollection = new Mock<IMongoCollection<Lookup>>();
            mockStatesLookupCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.States));

            // setup mock database
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<AlarmConfiguration>("AlarmConfiguration", null))
                .Returns(mockFacilityTagAlarmCollection.Object)
                .Returns(mockHostAlarmCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Parameters>("MasterVariables", null))
                .Returns(mockParameterCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockFacilityTagLookupCollection.Object)
                .Returns(mockParamStandardTypesLookupCollection.Object)
                .Returns(mockStatesLookupCollection.Object);

            var mocInfluxClient = new Mock<IDataHistoryTrendData>();
            var tables = new List<FluxTable>();

            var assetStore = new AlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object, mocInfluxClient.Object);

            var result = assetStore.GetFacilityHeaderAndDetails(new Guid("61e72096-72d4-4878-afb7-f042e0a30118")).Result;

            Assert.IsNull(result.NodeId);
            //Assert.IsNotNull(result);
        }
        #endregion
    }
}
