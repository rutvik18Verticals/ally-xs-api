using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="ESPWellMotorEntity"/> entity
    /// </summary>
    public class ESPWellMotors
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

            modelBuilder.Entity<ESPWellMotorEntity>()
                .HasKey(k => new
                {
                    k.NodeId,
                    k.ESPMotorId,
                    k.OrderNumber
                })
                .HasName("PK_tblESPWellMotors")
                .IsClustered(true);
        }

    }
}
