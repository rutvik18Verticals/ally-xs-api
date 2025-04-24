using InfluxDB.Client.Core.Flux.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Data.Influx.Tests.Services
{
    [TestClass]
    public class GetDataHistoryItemsServiceTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullConfigurationTest()
        {
            var mocInfluxClient = new Mock<IDataHistoryTrendData>();
            _ = new GetDataHistoryItemsService(null, mocInfluxClient.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullInfluxDbClientTest()
        {
            var mocConfig = new Mock<IConfiguration>();
            _ = new GetDataHistoryItemsService(mocConfig.Object, null);
        }

        [TestMethod]
        public void GetDataHistoryItemsTest()
        {
            var mocConfig = new Mock<IConfiguration>();

            Mock<IConfigurationSection> mockBucketName = new Mock<IConfigurationSection>();
            mockBucketName.Setup(x => x.Value).Returns("XSPOC");

            Mock<IConfigurationSection> mockOrg = new Mock<IConfigurationSection>();
            mockOrg.Setup(x => x.Value).Returns("XSPOC");

            mocConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "AppSettings:BucketName")))
                .Returns(mockBucketName.Object);
            mocConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "AppSettings:Org")))
                .Returns(mockOrg.Object);

            var mocInfluxClient = new Mock<IDataHistoryTrendData>();

            var tables = new List<FluxTable>();

            var request = new WithCorrelationId<DataHistoryTrendInput>("correlationId1", new DataHistoryTrendInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                CustomerId = new Guid("58505bfa-6e9c-40d6-b489-4d95f8c6acdf"),
                POCType = "8",
                Address = "2022",
                StartDate = "2024-01-01",
                EndDate = "2024-04-01"
            });

            var service = new GetDataHistoryItemsService(mocConfig.Object, mocInfluxClient.Object);

            var addresses = new List<string>();
            addresses.Add(request.Value.Address);

            var paramStdType = new List<string>();
            paramStdType.Add(request.Value.Address);

            var resultNew = service.GetDataHistoryTrendData(request.Value.AssetId, request.Value.CustomerId, request.Value.POCType,
                addresses, request.Value.StartDate, request.Value.EndDate).Result;

            var result = service.GetDataHistoryItems(request.Value.AssetId, request.Value.CustomerId,
                request.Value.POCType, addresses, It.IsAny<List<string>>(), request.Value.StartDate, request.Value.EndDate);
            mocInfluxClient.Verify(x => x.GetTrendData(request.Value.AssetId, request.Value.CustomerId, request.Value.POCType,
                addresses, It.IsAny<List<string>>(), request.Value.StartDate, request.Value.EndDate), Times.Once);
        }

        #endregion

    }
}
