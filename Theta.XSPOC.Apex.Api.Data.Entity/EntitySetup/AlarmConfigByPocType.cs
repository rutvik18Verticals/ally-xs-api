using Microsoft.EntityFrameworkCore;
using System;
using Theta.XSPOC.Apex.Api.Data.Entity.Alarms;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="AlarmConfigByPocTypeEntity"/> entity
    /// </summary>
    public static class AlarmConfigByPocType
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

            modelBuilder.Entity<AlarmConfigByPocTypeEntity>()
                .HasKey(k => new { k.PocType, k.Register, k.Bit })
                .IsClustered(false)
                .HasName("PK_tblAlarmConfigByPOCType");

            modelBuilder.Entity<AlarmConfigByPocTypeEntity>()
                .Property(m => m.Priority)
                .HasDefaultValue(0);

            modelBuilder.Entity<AlarmConfigByPocTypeEntity>()
                .Property(m => m.LoLimit)
                .HasDefaultValue(0);

            modelBuilder.Entity<AlarmConfigByPocTypeEntity>()
                .Property(m => m.HiLimit)
                .HasDefaultValue(0);

            modelBuilder.Entity<AlarmConfigByPocTypeEntity>()
                .Property(m => m.AlarmAction)
                .HasDefaultValue(0);
        }

    }
}