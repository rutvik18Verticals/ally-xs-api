using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="WellTestEntity"/> entity.
    /// </summary>
    public static class WellTest
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

            modelBuilder.Entity<WellTestEntity>()
                .HasKey(k => new
                {
                    k.NodeId,
                    k.TestDate
                });

            modelBuilder.Entity<WellTestEntity>()
                .Property(e => e.NodeId)
                .HasMaxLength(50)
                .IsRequired(true)
                .HasColumnName("NodeID");

            modelBuilder.Entity<WellTestEntity>()
                .Property(e => e.TestDate)
                .IsRequired(true)
                .HasColumnType("datetime")
                .HasColumnName("TestDate");
        }

    }
}
