using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="FacilityTagsEntity"/> entity
    /// </summary>
    public static class FacilityTags
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

            modelBuilder.Entity<FacilityTagsEntity>()
                .HasKey(e => new
                {
                    e.NodeId,
                    e.Address,
                    e.Bit,
                });

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.NodeId)
                .HasMaxLength(100)
                .IsRequired(true)
                .HasColumnName("NodeId");

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.Address)
                .IsRequired(true)
                .HasColumnName("Address");

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.Bit)
                .IsRequired(true)
                .HasColumnName("Bit");

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.AlarmState)
                .HasDefaultValue(0);

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.AlarmAction)
                .HasDefaultValue(0);

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.DataType)
                .HasDefaultValue(2);

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.Decimals)
                .HasDefaultValue(0);

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.Deadband)
                .HasDefaultValue(0f);

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.UnitType)
                .HasDefaultValue(0);

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.CaptureType)
                .HasDefaultValue(0);

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.NumConsecAlarms)
                .HasDefaultValue(0);

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.NumConsecAlarmsAllowed)
                .HasDefaultValue(0);

            modelBuilder.Entity<FacilityTagsEntity>()
                .Property(e => e.ArchiveFunction)
                .HasDefaultValue(1);
        }

    }
}
