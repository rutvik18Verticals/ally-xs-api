using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class LocalePhraseTests
    {

        #region Private Fields

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();

            SetupThetaLoggerFactory();
        }

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LocalePhraseNullContextFactoryTest()
        {
            _ = new LocalePhrasesSQLStore(null, _loggerFactory.Object);
        }

        [TestMethod]
        public void LocalePhraseConstructorTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var objLocalePhrase = new LocalePhrasesSQLStore(contextFactory.Object, _loggerFactory.Object);
            Assert.IsNotNull(objLocalePhrase);
        }

        [TestMethod]
        public void GetLocalePhraseTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.LocalePhrases)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetLocalePhraseData().AsQueryable()).Object);

            var objLocalePhrase = new LocalePhrasesSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = objLocalePhrase.Get(7048, string.Empty);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual(result,
                "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil.");
        }

        [TestMethod]
        public void GetLocalePhraseGetAllTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.LocalePhrases)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetLocalePhraseData().AsQueryable()).Object);

            var objLocalePhrase = new LocalePhrasesSQLStore(contextFactory.Object, _loggerFactory.Object);
            int[] phraseid = new int[]
            {
                7048
            };
            var result = objLocalePhrase.GetAll(string.Empty, phraseid);

            Assert.IsNotNull(result);
        }

        #endregion

        #region Private Methods

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        #endregion

    }
}
