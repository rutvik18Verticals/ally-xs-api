using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="EventGroupsEntity"/> entity
    /// </summary>
    public class EventGroups
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

            modelBuilder.Entity<EventGroupsEntity>()
                .Property(e => e.EventTypeId)
                .HasMaxLength(4)
                .IsRequired(true);

            modelBuilder.Entity<EventGroupsEntity>()
                .Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired(true);

            modelBuilder.Entity<EventGroupsEntity>()
                .Property(e => e.AllowUserCreation)
                .IsRequired(true);
        }

    }
}
