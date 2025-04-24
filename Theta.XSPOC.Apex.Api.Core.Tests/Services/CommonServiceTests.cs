using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class CommonServiceTests
    {

        #region Private Fields

        private ICommonService _commonService;
        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;
        private Mock<ILocalePhrases> _localePhraseStore;
        private Mock<IStates> _statesStore;
        private Mock<ISystemParameter> _systemParameterStore;
        private Mock<IMemoryCache> _memoryCache;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();
            _localePhraseStore = new Mock<ILocalePhrases>();
            _statesStore = new Mock<IStates>();
            _systemParameterStore = new Mock<ISystemParameter>();
            _memoryCache = new Mock<IMemoryCache>();

            SetupThetaLoggerFactory();
            SetupCommonService();
        }

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            _ = new CommonService(null, _localePhraseStore.Object, _statesStore.Object, _systemParameterStore.Object, _memoryCache.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLocalePhraseTest()
        {
            _ = new CommonService(_loggerFactory.Object, null, _statesStore.Object, _systemParameterStore.Object, _memoryCache.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullStatesTest()
        {
            _ = new CommonService(_loggerFactory.Object, _localePhraseStore.Object, null, _systemParameterStore.Object, _memoryCache.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetCardTypeNamePhraseCacheNullTest()
        {
            _localePhraseStore.Setup(x => x.Get(141, It.IsAny<string>())).Returns("Startup");

            IDictionary<int, string> phraseCache = null;
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            _ = _commonService.GetCardTypeName("N", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetCardTypeNameCauseIdPhraseCacheNullTest()
        {
            _localePhraseStore.Setup(x => x.Get(141, It.IsAny<string>())).Returns("Startup");

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = null;

            _ = _commonService.GetCardTypeName("N", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);
        }

        [TestMethod]
        public void GetCardTypeNullTest()
        {
            _localePhraseStore.Setup(x => x.Get(141, It.IsAny<string>())).Returns("Startup");

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName(null, 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual(string.Empty, cardType);
        }

        [TestMethod]
        public void GetCardTypeNameNTest()
        {
            _localePhraseStore.Setup(x => x.Get(141, It.IsAny<string>())).Returns("Startup");

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("N", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("Startup", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameNCachedPhraseTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>()
            {
                {
                    141, "Startup"
                },
            };
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("N", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("Startup", cardType);
        }

        [TestMethod]
        public void GetCardTypeNamePTest()
        {
            _localePhraseStore.Setup(x => x.Get(142, It.IsAny<string>())).Returns("Current");

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("P", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("Current", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameKTest()
        {
            _localePhraseStore.Setup(x => x.Get(6747, It.IsAny<string>())).Returns("PumpoffReference");

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("K", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("Std", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameFTest()
        {
            _localePhraseStore.Setup(x => x.Get(151, It.IsAny<string>())).Returns("Failure");

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("F", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("Failure", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameBTest()
        {
            _localePhraseStore.Setup(x => x.Get(150, It.IsAny<string>())).Returns("Base");

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("B", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("Base", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameLTest()
        {
            _localePhraseStore.Setup(x => x.Get(149, It.IsAny<string>())).Returns("Alarm");

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("L", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("Alarm", cardType);
        }

        [TestMethod]
        public void UnknownCardTypeNameTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("G", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("Unknown", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameSTest()
        {
            _localePhraseStore.Setup(x => x.Get(921, It.IsAny<string>())).Returns("PO/SD");

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("S", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("PO/SD", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameMTest()
        {
            _localePhraseStore.Setup(x => x.Get(1621, It.IsAny<string>())).Returns("S-Mal");

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("M", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("S-Mal", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameMPocType17Test()
        {
            _localePhraseStore.Setup(x => x.Get(1621, It.IsAny<string>())).Returns("S-Mal");
            _statesStore.Setup(x => x.GetCardTypeNamePocType17CardTypeMS(It.IsAny<int>(), It.IsAny<string>())).Returns("TestCardTypeName");

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("M", 99, 17, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("TestCardTypeName", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameMPocType17CachedPhraseTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>()
            {
                {
                    99, "TestCardTypeName"
                },
            };

            var cardType = _commonService.GetCardTypeName("M", 99, 17, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("TestCardTypeName", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameVTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("V", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("SDB-4", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameQTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("Q", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("SDB-4", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameWTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("W", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("SDB-3", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameRTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("R", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("SDB-3", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameXTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("X", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("SDB-2", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameHTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("H", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("SDB-2", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameYTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("Y", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("SDB-1", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameTTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("T", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("SDB-1", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameZTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("Z", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("SDB", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameUTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("U", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("SDB", cardType);
        }

        [TestMethod]
        public void GetCardTypeName1Test()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("1", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("POB-4", cardType);
        }

        [TestMethod]
        public void GetCardTypeName2Test()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("2", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("POB-3", cardType);
        }

        [TestMethod]
        public void GetCardTypeName3Test()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("3", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("POB-2", cardType);
        }

        [TestMethod]
        public void GetCardTypeName4Test()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("4", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("POB-1", cardType);
        }

        [TestMethod]
        public void GetCardTypeName5Test()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("5", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("POB", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameATest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("A", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("StNMPT", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameJTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("J", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("SD Buff", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameOTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("O", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("S-Oper", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameDTest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("D", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("Dyn-C", cardType);
        }

        [TestMethod]
        public void GetCardTypeNameETest()
        {
            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            var cardType = _commonService.GetCardTypeName("E", 99, 8, ref phraseCache, ref causeIdPhraseCache, string.Empty);

            Assert.IsNotNull(cardType);
            Assert.AreEqual("Dyn-U", cardType);
        }

        #endregion

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        private void SetupCommonService()
        {
            _commonService = new CommonService(_loggerFactory.Object, _localePhraseStore.Object, _statesStore.Object,
                _systemParameterStore.Object, _memoryCache.Object);
        }

    }
}
