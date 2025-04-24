using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="PlungerLiftDataHistoryEntity"/> entity.
    /// </summary>
    public class PlungerLiftDataHistory
    {

        /// <summary>
        /// This is the setup method to help build keys, indexes, and default values.
        /// </summary>
        /// <param name="modelBuilder">The <seealso cref="ModelBuilder"/>.</param>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<PlungerLiftDataHistoryEntity>()
                .HasKey(e => new
                {
                    e.NodeId,
                    e.Date
                }).HasName("PK_tblPlungerLiftDataHistory");
        }
    }
}
