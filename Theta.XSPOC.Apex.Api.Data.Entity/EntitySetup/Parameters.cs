using System;
using Microsoft.EntityFrameworkCore;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="ParameterEntity"/> entity
    /// </summary>
    public static class Parameters
    {

        /// <summary>
        /// Does additional setup using the <seealso cref="ModelBuilder"/>. Functionality like clustered indexes that
        /// are not supported by DataAnnotations can be built here using fluent syntax.
        /// </summary>
        /// <param name="modelBuilder">The <seealso cref="ModelBuilder"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="modelBuilder"/> is null.
        /// </exception>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<ParameterEntity>()
                .HasKey(x => new { x.Poctype, x.Address })
                .IsClustered(true)
                .HasName("PK_tblParameters");

            modelBuilder.Entity<ParameterEntity>()
                .Property(p => p.FastScan)
                .HasDefaultValue(false);

            modelBuilder.Entity<ParameterEntity>()
                .Property(p => p.UnitType)
                .HasDefaultValue(0);

            modelBuilder.Entity<ParameterEntity>()
                .Property(p => p.ArchiveFunction)
                .HasDefaultValue(1);

            modelBuilder.Entity<ParameterEntity>()
                .Property(p => p.EarliestSupportedVersion)
                .HasDefaultValue(0.0f);
        }

    }
}
