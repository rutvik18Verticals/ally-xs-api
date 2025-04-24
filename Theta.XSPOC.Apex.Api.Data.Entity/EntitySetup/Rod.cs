using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Represents the setup for the Rod entity.
    /// </summary>
    public static class Rod
    {

        /// <summary>
        /// Sets up the Rod entity in the model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <exception cref="ArgumentNullException">Thrown when the modelBuilder parameter is null.</exception>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<RodEntity>()
                .HasKey(e => new
                {
                    e.NodeId,
                    e.RodNum
                }).IsClustered(false);
        }

    }
}
