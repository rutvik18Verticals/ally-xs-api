using System;
using Microsoft.EntityFrameworkCore;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="ParamStandardTypesEntity"/> entity
    /// </summary>
    public class ParamStandardType
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

            modelBuilder.Entity<ParamStandardTypesEntity>()
                .HasKey(k => new { k.ParamStandardType })
                .IsClustered(true)
                .HasName("PK_tblStandardParamTypes");

            modelBuilder.Entity<ParamStandardTypesEntity>()
                .Property(m => m.PhraseId)
                .HasDefaultValue(0);

            modelBuilder.Entity<ParamStandardTypesEntity>()
                .Property(m => m.UnitTypeId)
                .HasDefaultValue(0);
        }

    }
}
