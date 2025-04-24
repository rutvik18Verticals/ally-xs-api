using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Tests.Contracts.Mappers
{
    [TestClass]
    public class CardCoordinateDataMapperTests
    {

        [TestMethod]
        public void MapNullCoreModelReturnsEmptyResponseTest()
        {
            var correlationId = "TestCorrelationId";

            var result = CardCoordinateDataMapper.Map(correlationId, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Values.Count);
        }

        [TestMethod]
        public void MapEmptyCoreModelValuesReturnsEmptyResponseTest()
        {
            var correlationId = "TestCorrelationId";
            var coreModel = new CardCoordinateDataOutput();

            var result = CardCoordinateDataMapper.Map(correlationId, coreModel);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Values.Count);
        }

        [TestMethod]
        public void MapCoreModelWithValuesTest()
        {
            var correlationId = "TestCorrelationId";
            var coreModel = new CardCoordinateDataOutput()
            {
                Id = correlationId,
                DateCreated = new DateTime(1997, 3, 12),
                Values = new List<CardResponseValuesOutput>()
                {
                    new CardResponseValuesOutput()
                    {
                        Id = 1,
                        Name = "TestName1",
                        CoordinatesOutput = new List<CoordinatesData<float>>()
                        {
                            new CoordinatesData<float>()
                            {
                                X = 1,
                                Y = 1,
                            },
                            new CoordinatesData<float>()
                            {
                                X = 2,
                                Y = 2,
                            },
                        },
                    },
                    new CardResponseValuesOutput()
                    {
                        Id = 2,
                        Name = "TestName2",
                        CoordinatesOutput = new List<CoordinatesData<float>>()
                        {
                            new CoordinatesData<float>()
                            {
                                X = 3,
                                Y = 3,
                            },
                        },
                    },
                },
            };

            var result = CardCoordinateDataMapper.Map(correlationId, coreModel);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Values.Count);

            // First Card
            Assert.AreEqual(2, result.Values[0].Coordinates.Count);
            Assert.AreEqual(1, result.Values[0].Coordinates[0].X);
            Assert.AreEqual(1, result.Values[0].Coordinates[0].Y);
            Assert.AreEqual(2, result.Values[0].Coordinates[1].X);
            Assert.AreEqual(2, result.Values[0].Coordinates[1].Y);

            // Second Card
            Assert.AreEqual(1, result.Values[1].Coordinates.Count);
            Assert.AreEqual(3, result.Values[1].Coordinates[0].X);
            Assert.AreEqual(3, result.Values[1].Coordinates[0].Y);
        }

        [TestMethod]
        public void MapCoreModelWithNoCoordinatesDataValuesTest()
        {
            var correlationId = "TestCorrelationId";
            var coreModel = new CardCoordinateDataOutput()
            {
                Id = correlationId,
                DateCreated = new DateTime(1997, 3, 12),
                Values = new List<CardResponseValuesOutput>()
                {
                    new CardResponseValuesOutput()
                    {
                        Id = 1,
                        Name = "TestName1",
                        CoordinatesOutput = new List<CoordinatesData<float>>()
                        {
                            new CoordinatesData<float>()
                            {
                                X = 1,
                                Y = 1,
                            },
                            new CoordinatesData<float>()
                            {
                                X = 2,
                                Y = 2,
                            },
                        },
                    },
                    new CardResponseValuesOutput()
                    {
                        Id = 2,
                        Name = "TestName2",
                    },
                },
            };

            var result = CardCoordinateDataMapper.Map(correlationId, coreModel);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Values.Count);

            // First Card
            Assert.AreEqual(2, result.Values[0].Coordinates.Count);
            Assert.AreEqual(1, result.Values[0].Coordinates[0].X);
            Assert.AreEqual(1, result.Values[0].Coordinates[0].Y);
            Assert.AreEqual(2, result.Values[0].Coordinates[1].X);
            Assert.AreEqual(2, result.Values[0].Coordinates[1].Y);
        }

    }
}
