using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="CurrentRawScanDataEntity"/> entity.
    /// </summary>
    public static class CurrentRawScanData
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

            modelBuilder.Entity<CurrentRawScanDataEntity>()
                .HasKey(k => new
                {
                    k.NodeId,
                    k.Address
                })
                .IsClustered(false)
                .HasName("PK_tblCurrRawScanData");

            modelBuilder.Entity<CurrentRawScanDataEntity>()
                .Property(m => m.DateTimeUpdated)
                .HasDefaultValue(new DateTime(1970, 1, 1));

            modelBuilder.Entity<CurrentRawScanDataEntity>()
                .HasIndex(e => e.Address, "tblCurrRawScanDataAddress");

            modelBuilder.Entity<CurrentRawScanDataEntity>()
                .HasIndex(e => e.NodeId, "tblNodeMastertblCurrRawScanDat");

            modelBuilder.Entity<CurrentRawScanDataEntity>()
                .Property(e => e.NodeId)
                .HasMaxLength(50)
                .HasColumnName("NodeID");
        }

    }
}
