using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="AnalyticsClassificationEntity"/> entity
    /// </summary>
    public class AnalyticsClassifications
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

            modelBuilder.Entity<AnalyticsClassificationEntity>()
                .HasKey(e => new
                {
                    e.NodeId,
                    e.StartDate,
                    e.EndDate,
                }).HasName("IX_tblAnalyticsClassifications_Node_Start_End");

            modelBuilder.Entity<AnalyticsClassificationEntity>()
                .HasKey(e => new
                {
                    e.Id,
                })
                .IsClustered(true)
                .HasName("PK_tblAnalyticsClassifications");

        }

    }
}
