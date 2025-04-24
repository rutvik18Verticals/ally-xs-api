using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="WellDetailsEntity"/> entity.
    /// </summary>
    public static class WellDetails
    {

        /// <summary>
        /// Does additional setup using the <seealso cref="ModelBuilder"/>. Functionality like clustered indexes that
        /// are not supported by DataAnnotations can be built here using fluent syntax.
        /// </summary>
        /// <param name="modelBuilder">The <seealso cref="ModelBuilder"/>.</param>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.WaterCut)
                .HasDefaultValue(0f);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.WaterSpecificGravity)
                .HasDefaultValue(1.06f);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.PumpType)
                .HasDefaultValue('R');

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.TotalRodLength)
                .HasDefaultValue(0);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.MaximumCounterBalanceMoment)
                .HasDefaultValue(0);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.PowerMeterType)
                .HasDefaultValue('1');

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.XDGFileAvailable)
                .HasDefaultValue(false);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.DepthOfTopmostPerforation)
                .HasDefaultValue(0);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.DepthOfBottommostPerforation)
                .HasDefaultValue(0);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.FluidTemperature)
                .HasDefaultValue(0);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.ValveCheckStandingValveLoad)
                .HasDefaultValue(0);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.ValveCheckTravelingValveLoad)
                .HasDefaultValue(0);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.ValveCheckLeakage)
                .HasDefaultValue(0);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.CounterBalanceDataType)
                .HasDefaultValue(0);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.CounterBalanceCrankAngle)
                .HasDefaultValue(90);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.PumpIntakePressureSensorInstalled)
                .HasDefaultValue(false);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.IsCasingValveClosed)
                .HasDefaultValue(false);

            modelBuilder.Entity<WellDetailsEntity>()
                .Property(m => m.HasPacker)
                .HasDefaultValue(false);
        }

    }
}
