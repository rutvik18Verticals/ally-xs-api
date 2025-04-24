using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="GroupStatusUserViewEntity"/> entity
    /// </summary>
    public class GroupStatusUserView
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

            modelBuilder.Entity<GroupStatusUserViewEntity>()
                .HasKey(x => new
                {
                    x.ViewId,
                    x.UserId,
                    x.GroupName
                })
                .IsClustered(false)
                .HasName("PK_tblGroupStatusUserViews");

            modelBuilder.Entity<GroupStatusUserViewEntity>()
                .Property(e => e.ViewId)
                .IsRequired(true)
                .HasColumnName("ViewID");

            modelBuilder.Entity<GroupStatusUserViewEntity>()
                .Property(e => e.UserId)
                .HasMaxLength(50)
                .IsRequired(true)
                .HasColumnName("UserID");

            modelBuilder.Entity<GroupStatusUserViewEntity>()
                .Property(e => e.GroupName)
                .HasMaxLength(50)
                .IsRequired(true)
                .HasColumnName("GroupName");
        }

    }
}
