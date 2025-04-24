using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="HostAlarmEntity"/>.
    /// </summary>
    public static class HostAlarm
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

            modelBuilder.Entity<HostAlarmEntity>()
                .Property(e => e.AlarmAction).HasDefaultValueSql("((0))");
            modelBuilder.Entity<HostAlarmEntity>()
                .Property(e => e.Enabled).HasDefaultValueSql("((1))");
            modelBuilder.Entity<HostAlarmEntity>()
                .Property(e => e.IgnoreZeroAddress).HasDefaultValueSql("((0))");
            modelBuilder.Entity<HostAlarmEntity>()
                .Property(e => e.NumDays).HasDefaultValueSql("((0))");
        }

    }
}
