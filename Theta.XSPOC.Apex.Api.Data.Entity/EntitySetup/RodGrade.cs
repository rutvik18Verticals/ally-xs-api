using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Represents the setup for the RodGrade entity.
    /// </summary>
    public static class RodGrade
    {

        /// <summary>
        /// Sets up the RodGrade entity in the model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <exception cref="ArgumentNullException">Thrown when the modelBuilder parameter is null.</exception>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<RodGradeEntity>()
                .HasKey(e => new
                {
                    e.RodGradeId
                }).IsClustered(false);

            modelBuilder.Entity<RodGradeEntity>()
                .Property(e => e.RodGradeId).ValueGeneratedNever();
        }

    }
}
