using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="PumpingUnitsEntity"/> entity.
    /// </summary>
    public class PumpingUnits
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

            modelBuilder.Entity<PumpingUnitsEntity>()
                .HasIndex(k => k.Id)
                .IsClustered(false)
                .IsUnique(true)
                .HasDatabaseName("UX_tblPumpingUnits_Id");

            modelBuilder.Entity<PumpingUnitsEntity>()
                .HasKey(e => e.UnitId);

            modelBuilder.Entity<PumpingUnitsEntity>()
                .ToTable("tblPumpingUnits");

            modelBuilder.Entity<PumpingUnitsEntity>()
                .HasIndex(e => e.Id, "UX_tblPumpingUnits_Id").IsUnique();

            modelBuilder.Entity<PumpingUnitsEntity>()
                .Property(e => e.UnitId)
                .HasMaxLength(255)
                .HasColumnName("UnitID");

            modelBuilder.Entity<PumpingUnitsEntity>()
                .Property(e => e.APIDesignation)
                .HasMaxLength(255)
                .HasColumnName("APIDesignation");

            modelBuilder.Entity<PumpingUnitsEntity>()
                .Property(e => e.ManufacturerId)
                .HasMaxLength(255)
                .HasColumnName("ManufID");

            modelBuilder.Entity<PumpingUnitsEntity>()
                .Property(e => e.OtherInfo).HasMaxLength(255);

            modelBuilder.Entity<PumpingUnitsEntity>()
                .Property(e => e.UnitName).HasMaxLength(255);

            modelBuilder.Entity<PumpingUnitsEntity>()
                .Property(e => e.WV_Make)
                .HasMaxLength(80)
                .HasColumnName("WV_Make");

            modelBuilder.Entity<PumpingUnitsEntity>()
                .Property(e => e.WV_Model)
                .HasMaxLength(80)
                .HasColumnName("WV_Model");

            modelBuilder.Entity<PumpingUnitsEntity>()
                .Property(e => e.WV_OtherInfo)
                .HasMaxLength(80)
                .HasColumnName("WV_OtherInfo");

            modelBuilder.Entity<PumpingUnitsEntity>()
                .Property(e => e.WV_Type)
                .HasMaxLength(80)
                .HasColumnName("WV_Typ");
        }

    }
}
