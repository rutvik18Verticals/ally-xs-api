using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class UserDefaultsSQLStoreTests
    {

        #region Test Constructors

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserDefaultsSQLStore_NullContext_ThrowsException()
        {
            // Arrange
            _ = new UserDefaultsSQLStore(null, null);

            // Act

            // Assert
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void GetUserDefaults()
        {
            var mockThetaLogger = new Mock<IThetaLoggerFactory>();
            var logger = new Mock<IThetaLogger>();
            mockThetaLogger.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var data = TestUtilities.SetupMockData(new List<UserDefaultEntity>()
            {
                new UserDefaultEntity()
                {
                    DefaultsGroup = "group1",
                    UserId = "test.user1",
                    Property = "Property1",
                    Value = "Value1",
                },
                new UserDefaultEntity()
                {
                    DefaultsGroup = "group1",
                    UserId = "test.user1",
                    Property = "Property2",
                    Value = "Value2",
                },

            }.AsQueryable());
            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.UserDefaults).Returns(data.Object);
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            // Arrange
            var userDefaultsSQLStore = new UserDefaultsSQLStore(contextFactory.Object, mockThetaLogger.Object);

            // Act
            var result = userDefaultsSQLStore.GetItem("test.user1", "Property1", "group1", new Guid(Guid.NewGuid().ToString()).ToString());

            // Assert
            Assert.AreEqual("Value1", result);
        }

        #endregion

    }
}
