using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="ESPWellDetailsEntity"/>.
    /// </summary>
    public static class ESPWellDetails
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

            modelBuilder.Entity<ESPWellDetailsEntity>()
                .HasKey(e => new
                {
                    e.NodeId
                })
                .IsClustered(true)
                .HasName("PK_tblESPWellDetails");
        }

    }
}
