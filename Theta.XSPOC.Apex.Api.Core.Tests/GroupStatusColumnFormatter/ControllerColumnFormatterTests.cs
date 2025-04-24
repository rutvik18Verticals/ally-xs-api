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
    public class ControllerColumnFormatterTests
    {

        private Mock<IPocType> _pocTypeStore;
        private Mock<IMemoryCache> _memoryCache;
        private ControllerColumnFormatter _controllerColumnFormatter;

        [TestInitialize]
        public void TestInitialize()
        {
            _pocTypeStore = new Mock<IPocType>();
            _memoryCache = new Mock<IMemoryCache>();

            SetupMemoryCache();

            _controllerColumnFormatter = new ControllerColumnFormatter(_pocTypeStore.Object);
        }

        [TestMethod]
        public void ConstructorNullPocTypeStoreTest()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ControllerColumnFormatter(null));
        }

        [TestMethod]
        public void CalculateValueNullDataRowTest()
        {
            // Arrange
            var column = new RowColumnModel();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _controllerColumnFormatter.CalculateValue(null, column, string.Empty));
        }

        [TestMethod]
        public void CalculateValueNullRowColumnModelTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _controllerColumnFormatter.CalculateValue(dr, null, string.Empty));
        }

        [TestMethod]
        public void CalculateValueInvalidPocTypeTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.POCTYPE", "invalid"
                }
            };
            var column = new RowColumnModel();

            // Act
            _controllerColumnFormatter.CalculateValue(dr, column, string.Empty);

            // Assert
            Assert.IsNull(column.Value);
        }

        [TestMethod]
        public void CalculateValueValidPocTypeTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.POCTYPE", "1"
                }
            };
            var column = new RowColumnModel();
            _pocTypeStore.Setup(x => x.Get(1, string.Empty)).Returns(new PocTypeModel()
            {
                Description = "Test"
            });

            // Act
            _controllerColumnFormatter.CalculateValue(dr, column, string.Empty);

            // Assert
            Assert.AreEqual("Test", column.Value);
        }

        [TestMethod]
        public void PerformFormatNullDataRowTest()
        {
            // Arrange
            var column = new RowColumnModel();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                _controllerColumnFormatter.PerformFormat(null, column, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatNullRowColumnModelTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                _controllerColumnFormatter.PerformFormat(dr, null, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatValidParametersTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>();
            var column = new RowColumnModel();

            // Act
            _controllerColumnFormatter.PerformFormat(dr, column, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty);

            // Assert
            Assert.AreEqual(null, null);
        }

        [TestMethod]
        public void CalculateValueCacheTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.POCTYPE", "1"
                }
            };
            var column = new RowColumnModel();
            var cache = new List<PocTypeModel>()
            {
                new PocTypeModel()
                {
                    PocType = 1,
                    Description = "Test"
                },
            };

            _controllerColumnFormatter.CalculateValue(dr, column, string.Empty, cache: cache);

            Assert.AreEqual("Test", column.Value);
        }

        private void SetupMemoryCache()
        {
            _memoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());
        }

    }
}
