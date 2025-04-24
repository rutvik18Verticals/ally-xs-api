using InfluxData.Net.Common.Enums;
using InfluxData.Net.InfluxDb;
using System;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Theta.XSPOC.Apex.Api.Data.Influx
{
    /// <summary>
    /// This is the implementation to create the Influx DbClient factory.
    /// </summary>
    public class EnterpriseInfluxClientFactory : IEnterpriseInfluxClientFactory
    {

        #region Private Fields

        private string _userName;
        private string _password;
        private string _endPoint;

        #endregion

        #region Protected Fields

        /// <summary>
        /// The <seealso cref="IConfiguration"/> configurations.
        /// </summary>
        protected IConfiguration AppConfig { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="EnterpriseInfluxClientFactory"/> using the provided <paramref name="appConfig"/>.
        /// </summary>
        /// <param name="appConfig">The <seealso cref="IConfiguration"/>.</param>       
        /// <exception cref="ArgumentNullException">
        /// <paramref name="appConfig"/> is null.
        /// </exception> 
        public EnterpriseInfluxClientFactory(IConfiguration appConfig)
        {
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            SetConfigurationOnInit();
        }

        #endregion

        #region IInfluxDbClientFactory Implementation

        /// <summary>
        /// Creates an <seealso cref="IInfluxDbClient"/> instance.
        /// </summary>
        /// <returns>
        /// An <seealso cref="IInfluxDbClient"/> instance.
        /// </returns>
        public IInfluxDbClient Create()
        {
            return new InfluxDbClient(_endPoint, _userName, _password, InfluxDbVersion.Latest);
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Set any additional configuration that needs to be done during the initialize process
        /// such as the Deployment Mode, Bucket, Org, EndPoint, User Password Or Token and Bucket Retention.
        /// </summary>
        private void SetConfigurationOnInit()
        {
            _userName = AppConfig.GetSection("AppSettings:InfluxUser").Value;
            _password = AppConfig.GetSection("AppSettings:InfluxPassword").Value;
            _endPoint = AppConfig.GetSection("AppSettings:InfluxEndPoint").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}