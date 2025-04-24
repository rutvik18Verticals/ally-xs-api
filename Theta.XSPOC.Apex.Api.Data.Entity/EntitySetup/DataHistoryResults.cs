using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="DataHistoryEntity"/> entity.
    /// </summary>
    public class DataHistoryResults
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

            modelBuilder.Entity<DataHistoryEntity>()
              .HasKey(k => new
              {
                  k.Date,
                  k.NodeID,
                  k.Address,
              }).HasName("PK_tblDataHistory")
              .IsClustered(true);

            modelBuilder.Entity<DataHistoryEntity>()
                .HasIndex(i => new
                {
                    i.NodeID,
                    i.Address,
                }).IsClustered(false);

            modelBuilder.Entity<DataHistoryEntity>()
                .HasIndex(i => new
                {
                    i.Address,
                }).IsClustered(false);
        }

    }
}
