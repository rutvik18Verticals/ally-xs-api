using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="CardDataEntity"/> entity.
    /// </summary>
    public static class CardData
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

            modelBuilder.Entity<CardDataEntity>()
                .Property(e => e.CardDate)
                .HasColumnType("datetime");

            modelBuilder.Entity<CardDataEntity>()
                .Property(e => e.NodeId)
                .HasMaxLength(50)
                .HasColumnName("NodeID");

            modelBuilder.Entity<CardDataEntity>()
                .HasKey(k => new
                {
                    k.CardDate,
                    k.NodeId
                })
                .IsClustered(true)
                .HasName("PK_tblCardData_1__10");

            modelBuilder.Entity<CardDataEntity>()
                .HasIndex(e => e.CardDate, "tblcarddata1");

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.Area)
                .HasDefaultValue(0);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.AreaLimit)
                .HasDefaultValue(0);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.CardArea)
                .HasDefaultValue(0);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.CorrectedCard)
                .HasDefaultValue(string.Empty);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.FillBasePercent)
                .HasDefaultValue(0);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.HiLoadLimit)
                .HasDefaultValue(0);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.LoadLimit2)
                .HasDefaultValue((short?)0);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.LoadSpanLimit)
                .HasDefaultValue(0);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.LowLoadLimit)
                .HasDefaultValue(0);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.PocDHCard)
                .HasDefaultValue(string.Empty);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.PositionLimit2)
                .HasDefaultValue((short?)0);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.ProcessCard)
                .HasDefaultValue(0);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.TorquePlotMinimumEnergy)
                .IsUnicode(false)
                .HasDefaultValue(string.Empty);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.TorquePlotMinimumTorque)
                .IsUnicode(false)
                .HasDefaultValue(string.Empty);

            modelBuilder.Entity<CardDataEntity>()
                .Property(m => m.TorquePlotCurrent)
                .IsUnicode(false)
                .HasDefaultValue(string.Empty);

            modelBuilder.Entity<CardDataEntity>()
                .Property(e => e.AnalysisDate)
                .HasColumnType("datetime");

            modelBuilder.Entity<CardDataEntity>()
                .Property(e => e.CardType)
                .HasMaxLength(1)
                .IsUnicode(false);

            modelBuilder.Entity<CardDataEntity>()
                .Property(e => e.CauseId)
                .HasColumnName("CauseID");

            modelBuilder.Entity<CardDataEntity>()
                .Property(e => e.DownHoleCard)
                .IsUnicode(false);

            modelBuilder.Entity<CardDataEntity>()
                .Property(e => e.POCDownholeCard)
                .IsUnicode(false)
                .HasColumnName("POCDownholeCard");

            modelBuilder.Entity<CardDataEntity>()
                .Property(e => e.PocDownHoleCardBinary)
                .HasColumnName("POCDownholeCardB");

            modelBuilder.Entity<CardDataEntity>()
                .Property(e => e.PredictedCard)
                .IsUnicode(false);

            modelBuilder.Entity<CardDataEntity>()
                .Property(e => e.StrokesPerMinute)
                .HasColumnName("SPM");

            modelBuilder.Entity<CardDataEntity>()
                .Property(e => e.SurfaceCard)
                .IsUnicode(false);
        }

    }
}
