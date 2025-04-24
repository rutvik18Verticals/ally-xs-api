using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Security;
using Theta.XSPOC.Apex.Kernel.Logging;
using User = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Security.User;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class AuthServiceMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new AuthServiceMongoStore(null, null);
        }

        [TestMethod]
        public void FindByNameAsyncTest()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockUserCollection = new Mock<IMongoCollection<User>>();
            mockUserCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<User>>(),
                It.IsAny<FindOptions<User>>(), It.IsAny<CancellationToken>()))
                .Returns(GetMockMongoData<User>("User"));

            var mockRoleCollection = new Mock<IMongoCollection<Role>>();
            mockRoleCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Role>>(),
                It.IsAny<FindOptions<Role>>(), It.IsAny<CancellationToken>()))
                .Returns(GetMockMongoData<Role>("Role"));

            var mockUserRoleCollection = new Mock<IMongoCollection<UserRole>>();
            mockUserRoleCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<UserRole>>(),
                It.IsAny<FindOptions<UserRole>>(), It.IsAny<CancellationToken>()))
                .Returns(GetMockMongoData<UserRole>("UserRole"));

            var mockUserPrefCollection = new Mock<IMongoCollection<UserPreference>>();
            mockUserPrefCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<UserPreference>>(),
                It.IsAny<FindOptions<UserPreference>>(), It.IsAny<CancellationToken>()))
                .Returns(GetMockMongoData<UserPreference>("UserPreference"));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<User>("User", null))
                .Returns(mockUserCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Role>("Role", null))
                .Returns(mockRoleCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<UserRole>("UserRole", null))
                .Returns(mockUserRoleCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<UserPreference>("UserPreference", null))
                .Returns(mockUserPrefCollection.Object);

            var dataStore = new AuthServiceMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            // Act
            var result = dataStore.FindByNameAsync("test", "test", "correlationId");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.UserName, "test");
            Assert.IsTrue(result.Admin);
            Assert.IsFalse(result.AdminLite);
            Assert.IsFalse(result.WellAdmin);
            Assert.IsTrue(result.WellConfig);
            Assert.IsFalse(result.WellConfigLite);
            Assert.IsTrue(result.WellControl);
            Assert.IsFalse(result.IsFirstTimeLogin);
        }

        #endregion

        #region

        public static IAsyncCursor<T> GetMockMongoData<T>(string collectionName)
        {
            var result = new Mock<IAsyncCursor<T>>();
            switch (collectionName)
            {
                case "User":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetUserData());
                    break;
                case "Role":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetRoleData());
                    break;
                case "UserRole":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetUserRoleData());
                    break;
                case "UserPreference":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetUserPreferencesData());
                    break;
                default:
                    break;
            }
            result.SetupSequence(m => m.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            return result.Object;
        }

        private static IEnumerable<UserRole> GetUserRoleData()
        {
            return new List<UserRole>
            {
                new()
                {
                    Id = "1",
                    UserId = "1",
                    RoleIds = new List<string> { "1", "2", "3" }
                }
            };
        }

        private static IEnumerable<UserPreference> GetUserPreferencesData()
        {
            return new List<UserPreference>
            {
                new()
                {
                    Id = "1",
                    UserId = "1",
                    PreferenceItem = new Preference
                    {
                        GroupName = "Login",
                        PropertyItem = new List<Property>
                        {
                            new()
                            {
                                PropertyName = "HasLoggedIn",
                                Value = "1"
                            }
                        }
                    }
                }
            };
        }

        private static IEnumerable<Role> GetRoleData()
        {
            return new List<Role>
            {
                new()
                {
                    Id = "1",
                    RoleName = "Well Control",
                },
                new()
                {
                    Id = "2",
                    RoleName = "Admin",
                },
                new()
                {
                    Id = "3",
                    RoleName = "Well Config",
                },
                new()
                {
                    Id = "4",
                    RoleName = "Admin Lite",
                },
                new()
                {
                    Id = "5",
                    RoleName = "Well Admin",
                },
                new()
                {
                    Id = "5",
                    RoleName = "Well Config Lite",
                },
            };
        }

        private static IEnumerable<User> GetUserData()
        {
            return new List<User>
            {
                    new()
                    {
                        Id = "1",
                        UserName = "test",
                        Password = "test",
                        IsEnabled = true,
                    }
            };
        }

        #endregion

    }
}
