using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using InfluxDB.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Moq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.AspNetCore.Mvc;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    public class TestUtilityInflux
    {
        public static void WriteMockData(string bucket, string org)
        {
            // Arrange
            var mockWriteApi = new Mock<IWriteApiAsync>();
            var service = new InfluxDataService(mockWriteApi.Object);

            var mockData = new MockInfluxData
            {
                Measurement = "XSPOCData",
                Tags = new Dictionary<string, string> { { "AssetID", "61e72096-72d4-4878-afb7-f042e0a30118" }, { "CustomerID", "9db1a9ea-e23e-4ee8-824a-887bb09e541e" }, { "POCType", "8" } },
                Fields = new Dictionary<string, object> { { "C1006", 60 }, { "C1005", 50} },
                Timestamp = DateTime.UtcNow
            };

            PointData capturedPointData = null;

            // Set up a callback to capture the PointData instance
            mockWriteApi
                .Setup(writeApi => writeApi.WritePointAsync(It.IsAny<PointData>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
                .Callback<PointData, string, string, System.Threading.CancellationToken>((point, b, o, ct) => capturedPointData = point);

            // Act
            service.WriteMockDataAsync(mockData, bucket, org).Wait();

            //// Assert
            //Assert.IsNotNull(capturedPointData);
            //var lineProtocol = capturedPointData.ToLineProtocol();

            //Assert.Contains("weather", lineProtocol);
            //Assert.Contains("location=office", lineProtocol);
            //Assert.Contains("temperature=25", lineProtocol);
            //Assert.Contains("humidity=40", lineProtocol);
        }
    }

    public class InfluxDataService
    {
        private readonly IWriteApiAsync _writeApi;

        public InfluxDataService(IWriteApiAsync writeApi)
        {
            _writeApi = writeApi;
        }

        public async Task WriteMockDataAsync(MockInfluxData data, string bucket, string org)
        {
            if (data != null)
            {
                var point = PointData
                .Measurement(data.Measurement)
                .Tag("AssetID", data.Tags["AssetID"])
                .Tag("CustomerID", data.Tags["CustomerID"])
                .Tag("POCType", data.Tags["POCType"])
                .Field("C1006", data.Fields["C1006"])
                .Field("C1005", data.Fields["C1005"])
                .Timestamp(data.Timestamp, WritePrecision.Ns);

                await _writeApi.WritePointAsync(point, bucket, org);
            }
            else
            {
                await Task.Yield();
            }
        }
    }

    public class MockInfluxData
    {
        public string Measurement { get; set; }
        public Dictionary<string, string> Tags { get; set; }
        public Dictionary<string, object> Fields { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
