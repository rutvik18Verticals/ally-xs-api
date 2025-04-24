using System;
using Microsoft.EntityFrameworkCore;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{

    /// <summary>
    /// Contains helper methods to set up a <seealso cref="POCTypeEntity"/> entity
    /// </summary>
    public static class PocTypes
    {

        /// <summary>
        /// This is the setup method to help build keys, indexes, and default values.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="modelBuilder"/> is null.
        /// </exception>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<POCTypeEntity>()
                .Property(m => m.Enabled)
                .HasDefaultValue(false);

            modelBuilder.Entity<POCTypeEntity>()
                .Property(m => m.IsMaster)
                .HasDefaultValue(false);
        }

    }
}