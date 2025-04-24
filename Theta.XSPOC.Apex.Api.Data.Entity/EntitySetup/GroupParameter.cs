using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="GroupParameterEntity"/> entity.
    /// </summary>
    public class GroupParameter
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

            modelBuilder.Entity<GroupParameterEntity>()
                .HasKey(e => e.Id)
                .HasName("PK__tblGroupParamete__5B2E79DB");

            modelBuilder.Entity<GroupParameterEntity>()
                .Property(e => e.Id).ValueGeneratedNever();
        }

    }
}
