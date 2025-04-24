using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="ParameterEntity"/> entity
    /// </summary>
    public class Parameter
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

            modelBuilder.Entity<ParameterEntity>()
                .HasKey(x => new
                {
                    x.Poctype,
                    x.Address
                });

            modelBuilder.Entity<ParameterEntity>()
                .Property(e => e.Poctype)
                .IsRequired(true)
                .HasColumnName("POCType");

            modelBuilder.Entity<ParameterEntity>()
                .Property(e => e.Address)
                .IsRequired(true)
                .HasColumnName("Address");

            modelBuilder.Entity<ParameterEntity>()
                .Property(m => m.FastScan)
                .HasDefaultValue(false);

            modelBuilder.Entity<ParameterEntity>()
                .Property(m => m.UnitType)
                .IsRequired(true)
                .HasDefaultValue(0);

            modelBuilder.Entity<ParameterEntity>()
                .Property(m => m.ArchiveFunction)
                .IsRequired(true)
                .HasDefaultValue(1);

            modelBuilder.Entity<ParameterEntity>()
                .Property(m => m.EarliestSupportedVersion)
                .IsRequired(true)
                .HasDefaultValue(0f);
        }

    }
}
