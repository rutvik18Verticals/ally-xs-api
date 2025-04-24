using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.WellControl.Data.Services.DbStoreManagers;

namespace Theta.XSPOC.Apex.Api.WellControl.Data.Services.Factories
{
    /// <summary>
    /// Provides an instance of the appropriate <seealso cref="IDbStoreManager"/> to handle a given responsibility.
    /// </summary>
    public class PersistenceFactory : IPersistenceFactory
    {

        #region Private Fields

        private readonly IEnumerable<IDbStoreManager> _dbStoreManagers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="PersistenceFactory"/>.
        /// </summary>
        /// <param name="dbStoreManagers">The list of <see cref="IDbStoreManager"/> that this factory supports.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dbStoreManagers"/> is null.
        /// </exception>
        public PersistenceFactory(IEnumerable<IDbStoreManager> dbStoreManagers)
        {
            _dbStoreManagers = dbStoreManagers ?? throw new ArgumentNullException(nameof(dbStoreManagers));
        }

        #endregion

        #region IPersistenceFactory Implementation

        /// <summary>
        /// Creates the <see cref="IDbStoreManager"/> responsible for handling the <param name="responsibility"></param>.
        /// </summary>
        /// <returns>The <see cref="IDbStoreManager"/> capable of handling that responsibility.</returns>
        /// <exception cref="NotSupportedException">
        /// When the requested 
        /// </exception>
        public IDbStoreManager Create(string responsibility)
        {
            var updater = _dbStoreManagers.FirstOrDefault(x => x.Responsibility == responsibility) ?? throw new NotSupportedException($"{responsibility} is not supported.");
            return updater;
        }

        #endregion

    }
}