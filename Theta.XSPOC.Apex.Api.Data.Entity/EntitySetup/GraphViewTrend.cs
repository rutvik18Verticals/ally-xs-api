using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Represents the setup for the GraphViewTrends entity.
    /// </summary>
    public static class GraphViewTrend
    {

        /// <summary>
        /// Sets up the GraphViewTrend entity in the model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <exception cref="ArgumentNullException">Thrown when the modelBuilder parameter is null.</exception>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<GraphViewTrendsEntity>()
                .HasKey(e => new
                {
                    e.ViewId,
                    e.Source,
                    e.PocType,
                }).IsClustered(false);
        }

    }
}
