using Microsoft.EntityFrameworkCore;
using System;
using Theta.XSPOC.Apex.Api.Data.Entity.RodLift;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="RodMotorSettingEntity"/> entity
    /// </summary>
    public static class RodMotorSettings
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

            modelBuilder.Entity<RodMotorSettingEntity>()
                .HasKey(k => new { k.Id })
                .IsClustered(true)
                .HasName("PK_tblMotorSettings");
        }

    }
}