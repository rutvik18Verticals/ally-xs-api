using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Tests.GroupStatusColumnFormatter
{
    [TestClass]
    public class CameraAlarmColumnFormatterTests
    {

        private Mock<ILocalePhrases> _phraseStore;
        private Mock<IMemoryCache> _memoryCache;
        private CameraAlarmColumnFormatter _formatter;

        [TestInitialize]
        public void Initialize()
        {
            _phraseStore = new Mock<ILocalePhrases>();
            _memoryCache = new Mock<IMemoryCache>();

            SetupLocalePhrase();
            SetupMemoryCache();

            _formatter = new CameraAlarmColumnFormatter(_phraseStore.Object, _memoryCache.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateValueNullDataRowTest()
        {
            _formatter.CalculateValue(null, new RowColumnModel(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateValueNullColumnTest()
        {
            _formatter.CalculateValue(new Dictionary<string, object>(), null, It.IsAny<string>());
        }

        [TestMethod]
        public void CalculateValueCountGreater0Test()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "CameraAlarms", 5
                }
            };
            var column = new RowColumnModel();

            _formatter.CalculateValue(dr, column, It.IsAny<string>());

            Assert.AreEqual("Alarm", column.Value);
        }

        [TestMethod]
        public void CalculateValueCountEqual0Test()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "CameraAlarms", 0
                }
            };
            var column = new RowColumnModel();

            _formatter.CalculateValue(dr, column, It.IsAny<string>());

            Assert.AreEqual("OK", column.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PerformFormatColumnNullTest()
        {
            _formatter.PerformFormat(new Dictionary<string, object>(), null, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), It.IsAny<string>());
        }

        [TestMethod]
        public void PerformFormatNoAlarmTest()
        {
            var dr = new Dictionary<string, object>();
            var column = new RowColumnModel
            {
                Value = "OK",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            _formatter.PerformFormat(dr, column, groupStatusColumn, It.IsAny<string>());

            Assert.IsNull(column.BackColor);
            Assert.IsNull(column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatAlarmTest()
        {
            var dr = new Dictionary<string, object>();
            var column = new RowColumnModel
            {
                Value = "Alarm",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            _formatter.PerformFormat(dr, column, groupStatusColumn, It.IsAny<string>());

            Assert.AreEqual("#FF0000", column.BackColor);
            Assert.AreEqual("#FFFFFF", column.ForeColor);
        }

        private void SetupLocalePhrase()
        {
            _phraseStore.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<string>())).Returns((int id, string correlationId) =>
            {
                if (id == 149)
                {
                    return "Alarm";
                }

                if (id == 611)
                {
                    return "OK";
                }

                return null;
            });
        }

        private void SetupMemoryCache()
        {
            _memoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());
        }

    }
}
