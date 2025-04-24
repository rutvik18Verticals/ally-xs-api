using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="XDiagResultEntity"/> entity.
    /// </summary>
    public static class XdiagResult
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

            modelBuilder.Entity<XDiagResultEntity>()
                .HasKey(k => new
                {
                    k.NodeId,
                    k.Date
                })
                .HasName("aaaaatblXDiagResults_PK")
                .IsClustered(false);

            modelBuilder.Entity<XDiagResultEntity>()
                .Property(e => e.NodeId)
                .HasMaxLength(50)
                .IsRequired(true)
                .HasColumnName("NodeID");

            modelBuilder.Entity<XDiagResultEntity>()
                .Property(e => e.Date)
                .IsRequired(true)
                .HasColumnType("datetime")
                .HasColumnName("Date");
        }

    }
}
