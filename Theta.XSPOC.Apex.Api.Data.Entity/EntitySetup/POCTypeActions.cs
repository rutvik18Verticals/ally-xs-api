using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="POCTypeActionsEntity"/>.
    /// </summary>
    public static class POCTypeActions
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

            modelBuilder.Entity<POCTypeActionsEntity>()
                .HasKey(k => new
                {
                    k.POCType,
                    k.ControlActionId,
                })
                .IsClustered(true)
                .HasName("PK_tblPOCTypeActions");
        }

    }
}
