using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="PCSFDatalogRecordEntity"/> entity.
    /// </summary>
    public class PCSFDatalogRecord
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

            modelBuilder.Entity<PCSFDatalogRecordEntity>()
                .HasKey(e => new
                {
                    e.NodeId,
                    e.DatalogNumber,
                    e.LogDateTime
                }).HasName("PK_tblPCSFDatalogRecords");

        }
    }
}
