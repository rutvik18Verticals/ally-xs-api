using InfluxDB.Client;
using Microsoft.Extensions.Configuration;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Influx
{
    /// <summary>
    /// This is the implementation to create the Influx DbClient factory.
    /// </summary>
    public class OSSInfluxClientFactory : IOSSInfluxClientFactory
    {

        #region Private Fields

        private string _userName;
        private string _password;
        private string _endPoint;
        private string _token;

        #endregion

        #region Protected Fields

        /// <summary>
        /// The <seealso cref="IConfiguration"/> configurations.
        /// </summary>
        protected IConfiguration AppConfig { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="OSSInfluxClientFactory"/> using the provided <paramref name="appConfig"/>.
        /// </summary>
        /// <param name="appConfig">The <seealso cref="IConfiguration"/>.</param>       
        /// <exception cref="ArgumentNullException">
        /// <paramref name="appConfig"/> is null.
        /// </exception> 
        public OSSInfluxClientFactory(IConfiguration appConfig)
        {
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            SetConfigurationOnInit();
        }

        #endregion

        #region IInfluxDbClientFactory Implementation

        /// <summary>
        /// Creates an <seealso cref="IInfluxDBClient"/> instance.
        /// </summary>
        /// <returns>
        /// An <seealso cref="IInfluxDBClient"/> instance.
        /// </returns>
        public IInfluxDBClient Create()
        {
            if (!string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_password)
                && !string.IsNullOrEmpty(_endPoint) && !string.IsNullOrEmpty(_token))
            {
                var options = new InfluxDBClientOptions.Builder()
                .Url(_endPoint)
                    .AuthenticateToken(_token)
                    .Authenticate(_userName, _password.ToCharArray())
                    .TimeOut(TimeSpan.FromMinutes(2))
                    .Build();
                return new InfluxDBClient(options);
            }
            else
            {
                return new InfluxDBClient(_endPoint, _userName, _password);
            }
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
            _token = AppConfig.GetSection("AppSettings:InfluxToken").Value;
        }

        #endregion

    }
}