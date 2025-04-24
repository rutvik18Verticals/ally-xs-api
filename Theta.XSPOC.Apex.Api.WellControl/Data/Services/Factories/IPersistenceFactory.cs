using Theta.XSPOC.Apex.Api.WellControl.Data.Services.DbStoreManagers;

namespace Theta.XSPOC.Apex.Api.WellControl.Data.Services.Factories
{
    /// <summary>
    /// Provides an instance of the appropriate <seealso cref="IDbStoreManager"/> to handle a given responsibility.
    /// </summary>
    public interface IPersistenceFactory
    {

        /// <summary>
        /// Creates the <see cref="IDbStoreManager"/>.
        /// </summary>
        /// <param name="responsibility">The responsibility (payload type).</param>
        /// <returns>The <see cref="IDbStoreManager"/>.</returns>
        IDbStoreManager Create(string responsibility);

    }
}