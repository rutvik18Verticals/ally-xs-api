using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Models.Mappers;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Mappers
{
    [TestClass]
    public class GLAnalysisDataMapperTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapToCurveCoordinateDomainObjectNullEntityTest()
        {
            GLAnalysisDataMapper.MapToCurveCoordinateDomainObject(null);
        }

        [TestMethod]
        public void MapToCurveCoordinateDomainObjectNotNullEntityTest()
        {
            // Arrange
            SurveyData entity = new SurveyData(1)
            {
                SurveyCurve = new SurveyAnalysisCurve(SurveyCurveType.TemperatureCurve)
                {
                    Curve = new List<CurveCoordinate>
                    {
                        new CurveCoordinate()
                        {
                            Coordinate = new Coordinate<double, double>(1.0, 2.0),
                        },
                        new CurveCoordinate
                        {
                            Coordinate = new Coordinate<double, double>(3.0, 4.0),
                        }
                    }
                }
            };

            // Act
            var result = GLAnalysisDataMapper.MapToCurveCoordinateDomainObject(entity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Id, result.Id);
            Assert.AreEqual(entity.SurveyCurve.GetCurveTypeId(), result.CurveTypeId);
            Assert.AreEqual(
                EnhancedEnumBase.GetValue<SurveyCurveType>(entity.SurveyCurve.GetCurveTypeId()).Name.Amount
                    .Replace(" ", string.Empty), result.Name);
            Assert.AreEqual(EnhancedEnumBase.GetValue<SurveyCurveType>(entity.SurveyCurve.GetCurveTypeId()), result.DisplayName);
            Assert.AreEqual(entity.SurveyCurve.GetCurveCoordinateList().Count, result.CoordinatesOutput.Count);
        }

    }
}
