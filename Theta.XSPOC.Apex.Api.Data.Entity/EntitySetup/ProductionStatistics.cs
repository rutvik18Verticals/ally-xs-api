using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="ProductionStatisticsEntity"/> entity.
    /// </summary>
    public class ProductionStatistics
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

            modelBuilder.Entity<ProductionStatisticsEntity>()
                .HasKey(k => new
                {
                    k.NodeId,
                    k.ProcessedDate
                })
                .HasName("PK__tblProdu__EC7D0B6DF95C06F0")
                .IsClustered(false);

            modelBuilder.Entity<ProductionStatisticsEntity>()
                .Property(e => e.NodeId)
                .HasMaxLength(50)
                .IsRequired(true)
                .HasColumnName("NodeID");

            modelBuilder.Entity<ProductionStatisticsEntity>()
                .Property(e => e.ProcessedDate)
                .IsRequired(true)
                .HasColumnType("datetime")
                .HasColumnName("ProcessedDate");

        }
    }
}
