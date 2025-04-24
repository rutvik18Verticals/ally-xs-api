using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{

    /// <summary>
    /// Contains helper methods to set up a <seealso cref="PumpingUnitManufacturerEntity"/> entity.
    /// </summary>
    public static class PumpingUnitManufacturer
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

            modelBuilder.Entity<PumpingUnitManufacturerEntity>()
                .HasKey(e => e.Id).IsClustered(false);

            modelBuilder.Entity<PumpingUnitManufacturerEntity>()
                .ToTable("tblPumpingUnitManufacturers");

            modelBuilder.Entity<PumpingUnitManufacturerEntity>()
                .Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            modelBuilder.Entity<PumpingUnitManufacturerEntity>()
                .Property(e => e.ManufacturerAbbreviation).HasMaxLength(255);

            modelBuilder.Entity<PumpingUnitManufacturerEntity>()
                .Property(e => e.UnitTypeId).HasColumnName("UnitTypeID");
        }

    }
}
