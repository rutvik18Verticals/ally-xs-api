using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="GroupStandardColumnEntity"/> entity
    /// </summary>
    public static class GroupStandardColumn
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

            modelBuilder.Entity<GroupStandardColumnEntity>()
                .Property(e => e.Id)
                .HasMaxLength(4)
                .HasColumnName("Id");

            modelBuilder.Entity<GroupStandardColumnEntity>()
                .Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("Name");

            modelBuilder.Entity<GroupStandardColumnEntity>()
                .Property(e => e.Definition)
                .HasMaxLength(1998)
                .HasColumnName("Definition");
        }

    }
}
