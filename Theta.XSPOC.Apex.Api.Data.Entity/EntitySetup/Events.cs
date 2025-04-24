using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="EventsEntity"/> entity
    /// </summary>
    public class Events
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

            modelBuilder.Entity<EventsEntity>()
                .Property(e => e.EventId)
                .HasMaxLength(4)
                .IsRequired(true);

            modelBuilder.Entity<EventsEntity>()
                .Property(e => e.NodeId)
                .HasMaxLength(100)
                .IsRequired(true);

            modelBuilder.Entity<EventsEntity>()
                .Property(e => e.EventTypeId)
                .HasMaxLength(4)
                .IsRequired(true);

            modelBuilder.Entity<EventsEntity>()
                .Property(e => e.Date)
                .IsRequired(true);
        }

    }
}
