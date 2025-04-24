using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="RodEntity"/> entity
    /// </summary>
    public class RodString
    {

        /// <summary>
        /// Does additional setup using the <seealso cref="ModelBuilder"/>. Functionality like clustered indexes that
        /// are not supported by DataAnnotations can be built here using fluent syntax.
        /// </summary>
        /// <param name="modelBuilder">The <seealso cref="ModelBuilder"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="modelBuilder"/> is null.
        /// </exception>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<RodEntity>()
            .HasKey(x => new { x.NodeId, x.RodNum })
            .IsClustered(false)
            .HasName("PK_tblRods");
        }

    }
}
