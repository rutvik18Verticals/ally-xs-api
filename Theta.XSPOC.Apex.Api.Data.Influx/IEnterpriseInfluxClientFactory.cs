using InfluxData.Net.InfluxDb;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Influx
{
    /// <summary>
    /// This is the interface that represents the influx db client factory.
    /// </summary>
    public interface IEnterpriseInfluxClientFactory : IDisposable
    {

        /// <summary>
        /// Creates an <seealso cref="IInfluxDbClient"/> instance.
        /// </summary>
        /// <returns>
        /// An <seealso cref="IInfluxDbClient"/> instance.
        /// </returns>
        IInfluxDbClient Create();

    }
}
