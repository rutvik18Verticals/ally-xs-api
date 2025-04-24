using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Tests.Contracts.Mappers
{
    [TestClass]
    public class DataHistoryMapperTests
    {

        [TestMethod]
        public void MapDataHistoryTrendOutputToDataHistoryTrendResponseTest()
        {
            var output = new DataHistoryTrendOutput()
            {
                Values = new List<GraphViewTrendsData>()
                {
                    new GraphViewTrendsData()
                    {
                        AxisIndex = 0,
                        AxisLabel = "TestAxisLabel1",
                        ItemId = 100,
                        AxisValues = new List<Core.Common.DataPoint>()
                        {
                            new Core.Common.DataPoint()
                            {
                                X = DateTime.Parse("2024-05-17"),
                                Y = 1000,
                                Note = "TestNote1000"
                            }
                        }
                    },
                    new GraphViewTrendsData()
                    {
                        AxisIndex = 1,
                        AxisLabel = "TestAxisLabel2",
                        ItemId = 200,
                        AxisValues = new List<Core.Common.DataPoint>()
                        {
                            new Core.Common.DataPoint()
                            {
                                X = DateTime.Parse("2024-05-17"),
                                Y = 2000,
                                Note = "TestNote2000"
                            }
                        }
                    },
                    new GraphViewTrendsData()
                    {
                        AxisIndex = 2,
                        AxisLabel = "TestAxisLabel3",
                        ItemId = 300,
                        AxisValues = new List<Core.Common.DataPoint>()
                        {
                            new Core.Common.DataPoint()
                            {
                                X = DateTime.Parse("2024-05-17"),
                                Y = 3000,
                                Note = "TestNote3000"
                            }
                        }
                    },
                    new GraphViewTrendsData()
                    {
                        AxisIndex = 3,
                        AxisLabel = "TestAxisLabel4",
                        ItemId = 400,
                        AxisValues = new List<Core.Common.DataPoint>()
                        {
                            new Core.Common.DataPoint()
                            {
                                X = DateTime.Parse("2024-05-17"),
                                Y = 4000,
                                Note = "TestNote4000"
                            }
                        }
                    }
                }
            };

            var result = DataHistoryMapper.Map(string.Empty, output);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Values.Chart1.Count);
            Assert.AreEqual(1, result.Values.Chart2.Count);
            Assert.AreEqual(1, result.Values.Chart3.Count);
            Assert.AreEqual(1, result.Values.Chart4.Count);
        }

        [TestMethod]
        public void MapDataHistoryTrendOutputToDataHistoryTrendResponseNullTest()
        {
            var result = DataHistoryMapper.Map(string.Empty, (DataHistoryTrendOutput)null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void MapDataHistoryTrendOutputToDataHistoryTrendResponseValuesNullTest()
        {
            var result = DataHistoryMapper.Map(string.Empty, new DataHistoryTrendOutput());

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Values.Chart1.Count);
            Assert.AreEqual(0, result.Values.Chart2.Count);
            Assert.AreEqual(0, result.Values.Chart3.Count);
            Assert.AreEqual(0, result.Values.Chart4.Count);
        }

    }
}
