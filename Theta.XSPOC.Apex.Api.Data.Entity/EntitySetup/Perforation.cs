using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="PerforationEntity"/> entity
    /// </summary>
    public class Perforation
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

            modelBuilder.Entity<PerforationEntity>()
                .HasKey(k => new
                {
                    k.NodeId,
                    k.Depth
                })
                .IsClustered(false)
                .HasName("PK_tblPerforations");

            modelBuilder.Entity<PerforationEntity>()
                .Property(e => e.Interval)
                .IsRequired(true);
        }

    }
}
