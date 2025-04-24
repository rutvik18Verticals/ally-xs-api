using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="GroupStatusViewsColumnEntity"/> entity
    /// </summary>
    public class GroupStatusViewsColumn
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

            modelBuilder.Entity<GroupStatusViewsColumnEntity>()
                .HasKey(x => new
                {
                    x.ViewId,
                    x.ColumnId
                });

            modelBuilder.Entity<GroupStatusViewsColumnEntity>()
                .Property(e => e.ViewId)
                .IsRequired(true)
                .HasColumnName("ViewID");

            modelBuilder.Entity<GroupStatusViewsColumnEntity>()
                .Property(e => e.ColumnId)
                .IsRequired(true)
                .HasColumnName("ColumnID");

            modelBuilder.Entity<GroupStatusViewsColumnEntity>()
                .Property(m => m.Orientation)
                .HasDefaultValue(0);

            modelBuilder.Entity<GroupStatusViewsColumnEntity>()
                .Property(m => m.FormatId)
                .HasDefaultValue(0);
        }

    }
}
