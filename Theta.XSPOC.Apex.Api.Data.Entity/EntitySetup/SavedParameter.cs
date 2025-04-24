using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="SavedParameterEntity"/> entity
    /// </summary>
    public class SavedParameter
    {

        /// <summary>
        /// This is the setup method to help build keys, indexes, and default values.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<SavedParameterEntity>()
                .HasKey(e => new
                {
                    e.NodeId,
                    e.Address,
                });

            modelBuilder.Entity<SavedParameterEntity>()
                .Property(e => e.Address)
                .HasMaxLength(4)
                .IsRequired(true);

            modelBuilder.Entity<SavedParameterEntity>()
                .Property(e => e.NodeId)
                .HasMaxLength(100)
                .IsRequired(true);
        }

    }
}
