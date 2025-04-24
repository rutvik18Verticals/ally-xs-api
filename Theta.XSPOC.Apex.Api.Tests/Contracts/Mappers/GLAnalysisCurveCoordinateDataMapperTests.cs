using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Tests.Contracts.Mappers
{
    [TestClass]
    public class GLAnalysisCurveCoordinateDataMapperTests
    {

        [TestMethod]
        public void MapNullCoreModelReturnsEmptyResponseTest()
        {
            var correlationId = "CorrelationId1";

            var result = GLAnalysisCurveCoordinateDataMapper.Map(correlationId, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void MapCoreModelWithValuesTest()
        {
            var correlationId = "CorrelationId1";
            var coreModel = new GLAnalysisCurveCoordinateDataOutput()
            {
                Values = new List<GLAnalysisCurveCoordinateData>()
                {
                    new GLAnalysisCurveCoordinateData()
                    {
                        Id = 1,
                        Name = "Gas Injection Curve",
                        CurveTypeId = 1,
                        DisplayName = "Gas Injection Curve",
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
                    new GLAnalysisCurveCoordinateData()
                    {
                        Id = 2,
                        Name = "Production Fluid Curve",
                        CurveTypeId =2 ,
                        DisplayName = "Production Fluid Curve",
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
                Result = new Kernel.Collaboration.Models.MethodResult<string>(true, string.Empty),
            };

            var result = GLAnalysisCurveCoordinateDataMapper.Map(correlationId, coreModel);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Values.Count);

            Assert.AreEqual(2, result.Values[0].Coordinates.Count);
            Assert.AreEqual(1, result.Values[0].Coordinates[0].X);
            Assert.AreEqual(1, result.Values[0].Coordinates[0].Y);
            Assert.AreEqual(2, result.Values[0].Coordinates[1].X);
            Assert.AreEqual(2, result.Values[0].Coordinates[1].Y);
            Assert.AreEqual("Gas Injection Curve", result.Values[0].Name);
            Assert.AreEqual("Production Fluid Curve", result.Values[1].Name);
        }

        [TestMethod]
        public void MapCoreModelWithNoCoordinatesDataValuesTest()
        {
            var correlationId = "CorrelationId1";
            var coreModel = new GLAnalysisCurveCoordinateDataOutput()
            {
                Values = new List<GLAnalysisCurveCoordinateData>()
                {
                    new GLAnalysisCurveCoordinateData()
                    {
                        Id = 1,
                        Name = "Gas Injection Curve",
                        CurveTypeId = 1,
                        DisplayName = "Gas Injection Curve",
                    },
                    new GLAnalysisCurveCoordinateData()
                    {
                        Id = 2,
                        Name = "Production Fluid Curve",
                        CurveTypeId =2 ,
                        DisplayName = "Production Fluid Curve",
                    }
                },
                Result = new Kernel.Collaboration.Models.MethodResult<string>(true, string.Empty),
            };

            var result = GLAnalysisCurveCoordinateDataMapper.Map(correlationId, coreModel);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Values.Count);
            Assert.IsNull(result.Values[0].Coordinates);
        }

    }
}
