using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class AuthServiceSQLStoreTest
    {
        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new AuthServiceSQLStore(null, null);
        }

        [TestMethod]
        public void AnalysisResultCurvesDataTest()
        {
            var userDefaultData = GetUserDefaultData().AsQueryable();
            var mockuserDefaultDbSet = SetupUserDefaultData(userDefaultData);
            var correlationId = Guid.NewGuid().ToString();

            var userSecurityData = GetUserSecurityData().AsQueryable();
            var mockuserSecurityDataDbSet = SetupUserSecurity(userSecurityData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.UserDefaults).Returns(mockuserDefaultDbSet.Object);
            mockContext.Setup(x => x.UserSecurity).Returns(mockuserSecurityDataDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var authService = new AuthServiceSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var resultByAssetId = authService.FindByNameAsync("Test@123", "password", "correlationId");
            Assert.IsNotNull(resultByAssetId);
            Assert.AreEqual("test@123", resultByAssetId.Email);
        }

        #endregion

        #region Private Methods

        private IList<UserDefaultEntity> GetUserDefaultData()
        {
            var eventData = new List<UserDefaultEntity>()
            {
                new UserDefaultEntity()
                {
                    DefaultsGroup = "Login",
                    Property = "HasLoggedIn",
                    UserId = "Test@123",
                    Value = "false",
                },
                new UserDefaultEntity()
                {
                   DefaultsGroup = "Login",
                    Property = "HasLoggedIn",
                    UserId = "Test@123",
                    Value = "false",
                },
                new UserDefaultEntity()
                {
                    DefaultsGroup = "Login",
                    Property = "AvgDHDSLoad",
                    UserId = "Public",
                    Value = "false",
                },
            };

            return eventData;
        }

        private IList<UserSecurityEntity> GetUserSecurityData()
        {
            var eventData = new List<UserSecurityEntity>()
            {
                new UserSecurityEntity()
                {
                    UserName = "Test@123",
                    WebToken = "true",
                    WebTokenExpire = DateTime.Now,
                    WellAdmin = true,
                    WellControl = true,
                    WellConfig = true,
                    Admin = true,
                    AdminLite = true,
                    WellConfigLite = true,
                    Password = "password",
                    Email = "test@123",
                    MustChangePassword = true,
                },
                new UserSecurityEntity()
                {
                    UserName = "StageUser",
                    WebToken = "true",
                    WebTokenExpire = DateTime.Now,
                    WellAdmin = true,
                    WellControl = true,
                    WellConfig = true,
                    Admin = true,
                    AdminLite = true,
                    WellConfigLite = true,
                    Password = "password",
                    Email = "test@123",
                    MustChangePassword = true,
                },
                new UserSecurityEntity()
                {
                    UserName = "TestUser",
                    WebToken = "true",
                    WebTokenExpire = DateTime.Now,
                    WellAdmin = true,
                    WellControl = true,
                    WellConfig = true,
                    Admin = true,
                    AdminLite = true,
                    WellConfigLite = true,
                    Password = "password",
                    Email = "test@123",
                    MustChangePassword = true,
                },
            };

            return eventData;
        }

        #endregion

        #region Private Setup Methods

        private Mock<DbSet<UserDefaultEntity>> SetupUserDefaultData(IQueryable<UserDefaultEntity> data)
        {
            var mockDbSet = new Mock<DbSet<UserDefaultEntity>>();
            mockDbSet.As<IQueryable<UserDefaultEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<UserDefaultEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<UserDefaultEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<UserDefaultEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<UserSecurityEntity>> SetupUserSecurity(IQueryable<UserSecurityEntity> data)
        {
            var mockDbSet = new Mock<DbSet<UserSecurityEntity>>();
            mockDbSet.As<IQueryable<UserSecurityEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<UserSecurityEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<UserSecurityEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<UserSecurityEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<XspocDbContext> SetupMockContext()
        {
            var contextOptions = new Mock<DbContextOptions<XspocDbContext>>();
            contextOptions.Setup(m => m.ContextType).Returns(typeof(XspocDbContext));
            contextOptions.Setup(m => m.Extensions).Returns(new List<IDbContextOptionsExtension>());

            var mockDateTimeConverter = new Mock<IDateTimeConverter>();
            var mockContext = new Mock<XspocDbContext>(mockDateTimeConverter.Object, contextOptions.Object);

            return mockContext;
        }

        #endregion

    }
}
