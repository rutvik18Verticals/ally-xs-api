using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {

        #region Test Constructors

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserService_NullAuthService_ThrowsException()
        {
            // Arrange
            _ = new UserService(null, null);

            // Act

            // Assert
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void IsUserFirstTimeLogin_NullUsername_ThrowsException()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var userService = new UserService(new Mock<IUserDefaultStore>().Object, mockThetaLoggerFactory.Object);
            // Act
            void action()
            {
                userService.IsUserFirstTimeLogin(null, new Guid(Guid.NewGuid().ToString()).ToString());
            }

            // Assert
            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void IsUserFirstTimeLogin_EmptyUsername_ThrowsException()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var userService = new UserService(new Mock<IUserDefaultStore>().Object, mockThetaLoggerFactory.Object);

            // Act
            void action()
            {
                userService.IsUserFirstTimeLogin(string.Empty, new Guid(Guid.NewGuid().ToString()).ToString());
            }

            // Assert
            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void IsUserFirstTimeLogin_WhitespaceUsername_ThrowsException()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var userService = new UserService(new Mock<IUserDefaultStore>().Object, mockThetaLoggerFactory.Object);

            // Act
            void action()
            {
                userService.IsUserFirstTimeLogin(" ", new Guid(Guid.NewGuid().ToString()).ToString());
            }

            // Assert
            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void IsUserFirstTimeLogin_ValidUsername_Returns_AlreadyLoggedIn()
        {
            // Arrange
            var userDefaultStore = new Mock<IUserDefaultStore>();
            userDefaultStore.Setup(u => u.GetItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns("1");
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var userService = new UserService(userDefaultStore.Object, mockThetaLoggerFactory.Object);

            // Act
            var result = userService.IsUserFirstTimeLogin("username", new Guid(Guid.NewGuid().ToString()).ToString());

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsUserFirstTimeLogin_ValidUsername_Returns_NotLoggedInYet()
        {
            // Arrange
            var userDefaultStore = new Mock<IUserDefaultStore>();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            userDefaultStore.Setup(u => u.GetItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns((string)null);
            var userService = new UserService(userDefaultStore.Object, mockThetaLoggerFactory.Object);

            // Act
            var result = userService.IsUserFirstTimeLogin("username", new Guid(Guid.NewGuid().ToString()).ToString());

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetUserLoggedIn_NullUsername_ThrowsException()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var userService = new UserService(new Mock<IUserDefaultStore>().Object, mockThetaLoggerFactory.Object);

            // Act
            void action()
            {
                userService.SetUserLoggedIn(null, string.Empty);
            }

            // Assert
            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void SetUserLoggedIn_EmptyUsername_ThrowsException()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var userService = new UserService(new Mock<IUserDefaultStore>().Object, mockThetaLoggerFactory.Object);

            // Act
            void action()
            {
                userService.SetUserLoggedIn(string.Empty, string.Empty);
            }

            // Assert
            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void SetUserLoggedIn_WhitespaceUsername_ThrowsException()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var userService = new UserService(new Mock<IUserDefaultStore>().Object, mockThetaLoggerFactory.Object);

            // Act
            void action()
            {
                userService.SetUserLoggedIn(" ", string.Empty);
            }

            // Assert
            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void SetUserLoggedIn_ValidUsername_ReturnsValue()
        {
            // Arrange
            var userDefaultStore = new Mock<IUserDefaultStore>();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            userDefaultStore.Setup(u => u.SetItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                )).Returns(true);
            var userService = new UserService(userDefaultStore.Object, mockThetaLoggerFactory.Object);

            // Act
            var result = userService.SetUserLoggedIn("username", string.Empty);

            // Assert            
            Assert.IsTrue(result);
            userDefaultStore.Verify(u => u.SetItem("username", "HasLoggedIn", "Login", "1", It.IsAny<string>()), Times.Once);
        }

        #endregion

    }
}
