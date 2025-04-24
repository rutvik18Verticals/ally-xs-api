using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Represents a class that handles the setup of the GroupStatusTable entity.
    /// </summary>
    public static class GroupStatusTable
    {
        /// <summary>
        /// Sets up the GroupStatusTable entity in the provided ModelBuilder.
        /// </summary>
        /// <param name="modelBuilder">The ModelBuilder instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when modelBuilder is null.</exception>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<GroupStatusTableEntity>(entity =>
            {
                entity.HasKey(e => e.TableId).IsClustered(false);
                entity.Property(e => e.TableId).ValueGeneratedNever();
            });
        }
    }
}
