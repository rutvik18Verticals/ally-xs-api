using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class PhraseRepositorySQLTests : DataStoreTestBase
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
        public void ConstructorNullTest()
        {
            _ = new LocalePhrasesSQLStore(null, _loggerFactory.Object);
        }

        [TestMethod]
        public void GetPhraseByIdMissingTest()
        {
            var phraseData = GetPhraseData();

            var mockPhraseDbSet = SetupMockDbSet(phraseData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.LocalePhrases).Returns(mockPhraseDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var repo = new LocalePhrasesSQLStore(mockFactory.Object, _loggerFactory.Object);

            var result = repo.Get(3, string.Empty);

            Assert.IsNotNull(result);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void GetPhraseByIdTest()
        {
            var phraseData = GetPhraseData();

            var mockPhraseDbSet = SetupMockDbSet(phraseData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.LocalePhrases).Returns(mockPhraseDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var repo = new LocalePhrasesSQLStore(mockFactory.Object, _loggerFactory.Object);

            var result = repo.Get(1, string.Empty);

            Assert.IsNotNull(result);

            Assert.AreEqual("English 1", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindAllAsyncIdListIsNullTest()
        {
            var phraseData = GetPhraseData();

            var mockPhraseDbSet = SetupMockDbSet(phraseData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.LocalePhrases).Returns(mockPhraseDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var repo = new LocalePhrasesSQLStore(mockFactory.Object, _loggerFactory.Object);

            _ = repo.GetAll(string.Empty, null);
        }

        [TestMethod]
        public void FindAllAsyncIdListIsEmptyTest()
        {
            var phraseData = GetPhraseData();

            var mockPhraseDbSet = SetupMockDbSet(phraseData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.LocalePhrases).Returns(mockPhraseDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var repo = new LocalePhrasesSQLStore(mockFactory.Object, _loggerFactory.Object);

            var ids = Array.Empty<int>();

            var result = repo.GetAll(string.Empty, ids);

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Keys.Count);
        }

        [TestMethod]
        public void FindAllAsyncTextEmptyTest()
        {
            var phraseData = GetPhraseData();

            var mockPhraseDbSet = SetupMockDbSet(phraseData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.LocalePhrases).Returns(mockPhraseDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var repo = new LocalePhrasesSQLStore(mockFactory.Object, _loggerFactory.Object);

            var ids = GetIds();
            ids.Add(4);

            var result = repo.GetAll(string.Empty, ids.ToArray());

            Assert.IsNotNull(result);

            Assert.AreEqual(3, result.Keys.Count);
            Assert.AreEqual("English 1", result[1]);
            Assert.AreEqual("English 2", result[2]);
        }

        [TestMethod]
        public void FindAllAsyncEnglishTest()
        {
            var phraseData = GetPhraseData();

            var mockPhraseDbSet = SetupMockDbSet(phraseData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.LocalePhrases).Returns(mockPhraseDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var repo = new LocalePhrasesSQLStore(mockFactory.Object, _loggerFactory.Object);

            var ids = GetIds();

            var result = repo.GetAll(string.Empty, ids.ToArray());

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Keys.Count);
            Assert.AreEqual("English 1", result[1]);
            Assert.AreEqual("English 2", result[2]);
        }

        #endregion

        #region Private Data Setup Methods

        private IQueryable<LocalePhraseEntity> GetPhraseData()
        {
            return new List<LocalePhraseEntity>()
            {
                new LocalePhraseEntity()
                {
                    PhraseId = 1,
                    English = "English 1",
                    Chinese = "Chinese 1",
                    French = "French 1",
                    German = "German 1",
                    Russian = "Russian 1",
                    Spanish = "Spanish 1",
                },
                new LocalePhraseEntity()
                {
                    PhraseId = 2,
                    English = "English 2",
                    Chinese = "Chinese 2",
                    French = "French 2",
                    German = "German 2",
                    Russian = "Russian 2",
                    Spanish = "Spanish 2",
                },
                new LocalePhraseEntity()
                {
                    PhraseId = 4,
                    English = "",
                    Chinese = "Chinese 2",
                    French = "French 2",
                    German = "German 2",
                    Russian = "Russian 2",
                    Spanish = "Spanish 2",
                },
            }.AsQueryable();
        }

        private IList<int> GetIds()
        {
            return new List<int>
            {
                1,
                2,
                3,
            };
        }

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        #endregion

    }
}
