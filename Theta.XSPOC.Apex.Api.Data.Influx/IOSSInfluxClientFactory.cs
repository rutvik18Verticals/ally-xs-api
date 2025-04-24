using InfluxDB.Client;

namespace Theta.XSPOC.Apex.Api.Data.Influx
{
    /// <summary>
    /// This is the interface that represents the influx db client factory.
    /// </summary>
    public interface IOSSInfluxClientFactory
    {

        /// <summary>
        /// Creates an <seealso cref="IInfluxDBClient"/> instance.
        /// </summary>
        /// <returns>
        /// An <seealso cref="IInfluxDBClient"/> instance.
        /// </returns>
        IInfluxDBClient Create();

    }
}
