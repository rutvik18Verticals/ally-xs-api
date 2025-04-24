using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="ESPAnalysisResultsEntity"/> entity
    /// </summary>
    public class ESPAnalysisResult
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

            modelBuilder.Entity<ESPAnalysisResultsEntity>()
                .HasKey(k => new
                {
                    k.TestDate,
                    k.NodeId
                })
                .IsClustered(true);

            modelBuilder.Entity<ESPAnalysisResultsEntity>()
                .Property(m => m.Success)
                .HasDefaultValue(false);

            modelBuilder.Entity<ESPAnalysisResultsEntity>()
                .Property(m => m.EnableGasHandling)
                .HasDefaultValue(false);

            modelBuilder.Entity<ESPAnalysisResultsEntity>()
                .Property(m => m.CasingValveClosed)
                .HasDefaultValue(false);

            modelBuilder.Entity<ESPAnalysisResultsEntity>()
                .Property(m => m.UseTVD)
                .HasDefaultValue(false);

            modelBuilder.Entity<ESPAnalysisResultsEntity>()
                .Property(e => e.Id).ValueGeneratedOnAdd();
        }

    }
}
