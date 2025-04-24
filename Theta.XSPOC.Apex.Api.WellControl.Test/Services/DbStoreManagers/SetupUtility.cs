using Microsoft.Extensions.Configuration;
using Moq;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.WellControl.Test.Services.DbStoreManagers
{
    /// <summary>
    /// Common setup methods, mostly for setting up Mocks, that can be re-used in unit tests.
    /// </summary>
    public static class SetupUtility
    {

        /// <summary>
        /// Sets up a Mock of IThetaLoggerFactory, using Moq, that operates on the Mocked <param name="logger."></param>
        /// </summary>
        /// <param name="logger">A Mock instance of a logger that can be used in Assert statements.</param>
        /// <param name="loggerFactory">
        /// A Mock instance of <seealso cref="IThetaLoggerFactory"/> that can be used when constructing the system
        /// under test, in order to be able to verify the logging produced by it.
        /// </param>
        public static void CreateMockLoggerFactory(out Mock<IThetaLogger> logger,
            out Mock<IThetaLoggerFactory> loggerFactory)
        {
            logger = new Mock<IThetaLogger>();
            loggerFactory = new Mock<IThetaLoggerFactory>();
            loggerFactory.Setup(m => m.Create(LoggingModel.MongoDataStore))
                .Returns(logger.Object);
        }

        /// <summary>
        /// Sets up a Mock of IConfiguration, using Moq, that operates on the Mocked.
        /// </summary>
        public static IConfiguration GetMockMongoDbConfiguration()
        {
            Mock<IConfigurationSection> mockNumberOfRetries = new Mock<IConfigurationSection>();
            mockNumberOfRetries.Setup(x => x.Value).Returns("2");

            Mock<IConfigurationSection> mockRetryInterval = new Mock<IConfigurationSection>();
            mockRetryInterval.Setup(x => x.Value).Returns("1000");

            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "MongodbSettings:Retries")))
                .Returns(mockNumberOfRetries.Object);
            mockConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "MongodbSettings:IntervalMilliseconds")))
                .Returns(mockRetryInterval.Object);

            return mockConfig.Object;
        }

    }
}
