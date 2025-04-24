using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="CurveSetCoordinatesEntity"/> entity
    /// </summary>
    public class CurveSetCoordinates
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

            modelBuilder.Entity<CurveSetCoordinatesEntity>()
                .Property(e => e.Id)
                .IsRequired(true);

            modelBuilder.Entity<CurveSetCoordinatesEntity>()
                .Property(e => e.X)
                .IsRequired(true);

            modelBuilder.Entity<CurveSetCoordinatesEntity>()
                .Property(e => e.Y)
                .IsRequired(true);

            modelBuilder.Entity<CurveSetCoordinatesEntity>()
                .Property(e => e.CurveId)
                .IsRequired(true);
        }

    }
}
