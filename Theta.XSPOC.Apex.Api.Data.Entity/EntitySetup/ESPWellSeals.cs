using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="ESPWellSealEntity"/> entity
    /// </summary>
    public class ESPWellSeals
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

            modelBuilder.Entity<ESPWellSealEntity>()
                .HasKey(k => new
                {
                    k.NodeId,
                    k.ESPSealId,
                    k.OrderNumber
                })
                .HasName("PK_tblESPWellSeals")
                .IsClustered(true);
        }

    }
}
