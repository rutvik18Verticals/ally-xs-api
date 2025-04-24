using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class UserDefaultRepositorySQLTests : DataStoreTestBase
    {
        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;

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
            _ = new UserDefaultSQLStore(null, null);
        }

        [TestMethod]
        public async Task GetDefaultItemUserIdEmptyTest()
        {
            var dbContext = SetupMockContext();
            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var repo = new UserDefaultSQLStore(mockFactory.Object, mockThetaLoggerFactory.Object);

            var result = await repo.GetDefaultItem(string.Empty, "Group", "correlationId", "property");

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetDefaultItemUserIdNullTest()
        {
            var dbContext = SetupMockContext();
            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new UserDefaultSQLStore(mockFactory.Object, mockThetaLoggerFactory.Object);

            var result = await repo.GetDefaultItem(null, "Group", "property");

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetDefaultItemPropertyIsNullTest()
        {
            var userDefaultData = GetUserDefaultData();

            var mockDbSet = SetupMockDbSetAsync(userDefaultData);

            var dbContext = SetupMockContext();
            dbContext.Setup(m => m.UserDefaults).Returns(mockDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new UserDefaultSQLStore(mockFactory.Object, _loggerFactory.Object);

            var result = await repo.GetDefaultItem("user1", "group1", null);

            Assert.IsNotNull(result);

            Assert.AreEqual("1", result.Value);
            Assert.AreEqual("user1", result.UserId);
            Assert.AreEqual("property1", result.Property);
            Assert.AreEqual("group1", result.DefaultsGroup);
        }

        [TestMethod]
        public async Task GetDefaultItemPropertyIsSetTest()
        {
            var userDefaultData = GetUserDefaultData();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockDbSet = SetupMockDbSetAsync(userDefaultData);

            var dbContext = SetupMockContext();
            dbContext.Setup(m => m.UserDefaults).Returns(mockDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new UserDefaultSQLStore(mockFactory.Object, mockThetaLoggerFactory.Object);

            var result = await repo.GetDefaultItem("user1", "group1", "correlationId", "property2");

            Assert.IsNotNull(result);

            Assert.AreEqual("1", result.Value);
            Assert.AreEqual("user1", result.UserId);
            Assert.AreEqual("property2", result.Property);
            Assert.AreEqual("group1", result.DefaultsGroup);
        }

        [TestMethod]
        public async Task GetDefaultItemByGroupUserNullTest()
        {
            var userDefaultData = GetUserDefaultData();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockDbSet = SetupMockDbSetAsync(userDefaultData);

            var dbContext = SetupMockContext();
            dbContext.Setup(m => m.UserDefaults).Returns(mockDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new UserDefaultSQLStore(mockFactory.Object, mockThetaLoggerFactory.Object);

            var result = await repo.GetDefaultItemByGroup(null, "group1", new Guid(Guid.NewGuid().ToString()).ToString());

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Keys.Count);
        }

        [TestMethod]
        public async Task GetDefaultItemByGroupUserIsEmptyTest()
        {
            var userDefaultData = GetUserDefaultData();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockDbSet = SetupMockDbSetAsync(userDefaultData);

            var dbContext = SetupMockContext();
            dbContext.Setup(m => m.UserDefaults).Returns(mockDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new UserDefaultSQLStore(mockFactory.Object, mockThetaLoggerFactory.Object);

            var result = await repo.GetDefaultItemByGroup(string.Empty, "group1", new Guid(Guid.NewGuid().ToString()).ToString());

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Keys.Count);
        }

        [TestMethod]
        public async Task GetDefaultItemByGroupNotFoundTest()
        {
            var userDefaultData = GetUserDefaultData();

            var mockDbSet = SetupMockDbSetAsync(userDefaultData);

            var dbContext = SetupMockContext();
            dbContext.Setup(m => m.UserDefaults).Returns(mockDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new UserDefaultSQLStore(mockFactory.Object, _loggerFactory.Object);

            var result = await repo.GetDefaultItemByGroup("user1", "not found", new Guid(Guid.NewGuid().ToString()).ToString());

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Keys.Count);
        }

        [TestMethod]
        public async Task GetDefaultItemByGroupUserNotFoundTest()
        {
            var userDefaultData = GetUserDefaultData();

            var mockDbSet = SetupMockDbSetAsync(userDefaultData);

            var dbContext = SetupMockContext();
            dbContext.Setup(m => m.UserDefaults).Returns(mockDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new UserDefaultSQLStore(mockFactory.Object, _loggerFactory.Object);

            var result = await repo.GetDefaultItemByGroup("not found", "group1", new Guid(Guid.NewGuid().ToString()).ToString());

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Keys.Count);
        }

        [TestMethod]
        public async Task GetDefaultItemByGroupTest()
        {
            var userDefaultData = GetUserDefaultData();

            var mockDbSet = SetupMockDbSetAsync(userDefaultData);

            var dbContext = SetupMockContext();
            dbContext.Setup(m => m.UserDefaults).Returns(mockDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new UserDefaultSQLStore(mockFactory.Object, _loggerFactory.Object);

            var result = await repo.GetDefaultItemByGroup("user1", "group1", new Guid(Guid.NewGuid().ToString()).ToString());

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Keys.Count);

            Assert.AreEqual("1", result["property1"].Value);
            Assert.AreEqual("property1", result["property1"].Property);
            Assert.AreEqual("user1", result["property1"].UserId);
            Assert.AreEqual("group1", result["property1"].DefaultsGroup);

            Assert.AreEqual("1", result["property2"].Value);
            Assert.AreEqual("property2", result["property2"].Property);
            Assert.AreEqual("user1", result["property2"].UserId);
            Assert.AreEqual("group1", result["property2"].DefaultsGroup);
        }

        #endregion

        #region Private Data Setup Methods

        private IQueryable<UserDefaultEntity> GetUserDefaultData()
        {
            return new List<UserDefaultEntity>()
            {
                new UserDefaultEntity()
                {
                    Value = "1",
                    UserId = "user1",
                    Property = "property1",
                    DefaultsGroup = "group1",
                },
                new UserDefaultEntity()
                {
                    Value = "1",
                    UserId = "user1",
                    Property = "property2",
                    DefaultsGroup = "group1",
                },
                new UserDefaultEntity()
                {
                    Value = "0",
                    UserId = "user2",
                    Property = "property1",
                    DefaultsGroup = "group1",
                },
                new UserDefaultEntity()
                {
                    Value = "1",
                    UserId = "user1",
                    Property = "property1",
                    DefaultsGroup = "group2",
                },
            }.AsQueryable();
        }

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        #endregion

    }
}