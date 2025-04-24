using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="GroupStatusViewEntity"/> entity
    /// </summary>
    public class GroupStatusView
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

            modelBuilder.Entity<GroupStatusViewEntity>()
                .HasKey(x => new
                {
                    x.ViewName,
                    x.UserId
                })
                .IsClustered(false)
                .HasName("PK_tblGroupStatusViews");

            modelBuilder.Entity<GroupStatusViewEntity>()
                .Property(e => e.ViewName)
                .HasMaxLength(50)
                .IsRequired(true)
                .HasColumnName("ViewName");

            modelBuilder.Entity<GroupStatusViewEntity>()
                .Property(e => e.UserId)
                .HasMaxLength(50)
                .IsRequired(true)
                .HasColumnName("UserID");
        }

    }
}
