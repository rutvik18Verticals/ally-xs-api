using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="CurveTypesEntity"/> entity
    /// </summary>
    public class CurveType
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

            modelBuilder.Entity<CurveTypesEntity>()
                .Property(e => e.Id)
                .IsRequired(true);

            modelBuilder.Entity<CurveTypesEntity>()
                .Property(e => e.PhraseId)
                .IsRequired(true);

            modelBuilder.Entity<CurveTypesEntity>()
                .Property(e => e.ApplicationTypeId)
                .IsRequired(true);
        }

    }
}
