using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;

namespace Theta.XSPOC.Apex.Api.Core.Tests.GroupStatusColumnFormatter
{
    [TestClass]
    public class TISColumnFormatterTests
    {

        #region Private Fields

        private TISColumnFormatter _formatter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _formatter = new TISColumnFormatter();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateValueNullColumnTest()
        {
            _formatter.CalculateValue(new Dictionary<string, object>(), null, string.Empty);
        }

        [TestMethod]
        public void CalculateValueTest()
        {
            var dr = new Dictionary<string, object>();
            dr.Add("TIS", "7985");
            dr.Add("Enabled", "True");

            var column = new RowColumnModel();

            _formatter.CalculateValue(dr, column, string.Empty);

            Assert.AreEqual("5.5 d", column.Value);
        }

        [TestMethod]
        public void CalculateNullTest()
        {
            var dr = new Dictionary<string, object>();
            dr.Add("TIS", "");
            dr.Add("Enabled", "True");

            var column = new RowColumnModel();

            _formatter.CalculateValue(dr, column, string.Empty);

            Assert.AreEqual("", column.Value);
        }

    }
}
