using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// A database context for the XSPOC database which executes queries with NOLOCK (dirty read; uncommitted read).
    /// </summary>
    public class NoLockXspocDbContext : XspocDbContext
    {

        #region Private Fields

        private readonly IDbConnectionInterceptor _noLockDbConnectionInterceptor;

        #endregion

        #region Constructors

        /// <summary>
        /// The constructor that creates a new instance of the <seealso cref="NoLockXspocDbContext"/>.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="noLockDbConnectionInterceptor">The no lock db connection interceptor</param>
        /// <param name="dateTimeConverter"></param>
        public NoLockXspocDbContext(DbContextOptions<NoLockXspocDbContext> options,
            IDbConnectionInterceptor noLockDbConnectionInterceptor, IDateTimeConverter dateTimeConverter) : base(dateTimeConverter, options)
        {
            _noLockDbConnectionInterceptor = noLockDbConnectionInterceptor
                                             ?? throw new ArgumentNullException(nameof(noLockDbConnectionInterceptor));
        }

        #endregion

        #region Overrides

        /// <summary>
        /// This method is overriden in order add the necessary interceptors for enabling no lock querying.
        /// </summary>
        /// <param name="optionsBuilder">The db context options builder.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }

            optionsBuilder.AddInterceptors(_noLockDbConnectionInterceptor);
        }

        #endregion

    }
}
