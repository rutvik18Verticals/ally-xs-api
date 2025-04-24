using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;

namespace Theta.XSPOC.Apex.Api.Core.Tests.GroupStatusColumnFormatter
{

    [TestClass]
    public class PercentRTColumnFormatterTests
    {

        #region Private Fields

        private PercentRTColumnFormatter _formatter;
        private Mock<ICommonService> _commonService;
        private Mock<IMemoryCache> _memoryCache;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _memoryCache = new Mock<IMemoryCache>();
            _commonService = new Mock<ICommonService>();

            SetupMemoryCache();
            SetupSystemParameterStore();

            _formatter = new PercentRTColumnFormatter(_commonService.Object);
        }

        [TestMethod]
        public void ConstructorNullParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new PercentRTColumnFormatter(null));
        }

        [TestMethod]
        public void CalculateValueNullDrParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.CalculateValue(null, new RowColumnModel(), string.Empty, true));
        }

        [TestMethod]
        public void CalculateValueNullColumnParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _formatter.CalculateValue(new Dictionary<string, object>(), null, string.Empty, true));
        }

        [TestMethod]
        public void CalculateValueTest()
        {
            var dr = new Dictionary<string, object>();
            dr.Add("Well", "Well1");
            dr.Add("tblNodeMaster.LastGoodScanTime", "2023-01-17 02:45:22.000");
            dr.Add("tblNodeMaster.TodayRuntime", "17.8");
            dr.Add("tblNodeMaster.POCType", "8");
            dr.Add("Enabled", "True");

            var column = new RowColumnModel();
            _formatter.CalculateValue(dr, column, string.Empty, true);

            Assert.AreEqual("100", column.Value);
        }

        [TestMethod]
        public void CalculateValueHoursSinceGaugeOffLessThanZeroTest()
        {
            var dr = new Dictionary<string, object>();
            dr.Add("Well", "Well1");
            dr.Add("tblNodeMaster.LastGoodScanTime", "2023-01-17 02:45:22.000");
            dr.Add("tblNodeMaster.TodayRuntime", "17.8");
            dr.Add("tblNodeMaster.POCType", "8");
            dr.Add("Enabled", "True");

            _commonService.Setup(x => x.GetSystemParameter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("3");

            var column = new RowColumnModel();
            _formatter.CalculateValue(dr, column, string.Empty, true);

            Assert.AreEqual("75", column.Value);
        }

        [TestMethod]
        public void CalculateValueZeroTest()
        {
            var dr = new Dictionary<string, object>();
            dr.Add("Well", "Well1");
            dr.Add("tblNodeMaster.LastGoodScanTime", "2023-01-17 02:00:00.000");
            dr.Add("tblNodeMaster.TodayRuntime", "17.8");
            dr.Add("tblNodeMaster.POCType", "8");
            dr.Add("Enabled", "True");

            _commonService.Setup(x => x.GetSystemParameter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("2");

            var column = new RowColumnModel();
            _formatter.CalculateValue(dr, column, string.Empty, true);

            Assert.AreEqual("0", column.Value);
        }

        [TestMethod]
        public void CalculateValuePocType9Test()
        {
            var dr = new Dictionary<string, object>();
            dr.Add("Well", "Well1");
            dr.Add("tblNodeMaster.LastGoodScanTime", "2023-01-17 02:45:22.000");
            dr.Add("tblNodeMaster.TodayRuntime", "17.8");
            dr.Add("tblNodeMaster.POCType", "9");
            dr.Add("Enabled", "True");

            var column = new RowColumnModel();
            _formatter.CalculateValue(dr, column, string.Empty, true);

            Assert.AreEqual("74", column.Value);
        }

        private void SetupMemoryCache()
        {
            _memoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());
        }

        private void SetupSystemParameterStore()
        {
            _commonService.Setup(x => x.GetSystemParameter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("0");
        }

    }
}
