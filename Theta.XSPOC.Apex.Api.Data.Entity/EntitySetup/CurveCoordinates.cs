using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="CurveCoordinatesEntity"/> entity
    /// </summary>
    public class CurveCoordinates
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

            modelBuilder.Entity<CurveCoordinatesEntity>()
                .Property(e => e.Id)
                .IsRequired(true);

            modelBuilder.Entity<CurveCoordinatesEntity>()
                .Property(e => e.X)
                .IsRequired(true);

            modelBuilder.Entity<CurveCoordinatesEntity>()
                .Property(e => e.Y)
                .IsRequired(true);

            modelBuilder.Entity<CurveCoordinatesEntity>()
                .Property(e => e.CurveId)
                .IsRequired(true);
        }

    }
}
