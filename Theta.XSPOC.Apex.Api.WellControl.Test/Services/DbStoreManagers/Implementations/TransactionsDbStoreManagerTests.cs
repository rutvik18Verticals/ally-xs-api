using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.WellControl.Data.Services.DbStoreManagers.Implementations;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.WellControl.Test.Services.DbStoreManagers.Implementations
{
    [TestClass]
    public class TransactionsDbStoreManagerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullTransactionDbStoreGuardTest()
        {
            _ = new TransactionsDbStoreManager(null, new Mock<IThetaLoggerFactory>().Object, new Mock<IConfiguration>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggingFactoryTest()
        {
            _ = new TransactionsDbStoreManager(new Mock<ITransactionsDbStore>().Object, null, new Mock<IConfiguration>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullConfigurationTest()
        {
            _ = new TransactionsDbStoreManager(new Mock<ITransactionsDbStore>().Object, new Mock<IThetaLoggerFactory>().Object, null);
        }

        [TestMethod]
        public void ResponsibilityTest()
        {
            var manager =
                new TransactionsDbStoreManager(new Mock<ITransactionsDbStore>().Object, new Mock<IThetaLoggerFactory>().Object, new Mock<IConfiguration>().Object);

            Assert.AreEqual("tblTransactions", manager.Responsibility);
        }

        #endregion

    }
}
