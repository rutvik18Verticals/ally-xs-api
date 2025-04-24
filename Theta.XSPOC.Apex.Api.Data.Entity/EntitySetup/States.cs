using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="StatesEntity"/> entity.
    /// </summary>
    public static class States
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

            modelBuilder.Entity<StatesEntity>()
                .HasKey(k => new
                {
                    k.StateId,
                    k.Value
                })
                .IsClustered(false)
                .HasName("PK_tblStates");

            modelBuilder.Entity<StatesEntity>()
                .Property(e => e.StateId)
                .IsRequired(true)
                .HasColumnName("StateID");

            modelBuilder.Entity<StatesEntity>()
                .Property(e => e.Value)
                .IsRequired(true)
                .HasColumnName("Value");
        }

    }
}
